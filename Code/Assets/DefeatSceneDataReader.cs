using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class DefeatSceneDataReader : MonoBehaviour
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

        IEnumerable<XElement> defeatScene = xmlDoc.Root.Element("DefeatSceneData").Elements("DefeatScene").ToList();

        foreach (XElement defeatXelement in defeatScene)
        {
            string title = defeatXelement.Element("Title").Value;
            string description = defeatXelement.Element("Description").Value;
            string source = defeatXelement.Element("Source").Value;
            
            textOne.text = title;
            textTwo.text = description;
            sourceText.text = source;
        }
    }
}
