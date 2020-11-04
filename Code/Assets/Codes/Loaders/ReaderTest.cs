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

    private List<GameObject> clueButtons = new List<GameObject>();

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

    public static Vector3[] AddPaddingToRect(Vector3[] corners)
    {
        corners[0].x -= 5;
        corners[0].y -= 5;
        corners[1].x -= 5;
        corners[1].y += 5;
        corners[2].x += 5;
        corners[2].y += 5;
        corners[3].x += 5;
        corners[3].y -= 5;

        return corners;
    }

    public static Rect CalculateRect(RectTransform rectTransform)
    {
        //először a recttransform 4 sarkát lekérdezzük worldspace-ben
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        corners = AddPaddingToRect(corners);

        //kiszámítjuk a szélességét és magasságát
        Vector2 size = new Vector2(corners[3].x - corners[0].x, corners[1].y - corners[0].y);

        //létrehozunk egy Rect-et az adatokból. Corners[0] a bal alsó, Corners[1] a bal felső, Corners[2] a jobb felső, Corners[3] a jobb alsó sarok
        return new Rect(corners[1], size);
    }

    public GameObject GenerateNewClueButton(GameObject prefabClueButton)
    {
        Vector3 spawnPosition = new Vector3();

        GameObject newButton = Instantiate(prefabClueButton, spawnPosition+(spawnPosition), Quaternion.identity) as GameObject;
        newButton.transform.SetParent(null);
        

        RectTransform prefabRectTransform = (prefabClueButton.transform as RectTransform);
        //Rect prefabRect = prefabRectTransform.rect;

        Transform clueCanvasTransform = GameObject.FindGameObjectWithTag("cluecanv").transform;
        bool vanAtfedes = true;

        int minX = -220;
        int maxX = 250;
        int minY = -100;
        int maxY = 100;
        // addig generálunk egy újabb pozíciót, amíg az jó helyre nem kerül
        int tryCount = 0;
        while (vanAtfedes && tryCount < 100000)
        {
            float xPos = Random.Range(minX, maxX);
            float yPos = Random.Range(minY, maxY);
            newButton.transform.position = new Vector3(xPos, yPos, 0f);

            int i = 0;
            //while (i < clueCanvasTransform.childCount && !FedesbenVan(clueCanvasTransform.GetChild(i).gameObject, newButton))
            while (i < clueButtons.Count && !FedesbenVan(clueButtons[i], newButton))
            {
                i++;
            }

            vanAtfedes = i < clueButtons.Count && clueButtons.Count != 0;

            ++tryCount;
        }

        if(tryCount == 100000)
        {
            return null;
        }

        return newButton;
    }

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        List<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue").ToList();

        GameObject prefabButton = GameObject.Find("CluePrefabButton");
        //GameObject prefabButton = GameObject.Find("ClueButton");


        for (int i = 0; i < clues.Count; i++)
        {
            XElement clue = clues[i];

            GameObject newButton = GenerateNewClueButton(prefabButton);

            if(newButton == null)
            {
                clueButtons.Clear();
                ReadClues(xmlFileAsText);
            }

            // newButton.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
            clueButtons.Add(newButton);

            Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
            buttonText.text = clue.Element("ClueTitle").Value;
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().Render();
        }

        foreach (var button in clueButtons)
        {
            button.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
        }
    }
}