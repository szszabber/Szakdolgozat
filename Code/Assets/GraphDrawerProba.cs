using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UIElements;
using CodeMonkey.Utils;

public class GraphDrawerProba : MonoBehaviour
{
    public TextAsset xmlRawFile;
    //public GameObject button;
    //private RectTransform graphContainer;

    void Start()
    {
        string data = xmlRawFile.text;
        ReadConclusions(data);
    }

    public void ReadConclusions(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        List<XElement> conclusions = xmlDoc.Root.Element("Conclusions").Elements("Conclusion").ToList();

        int minX = -200;
        int maxX = 250;
        int minY = 200;
        int maxY = -80;

        for (int i = 0; i < conclusions.Count; i++)
        {
            XElement conclusion = conclusions[i];

            GameObject prefabButton = GameObject.Find("prefabButton");

            float xPos = Random.Range(minX, maxX);
            float yPos = Random.Range(minY, maxY);
            Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
            //GameObject newButton = Instantiate(prefabButton, new Vector3(i, (i * 20)-100, 0), Quaternion.identity) as GameObject;
            GameObject newButton = Instantiate(prefabButton, spawnPosition, Quaternion.identity) as GameObject;

            newButton.transform.SetParent(GameObject.FindGameObjectWithTag("concCanv").transform, false);
            Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
            buttonText.text = conclusion.Element("ConclusionDescription").Value;

        }
    }
}

