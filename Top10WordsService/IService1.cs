/*
 * Description: Interface for Top10Words Service
 * 
 * Project 3 (Assignments 5 & 6)
 * Team 61
 * CSE 445/598 Distributed Software Development
 * Session C Fall 2020
 * Dr. Yinong Chen
 * 
 * Author:Bradley McGarvin
 * 
 * References: 7th edition Service-Oriented Computing and System Integration, 
 * Lecture 10 Slided 30 - 45.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Top10WordsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);


        // TODO: Add your service operations here
        [OperationContract]
        string[] Top10Words(string url);
    }


  
}
