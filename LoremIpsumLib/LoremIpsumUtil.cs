using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LoremIpsum
{
    public class LoremIpsumFactory
    {
        public static readonly string APP_KEY_LIPSUM_JSON_API_ENDPOINT = "lipsumJsonApiEndpoint";

        public LoremIpsumFactory() { }

        public string ApiKey { get; set; }
        
        /// <summary>
        /// The full Url to the JSON API for Lipsum.com.
        /// </summary>
        private string LipsumJsonEndpoint
        {
            get
            {
                if (this.ApiKey != null)
                {
                    return string.Format(ConfigurationManager.AppSettings[APP_KEY_LIPSUM_JSON_API_ENDPOINT], this.ApiKey);
                }
                else
                {
                    throw new InvalidDataException("The ApiKey property cannot be null");
                }
            }
        }

        /// <summary>
        /// Gets a new Lorem Ipsum result object from the JSON endpoint at http://lipsum.com.
        /// </summary>
        /// <param name="lipsumType">The type of result to get from the api. See <see cref="LipsumType"/></param>
        /// <param name="amount">An integer value representing how many of the given type to receive from the service.</param>
        /// <param name="startWithLoremIpsum">true to start with the "Lorem Ipsum" text or false to get a random result.</param>
        /// <param name="proxyCredentials">Use <see cref="System.Net.NetworkCredential"/>, <see cref="System.Net.CredentialCache.DefaultCredentials"/>, or <see cref="System.Net.CredentialCache.DefaultNetworkCredentials"/> to send to your proxy server.</param>
        /// <returns>An instance of the <see cref="LoremIpsum"/> class representing the data that was returned from the api.</returns>
        public LoremIpsum Create(LipsumType lipsumType, int amount, bool startWithLoremIpsum = true, ICredentials proxyCredentials = null)
        {
            // build the JSON request URL
            string requestUrl = string.Format(this.LipsumJsonEndpoint, amount, startWithLoremIpsum ? "yes" : "no", lipsumType.GetDescription());

            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

            if (proxyCredentials != null)
            {
                request.Proxy.Credentials = proxyCredentials;
            }
            else 
            {
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));

                StreamReader sr = new StreamReader(response.GetResponseStream());
                JsonTextReader tr = new JsonTextReader(sr);
                JsonSerializer ser = new JsonSerializer();

                return ser.Deserialize<LoremIpsum>(tr);
            }
        }

        public async Task<LoremIpsum> CreateAsync(LipsumType lipsumType, int amount, bool startWithLoremIpsum = true, ICredentials proxyCredentials = null)
        {
            string requestUrl = string.Format(this.LipsumJsonEndpoint, amount, startWithLoremIpsum ? "yes" : "no", lipsumType.GetDescription());

            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

            if (proxyCredentials != null)
            {
                request.Proxy.Credentials = proxyCredentials;
            }

            var response = await request.GetResponseAsync();

            StreamReader sr = new StreamReader(response.GetResponseStream());
            JsonTextReader tr = new JsonTextReader(sr);
            JsonSerializer ser = new JsonSerializer();

            return ser.Deserialize<LoremIpsum>(tr);
        }
    }

    /// <summary>
    /// The type of result to retrieve from the JSON endpoint.
    /// </summary>
    public enum LipsumType
    {
        [Description("words")]
        Words,
        [Description("paras")]
        Paragraphs,
        [Description("bytes")]
        Bytes,
        [Description("lists")]
        Lists
    }
}
