using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

public class ReaderProba : MonoBehaviour
{
    public TextAsset xmlRawFile;
    //public Button originalButton;
    public Text buttonText;
    public Text conclusionText;
    public Text motivationText;
    //public Text clueDescText;


    void Start()
    {
        string data = xmlRawFile.text;
        ReadCluesDesc(data);
        ReadConclusions(data);
        ReadMotivation(data);
    }

    public void ReadCluesDesc(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);
        List<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue").ToList();

        foreach (XElement clue in clues)
        {    
            
            buttonText.text += clue.Element("ClueDescription").Value+"\n";
        }
    }

    public void ReadConclusions(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);
        List<XElement> conclusions = xmlDoc.Root.Element("Conclusions").Elements("Conclusion").ToList();

        foreach (XElement conclusion in conclusions)
        {

            conclusionText.text +="Konklúzió: " + conclusion.Element("ConclusionDescription").Value + "\n";
        }
    }

    public void ReadMotivation(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);
        List<XElement> motivations = xmlDoc.Root.Element("Motivations").Elements("Motivation").ToList();

        foreach (XElement motivation in motivations)
        {

            motivationText.text += "Motiváció: " + motivation.Element("MotivationDescription").Value + "\n";
        }
    }
}

