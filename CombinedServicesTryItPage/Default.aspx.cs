/*
 * Description: This is the Master TryIt page for all services created by Bradley McGarvin.
 * The tryit page is meant to allow our users to test each service in succession. It will first
 * start by asking our user to input a valid zipcode (error handling enabled). This zip code will
 * be converted to specific latitude and longitude coordinates. These coordinates will then be used 
 * in order to invoke our NaturalHazards (Earthquake Index) service. If a user trys to invoke the 
 * service before converting a zip code, they will receive an error message. At the bottom of the
 * page will be our NewsFocus Search bar, allowing our user to search any topic they'd like. In return,
 * they will recieve the top 10 urls related to their search. Lastly, after invoking this service,
 * the user will be given the option to analyze any URL they select using our Top10Words service. 
 * This service will look at HTML script of the webpage and retrieve the top 10 words from the inner 
 * text of the body, essentially eleminated tags along with whitespace.
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
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WebGrease.Css.Extensions;

namespace CombinedServicesTryItPage
{
    public partial class _Default : Page
    {

        string startDate = "1919-10-16";                            // set a default start date
        string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");    // store todays date to help when accessing service data (i.e. earthquake indexes)
        string urlSelection = "";   // used to store our users url selection for our Top10Words Service
        int dateIndex = 0;          // used to adjust the start date based on user selection from date range drowpdown
        int radiusSwitch = 0;       // used to adjust the radius based on user selection from the radius dropdown
        int magSwitch = 0;          // used to adjust the magnitude based on user selection
        double radius = 16.0934;    // initialize starting radius to 10 miles (16.0934 km)
        double magnitude = 2.5;     // initialize our minium earthquake magnitude
        int listIndex = 10;         // initialize our listIndex to a value outside of available indices 

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*
         * Keeps track of what Index is selected by our user for date range for 
         * our NaturalHazards (Earthquake Index) service provided by USGS. The date 
         * range calculatioin is done for our users. It will first look at the current 
         * date and based on our users selection, subtract a certain amount of years from
         * that date and use the new date as the "Start Date" for our Earthquake Index
         * Analysis.
         */
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateIndex = DropDownList1.SelectedIndex;   // get the index that is currently selected


            // arithmic switch to change the year based on the the current date
            switch (dateIndex)
            {
                // 10 years
                case 0:
                    var myDate0 = DateTime.Now;
                    var uriStartDate0 = myDate0.AddYears(-10);
                    startDate = uriStartDate0.ToString("yyyy-MM-dd");
                    break;

                // 25 years
                case 1:
                    var myDate1 = DateTime.Now;
                    var uriStartDate1 = myDate1.AddYears(-25);
                    startDate = uriStartDate1.ToString("yyyy-MM-dd");
                    break;

                // 50 years
                case 2:
                    var myDate2 = DateTime.Now;
                    var uriStartDate2 = myDate2.AddYears(-50);
                    startDate = uriStartDate2.ToString("yyyy-MM-dd");
                    break;

                // 100 years
                case 3:
                    var myDate3 = DateTime.Now;
                    var uriStartDate3 = myDate3.AddYears(-100);
                    startDate = uriStartDate3.ToString("yyyy-MM-dd");
                    break;
            }

        }

        /*
         * Keeps track of what Index is selected by our user for Radius for 
         * our NaturalHazards (Earthquake Index) service provided by USGS. The radius
         * is provided to our users in miles and a switch is used to convert them to 
         * km (required for our Earthquake service), but not neccessary. 
         */
        protected void DropDownList_Radius_SelectedIndexChanged(object sender, EventArgs e)
        {
            radiusSwitch = DropDownList_Radius.SelectedIndex;   // gets currently selected radius index

            // arithmic switch that converts the selection in miles to kilometers
            switch (radiusSwitch)
            {
                // 10 miles in km is 16.0934
                case 0:
                    radius = 16.0934;
                    break;

                // 25 miles in km is 40.2336
                case 1:
                    radius = 40.2336;
                    break;

                // 50 miles in km is 80.4672
                case 2:
                    radius = 80.4672;
                    break;

                // 100 miles in km is 160.934
                case 3:
                    radius = 160.934;
                    break;

                default:
                    break;
            }
        }

        /*
         * Keeps track of what Index is selected by our user for Minimum Magnitude for 
         * our NaturalHazards (Earthquake Index) service provided by USGS.
         */
        protected void DropDownList_Magnitude_SelectedIndexChanged(object sender, EventArgs e)
        {
            magSwitch = DropDownList_Magnitude.SelectedIndex;   // get currently selected mag index

            // switch to match the selection of the user.
            switch (magSwitch)
            {
                // 2.5
                case 0:
                    magnitude = 2.5;
                    break;

                // 3.0
                case 1:
                    magnitude = 3.0;
                    break;

                // 3.5
                case 2:
                    magnitude = 3.5;
                    break;

                // 4.0
                case 3:
                    magnitude = 4.0;
                    break;

                // 4.5
                case 4:
                    magnitude = 4.5;
                    break;

                // 5.0
                case 5:
                    magnitude = 5.0;
                    break;

                // 5.5
                case 6:
                    magnitude = 5.5;
                    break;

                // 6.0
                case 7:
                    magnitude = 6.0;
                    break;

                default:
                    break;
            }
        }

        /*
         * Upone button click, we will first make sure the textbox is not empty 
         * and then we will make sure whatever is inputted is actually a valid expression
         * using Regular Expressions https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netcore-3.1
         * As long as the zip code is valid, we will proceed to get the latitude and longitude coordinates
         */
        protected void Convert_Zip_Btn_Click(object sender, EventArgs e)
        {

            Boolean isValid = true;                         // used for error checking
            string zipCode = "";                            // string to store text for textbox1

            /*
             * ^ = matches beginning of line
             * \d{5} = match any decimal digit (0-9) maximum length = 5(or match exactly 5 times)
             * (...) = Capture matched text in parentheses
             * ?: = Non-Capturing Group
             * [-\s] = this groups white space inidators suchs as space, tab, \r & \n
             * \d{4} = match any decimal digit (0-9) exactly 4 times
             * ? = matches the characte before the ? zero or one times
             * $ = matches end of line
             * https://www.computerhope.com/jargon/r/regex.htm
             * 
             * What this expression does is it starts with the beginning of the string and 
             * it trys to match any decimal (0-9) 5 times . . . omitting things like white space.
             * Using RegEx allows me to simply check decimal without having to check for all characters.
             * I added the \d{4})?$ just incase the zip code service I used actually can convert zip codes
             * with the postal standard.
             */
            var zipValidation = @"^\d{5}(?:[-\s]\d{4})?$";

            if (TextBox1.Text != string.Empty && TextBox1.Text.Length == 5)
            {
                zipCode = TextBox1.Text.ToString();

               // if there is not a match . . . FALSE
                if(!Regex.Match(zipCode, zipValidation).Success)
                {
                    isValid = false;                    
                }
                else
                {
                    isValid = true;
                }
            }
            else
            {
                isValid = false;
            }

            // error printout to screen if the zip code is not valid.
            if (isValid == false)
            {
                ZipError.Text = "Please insert a valid zip code.";
            }

            else
            {
                ZipError.Text = string.Empty;       // clear the ziperror

                // below is the process of construction our Uri, creating a channel WebClient.
                
                Uri baseUri = new Uri("http://webstrar61.fulton.asu.edu/page0/api/CombinedServices1/");

                UriTemplate myTemplate = new UriTemplate("GetZipCodeDetails?zipCode={zipCode}");

                Uri completeUri = myTemplate.BindByPosition(baseUri, zipCode);

                WebClient channel = new WebClient();

                string responseString = channel.DownloadString(completeUri);    // download the "data" directly as a string

                var responses = (JArray)JsonConvert.DeserializeObject(responseString);  // deserialize the string we download and place in JArray

                JArray jobj = JArray.Parse(responses.ToString());   // parse our array for just the data we need.

                // store our data in a JToken to convert to string.
                JToken jToken1 = jobj[0];
                JToken jToken2 = jobj[1];


                Lat_Output.Text = jToken1.ToString();
                Long_Output.Text = jToken2.ToString();
            }
        }


        /*
         * Earthquake Index button is what will activate/call our NaturalHazard Service
         * and return the earthquake index based on the latitude and longitude coordinates
         * provided by zip code conversion.
         */
        protected void Earthquake_Index_Btn_Click(object sender, EventArgs e)
        {
            Boolean isValid = true;     // handles error checking
            decimal latitude = 0;       // initialize latitude
            decimal longitude = 0;      // initialize longitude


            /*
             * nested if-if/else statement to check first if the textboxes for latitude or longitude are empty,
             * if they are not empty, we progress to the next check to make sure our input is actually a valid
             * latitude and longitude coordinate. -90 <= latitude < 90  && -180 <= longitude < 180. If these 
             * conditions are met, isValid = true. Otherewise, isValid = false. PRECAUTIONARY . . . OUR ZIP CODE
             * CONVERSION SHOULD PREVENT THIS FROM HAPPENING.
             */
            if (Lat_Output.Text != string.Empty && Long_Output.Text != string.Empty)
            {
                decimal lat = Convert.ToDecimal(Lat_Output.Text);      // store value from our latitude textbox
                decimal longi = Convert.ToDecimal(Long_Output.Text);   // store value from our longitude textbox

                if (lat >= -90 && lat < 90 && longi >= -180 && longi < 180)
                {
                    isValid = true;
                    latitude = lat;
                    longitude = longi;
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }

            // if isValid is false, display generic ERROR message.
            if (isValid == false)
            {

                ErrorLabel.Text = "ERROR: You must type in a zipcode to convert first!";

            }
            // else, carry on with calling service
            else
            {

                ErrorLabel.Text = string.Empty;     // erase our error label

                // below is the process of construction our Uri, creating a channel WebClient. 

                Uri baseUri = new Uri("http://webstrar61.fulton.asu.edu/page0/api/CombinedServices1/GetEarthQuakeHazard");

                UriTemplate myTemplate = new UriTemplate("?start={startDate}&latitude={latitude}&longitude={longitude}&radius={radius}&magnitude={magnitude}");

                Uri completeUri = myTemplate.BindByPosition(baseUri, startDate.ToString(), latitude.ToString(), longitude.ToString(), radius.ToString(), magnitude.ToString());

                WebClient channel = new WebClient();    // create a channel

                //           byte[] abc = channel.DownloadData(completeUri);

                string responseString = channel.DownloadString(completeUri);    //downloads the sources directly as a string

                //           Stream strm = new MemoryStream(abc);

                //            DataContractSerializer obj = new DataContractSerializer(typeof(string));

                //            string label = obj.ReadObject(strm).ToString();
                Index_Label.Text = responseString;                               // display our result in the GUI indexLabel


            }
        }


        /*
         *  The Search Button will listen for the button click and perform the 
         *  execution of the NewsFocus Service to return top 10 URLS based on the 
         *  the users query. The ListBox isVisible=false, until the button is clicked.
         *  Along with the instructions and buttons for our top10words service.
         *  
         */
        protected void Search_Btn_Click(object sender, EventArgs e)
        {

            if(URL_Listbox.Items.Count != 0)
            {
                URL_Listbox.Items.Clear();
            }

            if (NewsFocus_TextBox.Text != string.Empty)
            {
                if (ErrorLabel.Text != string.Empty)
                {
                    ErrorLabel.Text = string.Empty;
                }

                else
                {
                    // below is the process of construction our Uri, creating a channel WebClient.

                    Uri baseUri = new Uri("http://webstrar61.fulton.asu.edu/page0/api/CombinedServices1/");    // establish our baseUri

                    // UriTemplate to help to bind/construct our completeUri
                    UriTemplate myTemplate = new UriTemplate("GetNewsFocus?topics={topics}");

                    Uri completeUri = myTemplate.BindByPosition(baseUri, NewsFocus_TextBox.Text.ToString());

                    WebClient channel = new WebClient();    // create a channel

                    string responseString = channel.DownloadString(completeUri);    // download the "data" directly as a string

                    var responses = (JArray)JsonConvert.DeserializeObject(responseString);

                    JArray jobj = JArray.Parse(responses.ToString());

                    JToken[] urlTokens = new JToken[jobj.Count];


                    // Foreach to get all 10 children tokens (URLS) and convert them over to an array
                    JToken urlToken;
                    int j = 0;
                    foreach (dynamic prop in jobj.Children())
                    {
                        urlToken = prop.Value;
                        urlTokens[j] = urlToken;
                        j++;
                    }


                    // for loop to add url and top10words, respectively, to our URL_Listbox
                    for (int k = 0; k < 10; k++)
                    {
                        URL_Listbox.Items.Add(urlTokens[k].ToString());
                    }

                    // make URL results visible along with the instructions and button to
                    // our Top10Words service.
                    URL_Listbox.Visible = true;
                    Top10_Btn.Visible = true;
                    TopTenLabel.Visible = true;
                }

            }

            else
            {
                ErrorLabel.Text = "Please insert a topic to search";
            }
        }


        /*
         * Button handler to call our Top10Word Service based on URL selected
         */
        protected void Top10_Btn_Click(object sender, EventArgs e)
        {

            // creating a service client with our Top10WordsReference in order to use our service
            Top10WordsReference.Service1Client client = new Top10WordsReference.Service1Client();

            
            // error handling, if our global varialbe listIndex == 10, we know a 
            // url was never selected.
            if (listIndex == 10)
            {
                TopTenOutputLabel.Text = "You have not selected a URL to evaluate, please choose a URL.";
            }

            // else carry out the Top10Word Service
            else
            {
                Top10_URL_Label.Text = urlSelection;    // show which URL we evaluated
                TopTenOutputLabel.Text = string.Join(", ", client.Top10Words(urlSelection));    // separate each string with a comma

                // set the visibility to true for all labels and expected output upon button click
                Top10_URL_Label.Visible = true;
                Top10WordsFor_Label.Visible = true;
                Colon_Label.Visible = true;
                TopTenOutputLabel.Visible = true;

            }
        }

        /*
         * The URL_Listbox_SelectedIndexChanged listens for when the user selects an index
         * and assigns the correct URL to our global variable "urlSelection" in order to use
         * for our Top10Words Service.
         */
        protected void URL_Listbox_SelectedIndexChanged(object sender, EventArgs e)
        {

            // for loop was needed to cycle throw the different index options and to 
            // see if one is/was selected by our user.
            for(int i = 0; i < URL_Listbox.Items.Count; i++)
            {
                if (URL_Listbox.Items[i].Selected)
                {
                    listIndex = i;
                }
            }

            // switch to copy url string selected in order user for our Top10Word Service
            switch (listIndex)
            {

                // 0 is the first url 9 will be the 10th
                case 0:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

                
                case 1:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

                
                case 2:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

                
                case 3:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

               
                case 4:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

                
                case 5:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

                
                case 6:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

                
                case 7:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;


                case 8:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;


                case 9:
                    urlSelection = URL_Listbox.Items[listIndex].Text;
                    break;

                default:
                    break;
            }


        }
    }
}