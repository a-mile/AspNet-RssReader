using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Xml.Linq;
using AspNet_RssReader_Domain.Entities;

namespace AspNet_RssReader_WebUI.Validation
{
    public class RssLinkValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string url = value.ToString();

                Uri uriResult;
                bool isUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (isUrl)
                {
                    string xml;

                    using (var webClient = new WebClient())
                    {
                        try
                        {
                            var data = webClient.DownloadData(url);
                            var contentType = new ContentType(webClient.ResponseHeaders["Content-Type"]);
                            xml = Encoding.GetEncoding(contentType.CharSet).GetString(data);
                            xml = WebUtility.HtmlDecode(xml);
                        }
                        catch (Exception)
                        {
                            return new ValidationResult("An error when opening page");
                        }
                    }

                    xml = xml.Replace("&", "&amp;");

                    try
                    {
                        XDocument.Parse(xml);
                    }
                    catch (Exception)
                    {
                        return new ValidationResult("This page is not rss channel");
                    }    
                    
                    return ValidationResult.Success;
                }

                return new ValidationResult("Link is not valid");
            }

            return new ValidationResult(validationContext.DisplayName + " is required");
        }
    }
}