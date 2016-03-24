using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;

namespace LoremIpsum
{
    public static class LoremIpsumUtil
    {
        public static readonly string LIPSUM_JSON_URL = "http://lipsum.com/feed/json?amount={0}&start={1}&what={2}";

        /// <summary>
        /// Gets a new Lorem Ipsum result object from the JSON endpoint at http://lipsum.com.
        /// </summary>
        /// <param name="lipsumType">The type of result to get from the api. See <see cref="LipsumType"/></param>
        /// <param name="amount">An integer value representing how many of the given type to receive from the service.</param>
        /// <param name="startWithLoremIpsum">true to start with the "Lorem Ipsum" text or false to get a random result.</param>
        /// <param name="proxyCredentials">Use <see cref="System.Net.NetworkCredential"/>, <see cref="System.Net.CredentialCache.DefaultCredentials"/>, or <see cref="System.Net.CredentialCache.DefaultNetworkCredentials"/> to send to your proxy server.</param>
        /// <returns>An instance of the <see cref="LoremIpsum"/> class representing the data that was returned from the api.</returns>
        public static LoremIpsum GetNewLipsum(LipsumType lipsumType, int amount, bool startWithLoremIpsum = true, ICredentials proxyCredentials = null)
        {
            string requestUrl = string.Format(LIPSUM_JSON_URL, amount, startWithLoremIpsum ? "yes" : "no", lipsumType.GetDescription());

            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

            if (proxyCredentials != null)
            {
                request.Proxy.Credentials = proxyCredentials;
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));

                StreamReader sr = new StreamReader(response.GetResponseStream());

                return JsonConvert.DeserializeObject<LoremIpsum>(sr.ReadToEnd());
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

        /// <summary>
        /// Retrieve the description value from this enum.
        /// </summary>
        /// <typeparam name="T">Automatically provided when calling as an extension.</typeparam>
        /// <param name="e">Represents the current instance of an enum that this method is being called from.</param>
        /// <returns></returns>
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            Array values = System.Enum.GetValues(e.GetType());
            string description = null;

            foreach (int val in values)
            {
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var type = e.GetType();
                    var memInfo = type.GetMember(Enum.GetName(type, val));
                    var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    description = ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return description;
        }
    }
}
