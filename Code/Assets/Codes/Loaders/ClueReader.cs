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

        Rect rectA = (gameObjectA.transform as RectTransform).rect;
        Rect rectB = (gameObjectB.transform as RectTransform).rect;

        return rectA.Overlaps(rectB, true);

        //BoxCollider2D colliderA = gameObjectA.GetComponent<BoxCollider2D>();
        //BoxCollider2D colliderB = gameObjectB.GetComponent<BoxCollider2D>();
        //return colliderB.bounds.Intersects(colliderA.bounds);

        //Renderer rendererA = gameObjectA.GetComponent<Renderer>();
        //Renderer rendererB = gameObjectB.GetComponent<Renderer>();
        //var boundA = rendererA.bounds;
        //var boundB = rendererB.bounds;
        //return boundA.Intersects(boundB);
        //if (boundA.Intersects(boundB))
        //{
        //    return true;
        //}
        //else
        //{
        //    return true;
        //}




        //RectTransform rectTransformA = gameObjectA.GetComponent<RectTransform>();
        //RectTransform rectTransformB = gameObjectB.GetComponent<RectTransform>();

        //Vector3[] cornersA = new Vector3[4];
        //Vector3[] cornersB = new Vector3[4];
        //rectTransformA.GetWorldCorners(cornersA);
        //rectTransformB.GetWorldCorners(cornersB);

        //cornersA[0].

        //return true;




        //float width = (gameObjectA.transform as RectTransform).rect.width;
        //float height = (gameObjectA.transform as RectTransform).rect.height;

        //float aMinX = gameObjectA.transform.position.x - width / 2;
        //float aMaxX = gameObjectA.transform.position.x + width / 2;
        //float aMinY = gameObjectA.transform.position.y - height / 2;
        //float aMaxY = gameObjectA.transform.position.y + height / 2;

        //float bMinX = gameObjectB.transform.position.x - width / 2;
        //float bMaxX = gameObjectB.transform.position.x + width / 2;
        //float bMinY = gameObjectB.transform.position.y - height / 2;
        //float bMaxY = gameObjectB.transform.position.y + height / 2;

        //// B bal alsó sarka benne van e A-ban
        //if (bMinX >= aMinX && bMinX <= aMaxX && bMinY >= aMinY && bMinY <= aMaxY)
        //{
        //    return true;
        //}

        ////B jobb alsó sarka benne van A ban
        //if (bMinY <= aMaxY && bMinY >= aMinY && bMaxX <= aMaxX && bMaxX <= aMinX)
        //{
        //    return true;
        //}

        //// Ha B bal felső sarka benne van A-ban
        //if (bMaxY <= aMaxY && bMaxY >= aMinY && bMinX >= aMinX && bMinX <= aMaxX)
        //{
        //    return true;
        //}

        //// Ha B jobb felső sarka benne van A-ban
        //if (bMaxY >= aMinY && bMaxY <= aMaxY && bMaxX >= aMinX && bMaxX <= aMaxX)
        //{
        //    return true;
        //}

        //if (FedesbenVanForditva(aMinX, aMaxX, aMinY, aMaxY, bMinX, bMaxX, bMinY, bMaxY))
        //{
        //    return true;
        //}
        //else
        //{
        //    Debug.Log("aMinX: " + aMinX + "\n" +
        //              "aMaxX: " + aMaxX + "\n" +
        //              "aMinY: " + aMinY + "\n" +
        //              "aMaxY: " + aMaxY + "\n" +
        //              "bMinX: " + bMinX + "\n" +
        //              "bMaxX: " + bMaxX + "\n" +
        //              "bMinY: " + bMinY + "\n" +
        //              "bMaxY: " + bMaxY);

        //    return false;
        //}


        //float aMinX = gameObjectA.transform.position.x + colliderA.bounds.min.x;
        //float aMaxX = gameObjectA.transform.position.x + colliderA.bounds.max.x;
        //float aMinY = gameObjectA.transform.position.y + colliderA.bounds.min.y;
        //float aMaxY = gameObjectA.transform.position.y + colliderA.bounds.max.y;

        //float bMinX = gameObjectB.transform.position.x + colliderB.bounds.min.x;
        //float bMaxX = gameObjectB.transform.position.x + colliderB.bounds.max.x;
        //float bMinY = gameObjectB.transform.position.y + colliderB.bounds.min.y;
        //float bMaxY = gameObjectB.transform.position.y + colliderB.bounds.max.y;

        //// Ha B bal felső sarka benne van A-ban
        //if (bMinX >= aMinX && bMinX <= aMaxX && bMinY >= aMinY && bMinY <= aMaxY)
        //{
        //    return true;
        //}

        //// Ha A bal felső sarka benne van B-ban
        //if (aMinX >= bMinX && aMinX <= bMaxX && aMinY >= bMinY && aMinY <= bMaxY)
        //{
        //    return true;
        //}

        //if (bMinY <= aMaxY && bMinY >= aMinY && bMinX >= aMinX && bMinX <= aMaxX)
        //{
        //    return true;
        //}

        //if (aMinY <= bMaxY && aMinY <= bMinY && aMinX >= bMinX && aMinX <= bMaxX)
        //{
        //    return true;
        //}

        //return false;
    }

    //public bool FedesbenVanForditva(float bMinX, float bMaxX, float bMinY, float bMaxY, float aMinX, float aMaxX, float aMinY, float aMaxY)
    //{
    //    // B bal alsó sarka benne van e A-ban
    //    if (bMinX >= aMinX && bMinX <= aMaxX && bMinY >= aMinY && bMinY <= aMaxY)
    //    {
    //        return true;
    //    }

    //    //B jobb alsó sarka benne van A ban
    //    if (bMinY <= aMaxY && bMinY >= aMinY && bMaxX <= aMaxX && bMaxX <= aMinX)
    //    {
    //        return true;
    //    }

    //    // Ha B bal felső sarka benne van A-ban
    //    if (bMaxY <= aMaxY && bMaxY >= aMinY && bMinX >= aMinX && bMinX <= aMaxX)
    //    {
    //        return true;
    //    }

    //    // Ha B jobb felső sarka benne van A-ban
    //    if (bMaxY >= aMinY && bMaxY <= aMaxY && bMaxX >= aMinX && bMaxX <= aMaxX)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    public GameObject GenerateNewClueButton(GameObject prefabClueButton)
    {        
        int minX = -150;
        int maxX = 130;
        int minY = -130;
        int maxY = 130;
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

        GameObject prefabButton = GameObject.Find("ClueButtonPrefab");

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