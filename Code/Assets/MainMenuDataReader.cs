using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuDataReader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    public Image backgroundImage;

    public Text gameTitleText;

    public Text gameSubtitleText;

    public Text gameDescriptionText;

    public Text sourceText;

    public Text developerText;

    private void Start()
    {
        string data = xmlRawFile.text;

        ReadGameStoryTexts(data);
    }

    private void ReadGameStoryTexts(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> mainMenu = xmlDoc.Root.Element("MainMenuData").Elements("MainMenu").ToList();

        foreach (XElement mainMenuXelement in mainMenu)
        {
            string title = mainMenuXelement.Element("Title").Value;
            string subtitle = mainMenuXelement.Element("Subtitle").Value;
            string description = mainMenuXelement.Element("Description").Value;
            string source = mainMenuXelement.Element("Source").Value;
            string developer = mainMenuXelement.Element("Developer").Value;

            gameTitleText.text = title;
            gameSubtitleText.text = subtitle;
            gameDescriptionText.text = description;
            sourceText.text = source;
            developerText.text = developer;
        }
    }
}
