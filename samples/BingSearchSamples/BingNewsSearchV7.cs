// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;

// This sample makes a call to the Bing News Search API with a query word and returns related news.

namespace BingNewsSearch
{
    class Program
    {
        // Add your Bing Search V7 subscription key to your environment variables.
        static string accessKey = Environment.GetEnvironmentVariable("BING_SEARCH_V7_SUBSCRIPTION_KEY");
        // Add your Bing Search V7 endpoint to your environment variables.
        static string endpoint = Environment.GetEnvironmentVariable("BING_SEARCH_V7_ENDPOINT") + "/v7.0/news/search";

        const string query = "Microsoft";

        static void Main()
        {
            // Create dictionary to store a few extracted headers
            Dictionary<String, String> relevantHeaders = new Dictionary<String, String>();

            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Searching news for: " + query);

            // Construct the URI of the search request
            var uriQuery = endpoint + "?q=" + Uri.EscapeDataString(query);

            // Perform the Web request and get the response
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Extract Bing HTTP headers
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    relevantHeaders[header] = response.Headers[header];
            }

            Console.WriteLine("\nRelevant HTTP Headers:\n");
            foreach (var header in relevantHeaders)
                Console.WriteLine(header.Key + ": " + header.Value);

            Console.WriteLine("\nJSON Response:\n");
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            Console.WriteLine(JsonConvert.SerializeObject(parsedJson, Formatting.Indented));

            Console.ReadLine();
        }
    }
}