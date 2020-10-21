using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using JetBrains.Annotations;

public class ClueReader2 : MonoBehaviour
{
    public TextAsset xmlRawFile;

    //public GameObject button;
    //private RectTransform graphContainer;

    void Start()
    {
        string data = xmlRawFile.text;
        ReadClues(data);
    }

    Vector3 location = new Vector3();
    int MaxSpawnTime = 12;
    Vector2 min = new Vector2();
    Vector2 max = new Vector2();


    //public GameObject GenerateNewClueButton(GameObject newClueButton)
    //{


    //    //Transform clueCanvasTransform = GameObject.FindGameObjectWithTag("cluecanv").transform;


    //    //newButton.transform.position = spawnPos;
    //    //for (int i = 0; i < transform.childCount; i++)
    //    //{
    //    //    clueCanvasTransform.GetChild(i);
    //    //}

    //    //while (newClueButton.transform.position == spawnPos)
    //    //{
    //    //    Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
    //    //}
        
    //}

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        List<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue").ToList();

        GameObject prefabButton = GameObject.Find("ClueButton");

        for (int i = 0; i < clues.Count; i++)
        {
            XElement clue = clues[i];
            
            int minX = -200;
            int maxX = 250;
            int minY = 200;
            int maxY = -80;

            location.y = Random.Range(minY, maxY);
            location.x = Random.Range(minX, maxX);
            location.z = -Camera.main.transform.position.z;
            Vector3 worldLocation = Camera.main.WorldToScreenPoint(location);
            //Vector3 spawnPos = new Vector3(xPos, yPos, 0f);

            int spawnTries = 1;
            while (Physics2D.OverlapArea(min, max) != null && spawnTries < MaxSpawnTime)
            {
                location.x = Random.Range(minX, maxX);
                location.y = Random.Range(minY, maxY);
                worldLocation = Camera.main.ScreenToWorldPoint(location);

                spawnTries++;
            }

            if (Physics2D.OverlapArea(min, max) == null)
            {
                GameObject newButton = Instantiate(prefabButton) as GameObject;
                newButton.transform.position = location;
                newButton.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
                Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
                buttonText.text = clue.Element("ClueTitle").Value;
            }

            //GameObject newButton = GenerateNewClueButton(prefabButton);
            
        }
    }
}