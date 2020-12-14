using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class WinSceneDataReader : MonoBehaviour
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

        IEnumerable<XElement> winScene = xmlDoc.Root.Element("WinSceneData").Elements("WinScene").ToList();

        foreach (XElement winXelement in winScene)
        {
            string title = winXelement.Element("Title").Value;
            string description = winXelement.Element("Description").Value;
            textOne.text = title;
            textTwo.text = description;
        }
    }
}
