using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

using System.Drawing;
using System.Xml.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ClueReader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    void Start()
    {
        string data = xmlRawFile.text;
        ReadClues(data);
    }

    bool FedesbenVan(GameObject gameObjectA, GameObject gameObjectB)
    {
        if(gameObjectA.tag != "ClueButton")
        {
            return false;
        }

        BoxCollider2D colliderA = gameObjectA.GetComponent<BoxCollider2D>();
        BoxCollider2D colliderB = gameObjectB.GetComponent<BoxCollider2D>();

        float aMinX = gameObjectA.transform.position.x + colliderA.bounds.min.x;
        float aMaxX = gameObjectA.transform.position.x + colliderA.bounds.max.x;
        float aMinY = gameObjectA.transform.position.y + colliderA.bounds.min.y;
        float aMaxY = gameObjectA.transform.position.y + colliderA.bounds.max.y;

        float bMinX = gameObjectB.transform.position.x + colliderB.bounds.min.x;
        float bMaxX = gameObjectB.transform.position.x + colliderB.bounds.max.x;
        float bMinY = gameObjectB.transform.position.y + colliderB.bounds.min.y;
        float bMaxY = gameObjectB.transform.position.y + colliderB.bounds.max.y;

        // Ha B bal felső sarka benne van A-ban
        if (bMinX >= aMinX && bMinX <= aMaxX && bMinY >= aMinY && bMinY <= aMaxY)
        {
            return true;
        }

        // Ha A bal felső sarka benne van B-ban
        if (aMinX >= bMinX && aMinX <= bMaxX && aMinY >= bMinY && aMinY <= bMaxY)
        {
            return true;
        }

        if (bMinY <= aMaxY && bMinY >= aMinY && bMinX >= aMinX && bMinX <= aMaxX)
        {
            return true;
        }

        if (aMinY <= bMaxY && aMinY <= bMinY && aMinX >= bMinX && aMinX <= bMaxX)
        {
            return true;
        }

        return false;
    }

    public GameObject GenerateNewClueButton(GameObject prefabClueButton)
    {        
        int minX = -400;
        int maxX = 400;
        int minY = 300;
        int maxY = -300;
        Vector3 spawnPosition = new Vector3();

        GameObject newButton = Instantiate(prefabClueButton, spawnPosition, Quaternion.identity) as GameObject;
        newButton.transform.SetParent(null);
        RectTransform prefabRectTransform = (prefabClueButton.transform as RectTransform);
        Rect prefabRect = prefabRectTransform.rect;
        Transform clueCanvasTransform = GameObject.FindGameObjectWithTag("cluecanv").transform;
        bool vanAtfedes = true;
        // addig generálunk egy újabb pozíciót, amíg az jó helyre nem kerül
        while (vanAtfedes)
        {
            float xPos = Random.Range(minX, maxX);
            float yPos = Random.Range(minY, maxY);
            newButton.transform.position = new Vector3(xPos, yPos, 0f);

            int i = 0;
            while (i < clueCanvasTransform.childCount && ! FedesbenVan(clueCanvasTransform.GetChild(i).gameObject, newButton))
            {                
                i++;
            }

            vanAtfedes = i < clueCanvasTransform.childCount;
        }

        return newButton;
    }

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        List<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue").ToList();

        GameObject prefabButton = GameObject.Find("ClueButton");

        for (int i = 0; i < clues.Count; i++)
        {
            XElement clue = clues[i];

            GameObject newButton = GenerateNewClueButton(prefabButton);
            newButton.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
            Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
            buttonText.text = clue.Element("ClueTitle").Value;
        }
    }
}