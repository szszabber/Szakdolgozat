using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GraphDrawer : MonoBehaviour
{
    [SerializeField]
    private Sprite nagyitoSprite;
    private RectTransform graphContainer;

    private List<GameObject> conclusionButtons = new List<GameObject>();
    private List<GameObject> motivationButtons = new List<GameObject>();
    private List<GameObject> finalDeductionnButtons = new List<GameObject>();

    private List<GameObject> connections = new List<GameObject>();

    private Dictionary<GameObject, Relation> buttonsToRelations = new Dictionary<GameObject, Relation>();

    public void Awake()
    {
        GameObject grapContainerGO = GameObject.Find("GraphContainer");
        if(grapContainerGO == null)
        {
            return;
        }

        graphContainer = grapContainerGO.GetComponent<RectTransform>();

        connections.Clear();
        buttonsToRelations.Clear();
        conclusionButtons.Clear();
        motivationButtons.Clear();
        finalDeductionnButtons.Clear();

        ShowGraph();
    }

    //private GameObject CreateCircle(Vector2 anchoredPosition, string title)
    //{
    //    GameObject gameObject = new GameObject("Nagyito", typeof(Image));
    //    gameObject.transform.SetParent(graphContainer, false);
    //    gameObject.GetComponent<Image>().sprite = nagyitoSprite;
    //    RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
    //    rectTransform.anchoredPosition = anchoredPosition;
    //    rectTransform.sizeDelta = new Vector2(40, 40);
    //    rectTransform.anchorMin = new Vector2(0, 0);
    //    rectTransform.anchorMax = new Vector2(0, 0);
    //    return gameObject;
    //}

    //private void ShowGraph()
    //{
    //    float graphHeight = graphContainer.sizeDelta.y;
    //    float yMaximum = 100f;
    //    float xSize = 50f;

    //    GameObject lastCircleGameObject = null;
    //    for (int i = 0; i < valueList.Count; i++)
    //    {
    //        float xPosition = xSize + i * xSize;
    //        float yPosition = (valueList[i] / yMaximum) * graphHeight;
    //        GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
    //        if (lastCircleGameObject != null)
    //        {
    //            CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
    //        }
    //        lastCircleGameObject = circleGameObject;
    //    }
    //}

    private void ShowGraph()
    {
        DrawConclusions();
        DrawMotivations();
        DrawFinalDeductions();

        DrawConnectionsBetweenConclusionsAndMotivations();
        //DrawConnectionsBetweenMotivationsAndFinalDeductions();
    }

    private void DrawConnectionsBetweenMotivationsAndFinalDeductions()
    {
        throw new NotImplementedException();
    }

    private void DrawConnectionsBetweenConclusionsAndMotivations()
    {
        foreach (var motivationButton in motivationButtons)
        {
            Relation relation = buttonsToRelations[motivationButton];
            List<GameObject> relatedConclusionButtons = conclusionButtons
                .FindAll(concButton => buttonsToRelations[concButton].Output == relation.Input1 || buttonsToRelations[concButton].Output == relation.Input2)
                .ToList();

            foreach (var conclusionButton in relatedConclusionButtons)
            {
                CreateDotConnection(conclusionButton.GetComponent<RectTransform>().anchoredPosition, motivationButton.GetComponent<RectTransform>().anchoredPosition);
            }
            
        }
    }

    private void DrawConclusions()
    {
        //float graphHeight = graphContainer.sizeDelta.y;
        float ySize = 20f;

        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenClueRelations.Count; i++)
        {

            float xPosition = -230f;
            float yPosition = ySize + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            string title = Data.ChoosenClueRelations[i].Output.Title;
            //Vector2 anchoredPosition = new Vector2(xPosition, yPosition);
            //GameObject circleGameObject = CreateCircle(anchoredPosition, title);    

            GameObject newConcButton = CreateNode(prefabConclusionButton, spawnPos);

            if (newConcButton == null)
            {
                conclusionButtons.Clear();
                DrawConclusions();
            }
            conclusionButtons.Add(newConcButton);
            Text buttonText = (Text)newConcButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            buttonsToRelations.Add(newConcButton, Data.ChoosenClueRelations[i]);
            //buttonsToRelations[newConcButton] = Data.ChoosenClueRelations[i];
        }
    }

    private void DrawMotivations()
    {
        float ySize = 20f;
        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenConclusionRelations.Count; i++)
        {
            float xPosition = 0f;
            float yPosition = ySize + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            string title = Data.ChoosenConclusionRelations[i].Output.Title;

            GameObject newMotivationButton = CreateNode(prefabConclusionButton, spawnPos);
            if (newMotivationButton == null)
            {
                motivationButtons.Clear();
                DrawMotivations();
            }
            motivationButtons.Add(newMotivationButton);
            Text buttonText = (Text)newMotivationButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            buttonsToRelations.Add(newMotivationButton, Data.ChoosenConclusionRelations[i]);
        }
    }

    private void DrawFinalDeductions()
    {
        float ySize = 20f;
        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenMotivationRelations.Count; i++)
        {
            float xPosition = 230f;
            float yPosition = ySize + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            string title = Data.ChoosenMotivationRelations[i].Output.Title;

            GameObject newFinalDeductionButton = CreateNode(prefabConclusionButton, spawnPos);
            if (newFinalDeductionButton == null)
            {
                finalDeductionnButtons.Clear();
                DrawMotivations();
            }
            finalDeductionnButtons.Add(newFinalDeductionButton);
            Text buttonText = (Text)newFinalDeductionButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            buttonsToRelations.Add(newFinalDeductionButton, Data.ChoosenMotivationRelations[i]);
        }
    }

    private GameObject CreateNode(GameObject prefabConclusionButton, Vector3 spawnPos)
    {
        GameObject conclusionButton = Instantiate(prefabConclusionButton, spawnPos, Quaternion.identity);
        conclusionButton.transform.SetParent(GameObject.FindGameObjectWithTag("concCanv").transform, false);

        RectTransform rectTransform = conclusionButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = conclusionButton.GetComponent<RectTransform>().anchoredPosition;
        //rectTransform.sizeDelta = new Vector2(40, 40);
        //rectTransform.anchorMin = new Vector2(0, 0);
        //rectTransform.anchorMax = new Vector2(0, 0);

        return conclusionButton;
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

        connections.Add(gameObject);
    }
}
