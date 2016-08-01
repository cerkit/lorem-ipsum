using System.Configuration;
using System.Diagnostics;
using System.Net;

namespace LoremIpsum
{
    class Program
    {
        static void Main(string[] args)
        {
            // create our factory
            var loremIpsum = new LoremIpsumFactory()
            {
                ApiKey = ConfigurationManager.AppSettings["lipsumApiKey"]
            };

            // the simplest example (get 5 words)
            var simplestExample = loremIpsum.Create(LipsumType.Words, 5);
            Debug.WriteLine(simplestExample.Feed.Lipsum);

            // a simple example (get 2 paragraphs and don't start with Lorem Ipsum)
            var simpleExample = loremIpsum.Create(LipsumType.Paragraphs, 2, false);
            Debug.WriteLine(simpleExample.Feed.Lipsum);

            // an example of using custom Proxy authentication to get 3 words starting with "Lorem Ipsum"
            var proxyCredentials = new NetworkCredential("username", "password");
            var customProxyAuth = loremIpsum.Create(LipsumType.Words, 3, true, proxyCredentials);
            Debug.WriteLine(customProxyAuth.Feed.Lipsum);
        }
    }
}
