using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

public class ClueReader : MonoBehaviour
{
    public TextAsset xmlRawFile;
    public Text uiText;

    // Use this for initialization
    void Start()
    {
        string data = xmlRawFile.text;
        ReadClues(data);
        //ReadConclusions(data);
    }

    //public void ReadClues(string xmlData)
    //{
    //    string totVal = "";
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(new StringReader(xmlData));

    //    string xmlPathPattern = "//Clues/Clue";
        

    //    XmlNodeList myNodeList = xmlDoc.SelectNodes(xmlPathPattern);
    //    foreach (XmlNode node in myNodeList)
    //    {
    //        XmlNode ClueTitle = node.FirstChild;
    //        XmlNode desc = node.NextSibling;
    //        totVal += ClueTitle.InnerXml +" - " + desc.InnerXml+"\n";
    //        uiText.text = totVal;            
    //    } 
    //}

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        List<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue").ToList();

        foreach (XElement clue in clues)
        {
            uiText.text += "Cím: " + clue.Element("ClueTitle").Value + ", Leírás: " + clue.Element("ClueDescription").Value+"\n";
        }
    }

    //public void ReadConclusions(string xmlData)
    //{
    //    string totVal = "";
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(new StringReader(xmlData));

    //    string xmlPathPattern = "//Conclusions/Conclusion";


    //    XmlNodeList myNodeList = xmlDoc.SelectNodes(xmlPathPattern);
    //    foreach (XmlNode node in myNodeList)
    //    {
    //        XmlNode title = node.FirstChild;

    //        totVal += title.InnerXml + "\n";
    //        uiText.text = totVal;
    //    }
    //}

    
}