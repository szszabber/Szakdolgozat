using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FixPositionedClues : MonoBehaviour
{
    public TextAsset xmlRawFile;
    
    void Start()
    {
        string data = xmlRawFile.text;
        ReadClues(data);
    }

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        List<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue").ToList();

        GameObject prefabButton = GameObject.Find("ClueButton");

        for (int i = 0; i < clues.Count; i++)
        {
            XElement clue = clues[i];

            //GameObject newButton = GenerateNewClueButton(prefabButton);
            GameObject newButton = Instantiate(prefabButton, new Vector3(0, -110 + i * 22, 0), Quaternion.identity);

            newButton.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
            Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
            buttonText.text = clue.Element("ClueTitle").Value;
        }
    }
}
