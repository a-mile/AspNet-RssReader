using System;
using System.IO;
using System.Net;
using AspNet_RssReader_Domain.Abstract;
using Newtonsoft.Json.Linq;

namespace AspNet_RssReader_Domain.Concrete
{
    public class MercuryArticleExtractor : IArticleExtractor
    {
        private readonly string apiKey = "BwxyrHfL7GpEleQDt5CPasVmVNS80JPNOhZclJnZ";
        public string GetArticleLeadImageUrl(string articleUrl)
        {
            return GetMercuryTokenString(articleUrl, "lead_image_url");
        }

        public string GetArticleContent(string articleUrl)
        {
            return GetMercuryTokenString(articleUrl, "content");
        }

        private string GetMercuryTokenString(string articleUrl, string tokenName)
        {
            string url = "https://mercury.postlight.com/parser?url=" + articleUrl;
            WebRequest request = WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Headers.Add("x-api-key", apiKey);

            var responseStream = request.GetResponse().GetResponseStream();

            StreamReader streamReader = new StreamReader(responseStream);
            string responseString = streamReader.ReadToEnd();

            try
            {
                JToken token = JObject.Parse(responseString);
                var currentToken = token.SelectToken(tokenName);

                if (currentToken != null)
                    return currentToken.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
