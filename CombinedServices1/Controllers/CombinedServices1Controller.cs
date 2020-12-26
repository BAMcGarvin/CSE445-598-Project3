/*
 * Description: This is a Combined Service Controller as our group did not 
 * have enough Server Pages for all services. This controller is responsible for
 * implementing 3 services. It includes our Natural Hazard Service (returns the Earthquake
 * Index for a given latitude and longitude), NewsFocus (allows for web searches of topics 
 * important to our users), and it implements a ZipCodeDetails service in order to specifically
 * extract latitude and longitude coordinates from a zip code.
 * 
 * Project 3 (Assignments 5 & 6)
 * Team 61
 * CSE 445/598 Distributed Software Development
 * Session C Fall 2020
 * Dr. Yinong Chen
 * 
 * Author:Bradley McGarvin
 * 
 * References: 7th edition Service-Oriented Computing and System Integration, Chapter 3 and 7
 * WebStrar tutorial, Newtonsoft.Json, Newtonsoft.Json.Linq, Lecture 10 all slides.
 * 
 * NaturalHazard Service References: 
 * API Information: https://earthquake.usgs.gov/fdsnws/event/1/ 
 * Additional:      https://docs.microsoft.com/en-us/dotnet/api/system.net.webclient.downloadstring?view=netcore-3.1 
 * which was used to read more about web client.
 * 
 * NewsFocus Services References: 
 * API Information: https://rapidapi.com/contextualwebsearch/api/web-search/endpoints
 *                  https://contextualweb.io/news-api/
 *                  https://contextualweb.io/search-apis/documentation/
 * Additional Required Packages to use Service: 
 *                  https://www.nuget.org/packages/Unirest-API/
 *                  
 * Top10Words Services References: 
 *                  https://www.nuget.org/packages/HtmlAgilityPack/
 *                  StackOverflow and geeksforgeeks
 *                  
 * ZipCode Details Service References:
 *                  https://www.zippopotam.us/
 *                  https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netcore-3.1
 * 
 * 
 * This RESTful service is uploaded to http://webstrar61.fulton.asu.edu/page0
 * WSDL Service Top10Words uploaded to http://webstrar61.fulton.asu.edu/page2
 */

using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using unirest_net.http;

namespace CombinedServices1.Controllers
{
    public class CombinedServices1Controller : ApiController
    {

        /*
         * This control is meant to call the RESTful service provided by USGS based on desired ranges from our users.
         * This will allow the user to control the date range for the index, the specific maximum radius from the given
         * Latitude and Longitude Coordinates and it will also allow the user to specify the minimum magnitude for earthquakes
         * when determining the respective index for that territory.
         * Inputs: (string)starting date, (decimal)latitude/longitude, (double)radius and magnitude
         * Outputs: (decimal) Earthquake Index
         * 
         * This is ONE service
         */
        public decimal GetEarthQuakeHazard(string start, decimal latitude, decimal longitude, double radius, double magnitude)
        {

            string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");     // strore current date in correct format


            Uri baseUri = new Uri("https://earthquake.usgs.gov/fdsnws/event/1");    // establish our baseUri

            // UriTemplate to help to bind/construct our completeUri
            UriTemplate myTemplate = new UriTemplate("count?starttime={startDate}&endtime={todaysDate}&latitude={latitude}&longitude={longitude}&maxradiuskm={radius}&minmagnitude={magnitude}");

            Uri completeUri = myTemplate.BindByPosition(baseUri, start, todaysDate, latitude.ToString(), longitude.ToString(), radius.ToString(), magnitude.ToString());

            WebClient channel = new WebClient();    // create a channel

            //            byte[] abc = channel.DownloadData(completeUri);

            string responseString = channel.DownloadString(completeUri);    // download the "data" directly as a string

            //           System.IO.Stream strm = new System.IO.MemoryStream(abc);

            //           DataContractSerializer obj = new DataContractSerializer(typeof(string));

            //           string label = obj.ReadObject(strm).ToString();
            return Convert.ToDecimal(responseString);                       // convert our string to decimal

        }


        /*
         * This control is used specifically to gather details about an area based on the zipcode
         * It will receive a zipcode as input and be used to only gather the latitude and longitude 
         * coordinates. This service will be complimentary to many of our services that require 
         * latitude and longitude inputs, making it easier for our users to process search queries.
         * Input: (string) zipCode
         * Output: (string[]) Latitude and Longitude Coordinates
         * 
         * This is a Second Service
         */
        public string[] GetZipCodeDetails(string zipCode)
        {


            Uri baseUri = new Uri("http://api.zippopotam.us/");    // establish our baseUri

            // UriTemplate to help to bind/construct our completeUri
            UriTemplate myTemplate = new UriTemplate("us/{zipCode}");

            Uri completeUri = myTemplate.BindByPosition(baseUri, zipCode);

            WebClient channel = new WebClient();    // create a channel

            string responseString = channel.DownloadString(completeUri);    // download the "data" directly as a string


            var responses = (JObject)JsonConvert.DeserializeObject(responseString);

            var parsing = JObject.Parse(responses.ToString());
            var jobj_longitude = parsing.SelectToken("places[*].longitude");

            var jobj_latitude = parsing.SelectToken("places[*].latitude");


            string latitude = jobj_latitude.ToString();
            string longitude = jobj_longitude.ToString();


            string[] output = new string[2];
            output[0] = latitude;
            output[1] = longitude;

            return output;

        }


