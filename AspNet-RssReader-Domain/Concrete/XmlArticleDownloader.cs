using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Xml.Linq;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Entities;
using CSharp_HtmlParser_Library.HtmlDocumentStructure;

namespace AspNet_RssReader_Domain.Concrete
{
    public class XmlArticleDownloader : IArticleDownloader
    {
        private int _descriptionLength = 160;
        public IEnumerable<Article> GetArticles(string sourceLink)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(sourceLink, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (result)
            {
                string xml;

                using (var webClient = new WebClient())
                {
                    try
                    {
                       var data = webClient.DownloadData(sourceLink);
                       var contentType = new ContentType(webClient.ResponseHeaders["Content-Type"]);
                       xml = Encoding.GetEncoding(contentType.CharSet).GetString(data);
                       xml = WebUtility.HtmlDecode(xml);
                    }
                    catch (Exception)
                    {
                        return Enumerable.Empty<Article>();
                    }                                        
                }

                xml = xml.Replace("&", "&amp;");

                XDocument document = XDocument.Parse(xml);

                var articles = document.Descendants("item").Select(x => new Article()
                {
                    Title = x.Element("title").Value,
                    Description = FormatDescritption(x.Element("description").Value),
                    Link = x.Element("link").Value,
                    PubDate = ParseDate(x.Element("pubDate").Value),
                });

                return articles;
            }

            return Enumerable.Empty<Article>();
        }

        private string FormatDescritption(string description)
        {
            HtmlDocument document = new HtmlDocument(description);
            document.Parse();

            var links = document.RootNode.Descendants.Where(x => x.Name == "a");
            foreach (var link in links)
            {
                document.RootNode.DeleteNode(link);
            }

            string result = document.RootNode.InnerText;
            if (result.Length > _descriptionLength)
            {
                result = result.Substring(0, _descriptionLength - 3);
                result += "...";
            }

            return result;
        }

        private DateTime ParseDate(string date)
        {
            DateTime result;
            return DateTime.TryParse(date, out result) ? result : DateTime.MinValue;
        }
    }
}
