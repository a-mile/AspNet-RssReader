using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using AspNet_RssReader_Domain.Concrete;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.Extensions;
using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.ViewModels
{    
    public class SourceViewModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public int? CategoryId { get; set; }

        public IEnumerable<Category> Categories
        {
            get
            {
                var currentUser = HttpContext.Current.GetCurrentUser();
                var userCategories = currentUser.Categories;

                return userCategories;
            }
        }

        public IEnumerable<SelectListItem> CategoriesSelectList => new SelectList(Categories, "Id", "Name", CategoryId);
    }

    [Validator(typeof(SourceViewModelValidator))]
    public class CreateSourceViewModel : SourceViewModel
    {
    }

    [Validator(typeof(SourceViewModelValidator))]
    public class UpdateSourceViewModel : SourceViewModel
    {
        public int Id { get; set; }
    }

    public class DeleteSourceViewModel : SourceViewModel
    {
        public int Id { get; set; }
    }

    public class SourceViewModelValidator:AbstractValidator<SourceViewModel>
    {
        public SourceViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Source name cannot be empty")
                .Length(0, 15).WithMessage("Maximum length of source name is 15")
                .Must(UniqueName).WithMessage("There is already source with this name");
            RuleFor(x => x.Link).NotEmpty().WithMessage("Link cannot be empty")
                .Length(0, 200).WithMessage("Maximum length of link is 200")
                .Must(Link).WithMessage("This is not valid link")
                .Must(RssChannel).WithMessage("This is not valid rss channel")
                .Must(UniqueLink).WithMessage("There is already source with this link");
        }

        private bool UniqueName(string name)
        {
            var currentUser = HttpContext.Current.GetCurrentUser();

            return currentUser.Sources.All(x => x.Name != name);
        }

        private bool UniqueLink(string link)
        {
            var currentUser = HttpContext.Current.GetCurrentUser();

            return currentUser.Sources.All(x => x.Link != link);
        }

        private bool Link(string link)
        {
            Uri uriResult;
            bool isUrl = Uri.TryCreate(link, UriKind.Absolute, out uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return isUrl;
        }

        private bool RssChannel(string link)
        {
            string xml;

            using (var webClient = new WebClient())
            {
                try
                {
                    var data = webClient.DownloadData(link);
                    var contentType = new ContentType(webClient.ResponseHeaders["Content-Type"]);
                    xml = Encoding.GetEncoding(contentType.CharSet).GetString(data);
                    xml = WebUtility.HtmlDecode(xml);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            xml = xml.Replace("&", "&amp;");

            try
            {
                XDocument.Parse(xml);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}