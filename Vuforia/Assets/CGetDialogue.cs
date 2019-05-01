using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

//Written by Joe and Josh

//Get dialogue from the "dialogue.xml" file based on an attritube, and return the value
[System.Serializable]
public class CGetDialogue
{
    //Provide the tag for the dialogue, and returns the value inside that tag
    public string GetDialogue(string tag)
    {
        string result = "error";    //If this wasn't overwritten, something went wrong

        //Load the XML file
        XmlDocument document = new XmlDocument();
        document.Load("assets/assets/dialogue.xml");

        //Put the last node into the Result variable
        foreach (XmlNode node in document.GetElementsByTagName(tag))
            result = node.InnerText;

        return result;
    }
}
