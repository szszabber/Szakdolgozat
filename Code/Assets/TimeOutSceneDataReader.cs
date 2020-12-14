using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class TimeOutSceneDataReader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    public Text textOne;

    public Text textTwo;

    private void Start()
    {
        string data = xmlRawFile.text;

        ReadGameStoryTexts(data);
    }

    private void ReadGameStoryTexts(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> timeOutScene = xmlDoc.Root.Element("TimeOutSceneData").Elements("TimeOutScene").ToList();

        foreach (XElement timeOutXelement in timeOutScene)
        {
            string title = timeOutXelement.Element("Title").Value;
            string description = timeOutXelement.Element("Description").Value;
            textOne.text = title;
            textTwo.text = description;
        }
    }
}
