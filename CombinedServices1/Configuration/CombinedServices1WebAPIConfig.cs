/*
 * Description: This is our configuration class used to register and configure our Web API.
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
 * WebStrar tutorial, Lecture 10 slide 39
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CombinedServices1.Configuration
{
    public class CombinedServices1WebAPIConfig
    {

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "defaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

        }

    }
}