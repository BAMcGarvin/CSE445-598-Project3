/*
 * Description: This service is used to convert a web page from whatever format
 * its information may be in and switch it to html. Then we only get the content 
 * works, omitting the tags and short 3 letter words. This is a required service
 * that we plan to include with our NewsFocus application to help evaluate the information
 * on web pages easier/quicker.
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
 * WebStrar tutorial, a lot of Microsoft and stack overflow forums for how to use Regix and HtmlAgiligyPack
 * https://www.nuget.org/packages/HtmlAgilityPack/
 * Lecture 10 Slided 30 - 45.
 * 
 */

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Top10WordsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }


        /*
         * Service Call / Will be API Controller for RESTful Service
         * Input: string in the form of a url
         * Output: is an array of strings, provide a list of top 10 words in 
         * descending order.
         */
        public string[] Top10Words(string url)
        {
            // GetWebContentService.ServiceClient objects = new GetWebContentService.ServiceClient();
            //String web_content=objects.GetWebContent(url);

            string[] arr = new string[10];
            

            // instantiate an HTML web object to store an HTML file
            HtmlWeb web_obj = new HtmlWeb();

            // obtain html document and find the first node to indicate body
            var htmlDoc = web_obj.Load(url);
            HtmlNode n = htmlDoc.DocumentNode.SelectSingleNode("//body");

            // get the text between that start and end tags, omit null and whitespace
            var content = n.InnerText;                                  
            var contentWords = Regex.Split(content, @"[\W\d]+").Where(c => !String.IsNullOrWhiteSpace(c));

            // convert all content words to lowercase string and sort the string in descending order
            Regex.Split(contentWords.ToString().ToLower(), @"\W+")
                       .Where(s => s.Length > 3)
                       .GroupBy(s => s)
                       .OrderByDescending(g => g.Count());


            // for string in content words we want to send to an array of strings separated by a new line
            // once at 10, we stop and print the array to the screen.
            int k = 0;
            foreach (var s in contentWords)
            {
                String word = s.ToString();
                arr[k] = word.Split('\n')[0].Replace("{ Word =", String.Empty);
                k++;
                if (k == 10)
                    break;
            }

            return arr;
        }
        
    }
}