        /*
         * NewsFocus is a 3rd service that allows a user to search the web for news stories
         * related to the topic(s) of their choice. This API was provided by API and has a HARD LIMIT
         * of 500 search per month. The API-Key is not to be distributed. Additional code that is not 
         * used was provided directly by RapidAPI and is inserted here for future use (not for this project)
         * Input: string of topics
         * Output: array of strings (urls)
         */
        public string[] GetNewsFocus(string topics)
        {

            // The following string value is my customer API key provided by RAPIDAPI =  X-RapidAPI-Key.
            // This API-Key is not to be shared or redistributed per RAPIDAPI and Contextual Web Search
            string X_RapidAPI_Key = GET YOUR OWN API KEY!!!!;


            /*
             * The query parameters:  q will represent the topics our user is searching
             * pageNumber = 1 to pull up the first page of relevant search results
             * pageSize = 10 to limit the amount of URLs provide on the first page to 10
             * autoCorrect is enabled to provide suggested spelling for misspelled searches
             * safeSearch is disabled so that all search results can be displayed
             */
            string q = topics; //the search query
            int pageNumber = 1; //the number of requested page
            int pageSize = 10; //the size of a page
            bool autoCorrect = true; //autoCorrectspelling
            bool safeSearch = false; //filter results for adult content

            // Perform the web request and get the response using Unirest package provided by
            // https://www.nuget.org/packages/Unirest-API/
            var response = Unirest.get(string.Format("https://contextualwebsearch-websearch-v1.p.rapidapi.com/api/Search/WebSearchAPI?q={0}&amp;;pageNumber={1}&amp;p;pageSize={2}&amp;;autoCorrect={3}&amp;;safeSearch={4}", q, pageNumber, pageSize, autoCorrect, safeSearch)).header("X-RapidAPI-Key", X_RapidAPI_Key).asJson<string>();


            //Get the ResponseBody as a JSON
            dynamic jsonBody = JsonConvert.DeserializeObject(response.Body);

            //Parse the results(below)

            //Get the numer of items returned
            int totalCount = (int)jsonBody["totalCount"];


            List<string> webUrls = new List<string>();          // used to add each URL to a String List

            /*            //OPTIONAL, MAY IMPLEMENT TO DISPLAY MORE INFORMATION
                        List<string> titles = new List<string>();           // used to add each URL title to a String List
                        List<string> publishDates = new List<string>();     // used to add each publishDate to a String List
            */

            //Go over each resulting item, NOT ALL ITEMS/METADATA will be used
            foreach (var webPage in jsonBody["value"])
            {
                //Get the web page metadata
                string url = webPage["url"].ToString();
                string title = webPage["title"].ToString();                 // not for project, provided by RapidAPI
                string description = webPage["description"].ToString();     // not for project, provided by RapidAPI
                string keywords = webPage["keywords"].ToString();           // not for project, provided by RapidAPI
                string provider = webPage["provider"]["name"].ToString();   // not for project, provided by RapidAPI
                DateTime datePublished = DateTime.Parse(webPage["datePublished"].ToString());   // not for project, provided by RapidAPI

                //Get the web page image (if exists)
                string imageUrl = webPage["image"]["url"].ToString();       // not for project, provided by RapidAPI
                int imageHeight = (int)webPage["image"]["height"];          // not for project, provided by RapidAPI
                int imageWidth = (int)webPage["image"]["width"];            // not for project, provided by RapidAPI

                //Get the web page image thumbail (if exists)
                string thumbnail = webPage["image"]["thumbnail"].ToString();    // not for project, provided by RapidAPI
                int thumbnailHeight = (int)webPage["image"]["thumbnailHeight"]; // not for project, provided by RapidAPI
                int thumbnailidth = (int)webPage["image"]["thumbnailWidth"];    // not for project, provided by RapidAPI


                webUrls.Add(url);

                /*                //OPTIONAL, CURRENTLY NOT USING
                                titles.Add(title);
                                publishDates.Add(datePublished.ToString());
                */

            }
            var outputStr = String.Join(", ", webUrls.ToArray());       // no longer used/needed

            string[] urlOutput = new string[webUrls.Count];
            /*
                        string[] titleOutput = new string[webUrls.Count];
                        string[] publisherOutput = new string[webUrls.Count];
            */

            for (int i = 0; i < webUrls.Count; i++)
            {
                urlOutput[i] = webUrls[i];
            }

            return urlOutput;


        }



    }
}
