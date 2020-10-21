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

    //public GameObject button;
    //private RectTransform graphContainer;

    void Start()
    {
        string data = xmlRawFile.text;
        ReadClues(data);
    }

    //bool FedesbenVan(Vector3 vewctorA, Vector3 vectorB, Rect rectPrefab)
    //{
    //    Point positionA = new Point((int)vewctorA.x, (int)vewctorA.y);
    //    Point positionB = new Position();
    //    Rectangle rectA = new Rectangle(positionA, new Size(rectPrefab.Width, rectPrefab.Height)
    //    Rectangle rectB = new Rectangle(positionB, new Size(rectPrefab.Width, rectPrefab.Height)

    //    Rectangle.Intersect
    //}

    bool FedesbenVan(GameObject gameObjectA, GameObject gameObjectB)
    {
        BoxCollider2D colliderA = gameObjectA.GetComponent<BoxCollider2D>();
        BoxCollider2D colliderB = gameObjectB.GetComponent<BoxCollider2D>();

        float aMinX = colliderA.bounds.min.x;
        float aMaxX = colliderA.bounds.max.x;
        float aMinY = colliderA.bounds.min.y;
        float aMaxY = colliderA.bounds.max.y;

        float bMinX = colliderB.bounds.min.x;
        float bMaxX = colliderB.bounds.max.x;
        float bMinY = colliderB.bounds.min.y;
        float bMaxY = colliderB.bounds.max.y;

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
        int minX = -200;
        int maxX = 250;
        int minY = 200;
        int maxY = -80;
        Vector3 spawnPosition = new Vector3();

        GameObject newButton = Instantiate(prefabClueButton, spawnPosition, Quaternion.identity) as GameObject;
        RectTransform prefabRectTransform = (prefabClueButton.transform as RectTransform);
        Rect prefabRect = prefabRectTransform.rect;
        Transform clueCanvasTransform = GameObject.FindGameObjectWithTag("cluecanv").transform;
        bool vanAtfedes = true;
        // addig generálunk egy újabb pozíciót, amíg az jó helyre nem kerül
        while (vanAtfedes)
        {
            float yPos = Random.Range(minY, maxY);
            float xPos = Random.Range(minX, maxX);
            newButton.transform.position = new Vector3(xPos, yPos, 0f);

            int i = 0;
            //while (i < clueCanvasTransform.childCount && ! FedesbenVan(clueCanvasTransform.GetChild(i).position, spawnPosition, prefabRect))
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