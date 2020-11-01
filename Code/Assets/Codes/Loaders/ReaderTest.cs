using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

using System.Drawing;
using System.Xml.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ReaderTest : MonoBehaviour
{
    public TextAsset xmlRawFile;

    void Start()
    {
        string data = xmlRawFile.text;
        ReadClues(data);
    }

    public static bool FedesbenVan(GameObject gameObjectA, GameObject gameObjectB)
    {
        if (gameObjectA.tag != "ClueButton")
        {
            return false;
        }

        RectTransform rectTransformA = gameObjectA.transform as RectTransform;
        RectTransform rectTransformB = gameObjectB.transform as RectTransform;

        Rect rectA = CalculateRect(rectTransformA);
        Rect rectB = CalculateRect(rectTransformB);

        return rectA.Overlaps(rectB, true);
    }

    public static Rect CalculateRect(RectTransform rectTransform)
    {
        //először a recttransform 4 sarkát lekérdezzük worldspace-ben
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        //kiszámítjuk a szélességét és magasságát
        Vector2 size = new Vector2(corners[3].x - corners[0].x, corners[1].y - corners[0].y);

        //létrehozunk egy Rect-et az adatokból. Corners[0] a bal alsó, Corners[1] a bal felső, Corners[2] a jobb felső, Corners[3] a jobb alsó sarok
        return new Rect(corners[1], size);
    }

    public GameObject GenerateNewClueButton(GameObject prefabClueButton)
    {
        int minX = -150;
        int maxX = 130;
        int minY = -200;
        int maxY = 200;
        Vector3 spawnPosition = new Vector3();

        GameObject newButton = Instantiate(prefabClueButton, spawnPosition, Quaternion.identity) as GameObject;
        newButton.transform.SetParent(null);

        RectTransform prefabRectTransform = (prefabClueButton.transform as RectTransform);
        //Rect prefabRect = prefabRectTransform.rect;

        Transform clueCanvasTransform = GameObject.FindGameObjectWithTag("cluecanv").transform;
        bool vanAtfedes = true;

        // addig generálunk egy újabb pozíciót, amíg az jó helyre nem kerül
        while (vanAtfedes)
        {
            float xPos = Random.Range(minX, maxX);
            float yPos = Random.Range(minY, maxY);
            newButton.transform.position = new Vector3(xPos, yPos, 0f);

            int i = 0;
            while (i < clueCanvasTransform.childCount && !FedesbenVan(clueCanvasTransform.GetChild(i).gameObject, newButton))
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