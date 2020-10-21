using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProbaDescReader : MonoBehaviour
{
    public TextAsset xmlRawFile;
    //public Button originalButton;
    //public Text buttonText;
    public Text clueDescText;

    // Start is called before the first frame update
    void Start()
    {
        string data = xmlRawFile.text;
        ReadClueDesc(data);
    }
    public void ReadClueDesc(string xmlFileAsText)
    {
        XDocument xmlDoc2 = XDocument.Parse(xmlFileAsText);
        List<XElement> cluesDesc = xmlDoc2.Root.Element("Clues").Elements("Clue").ToList();
        foreach (XElement clue in cluesDesc)
        {
            clueDescText.text += clue.Element("ClueDescription").Value + "\n";
        }
    }
}
