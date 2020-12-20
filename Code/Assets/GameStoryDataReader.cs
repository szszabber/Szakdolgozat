using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class GameStoryDataReader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    public Text textOne;

    public Text textTwo;

    public Text sourceText;

    private void Start()
    {
        string data = xmlRawFile.text;

        ReadGameStoryTexts(data);
    }

    private void ReadGameStoryTexts(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> gameScene = xmlDoc.Root.Element("GameStoryData").Elements("GameStory").ToList();

        foreach (XElement storyXelement in gameScene)
        {
            string title = storyXelement.Element("Title").Value;
            string description = storyXelement.Element("Description").Value;
            string source = storyXelement.Element("Source").Value;
            textOne.text = title;
            textTwo.text = description;
            sourceText.text = source;
        }
    }
}
