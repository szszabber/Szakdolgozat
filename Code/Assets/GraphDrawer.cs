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
    private List<GameObject> finalDeductionButtons = new List<GameObject>();
    private List<GameObject> conclusionsToFinalDeductionButtons = new List<GameObject>();

    private List<GameObject> connections = new List<GameObject>();

    private Dictionary<GameObject, Relation> buttonsToRelations = new Dictionary<GameObject, Relation>();

    public void Awake()
    {

        GameObject grapContainerGO = GameObject.Find("GraphContainer");
        if (grapContainerGO == null)
        {
            return;
        }

        graphContainer = grapContainerGO.GetComponent<RectTransform>();

        connections.Clear();
        buttonsToRelations.Clear();
        conclusionButtons.Clear();
        motivationButtons.Clear();
        finalDeductionButtons.Clear();

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
        DrawConclusionsToFinalDeductions();

        DrawConnectionsBetweenConclusionsAndMotivations();
        DrawConnectionsBetweenMotivationsAndFinalDeductions();
        DrawConnectionsBetweenConclusionsAndFinalDeductions();
    }

    private void DrawConnectionsBetweenConclusionsAndMotivations()
    {
        foreach (var motivationButton in motivationButtons)
        {
            Relation relation = buttonsToRelations[motivationButton];
            List<GameObject> relatedConclusionButtons = conclusionButtons
                .FindAll(concButton => buttonsToRelations[concButton].Output1 == relation.Input1 || buttonsToRelations[concButton].Output1 == relation.Input2)
                .ToList();

            foreach (var conclusionButton in relatedConclusionButtons)
            {
                CreateDotConnection(conclusionButton.GetComponent<RectTransform>().anchoredPosition, motivationButton.GetComponent<RectTransform>().anchoredPosition);
            }
        }
    }

    private void DrawConnectionsBetweenMotivationsAndFinalDeductions()
    {
        foreach (var finalDeductionButton in finalDeductionButtons)
        {
            Relation relation = buttonsToRelations[finalDeductionButton];
            List<GameObject> relatedMotivationButtons = motivationButtons
                .FindAll(motivationButton => buttonsToRelations[motivationButton].Output1 == relation.Input1 || buttonsToRelations[motivationButton].Output1 == relation.Input2)
                .ToList();

            foreach (var motivationButton in relatedMotivationButtons)
            {
                CreateDotConnection(motivationButton.GetComponent<RectTransform>().anchoredPosition, finalDeductionButton.GetComponent<RectTransform>().anchoredPosition);
            }
        }
    }

    private void DrawConnectionsBetweenConclusionsAndFinalDeductions()
    {
        foreach (var finalDeductionButton in finalDeductionButtons)
        {
            Relation relation = buttonsToRelations[finalDeductionButton];
            List<GameObject> relatedConclusionButtons = conclusionButtons
                .FindAll(concButton => buttonsToRelations[concButton].Output1 == relation.Input1 || buttonsToRelations[concButton].Output1 == relation.Input2)
                .ToList();

            foreach (var conclusionButton in relatedConclusionButtons)
            {
                CreateDotConnection(conclusionButton.GetComponent<RectTransform>().anchoredPosition, finalDeductionButton.GetComponent<RectTransform>().anchoredPosition);
            }
        }
    }

    private void DrawConclusions()
    {
        //float graphHeight = graphContainer.sizeDelta.y;
        float ySize = 50f;

        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenClueRelations.Count; i++)
        {

            float xPosition = -250f;
            float yPosition = (ySize - 200f) + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            GameObject newConcButton = CreateNode(prefabConclusionButton, spawnPos);
            //Vector2 anchoredPosition = new Vector2(xPosition, yPosition);
            //GameObject circleGameObject = CreateCircle(anchoredPosition, title);    

            string title;
            if (Data.ChoosenClueRelations[i].Output2 == null)
            {
                title = Data.ChoosenClueRelations[i].Output1.Title;
            }
            else
            {
                Button newConcButtonClick = newConcButton.GetComponent<Button>();
                newConcButtonClick.onClick.AddListener(HandleConcButtonClick);
                if (Data.ChoosenClueRelations[i].SelectedOutput == null)
                {
                    title = "Válassz konklúziót!";
                }
                else
                {
                    title = Data.ChoosenClueRelations[i].SelectedOutput.Title;
                }

            }


            if (newConcButton == null)
            {
                conclusionButtons.Clear();
                DrawConclusions();
            }
            conclusionButtons.Add(newConcButton);
            Text buttonText = (Text)newConcButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            //buttonsToRelations.Add(newConcButton, Data.ChoosenClueRelations[i]);
            buttonsToRelations[newConcButton] = Data.ChoosenClueRelations[i];
        }
    }

    private void HandleConcButtonClick()
    {
        GameObject graphCanvas = GameObject.Find("GraphCanvas");
        GameObject choosingCanvas = graphCanvas.transform.Find("ChoosingCanvas").gameObject;
        choosingCanvas.SetActive(true);

        GameObject outputButton1 = GameObject.Find("ChoosingPrefabButton1");
        GameObject outputButton2 = GameObject.Find("ChoosingPrefabButton2");

        Text panel1DescText = (Text)GameObject.Find("TextDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2DescText = (Text)GameObject.Find("TextDescPanel2").GetComponentInChildren(typeof(Text));

        for (int i = 0; i < Data.ChoosenClueRelations.Count; i++)
        {
            Text buttonText1 = (Text)outputButton1.GetComponentInChildren(typeof(Text));
            buttonText1.text = Data.ChoosenClueRelations[i].Output1.Title.ToString();
            panel1DescText.text = Data.ChoosenClueRelations[i].Output1.Desription.ToString();

            Text buttonText2 = (Text)outputButton2.GetComponentInChildren(typeof(Text));
            buttonText2.text = Data.ChoosenClueRelations[i].Output2.Title.ToString();
            panel2DescText.text = Data.ChoosenClueRelations[i].Output2.Desription.ToString();
        }
    }

    private void DrawMotivations()
    {
        float ySize = 50f;
        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenConclusionRelations.Count; i++)
        {
            float xPosition = 0f;
            float yPosition = ySize + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            string title = Data.ChoosenConclusionRelations[i].Output1.Title;
            GameObject newMotivationButton = CreateNode(prefabConclusionButton, spawnPos);

            if (newMotivationButton == null)
            {
                foreach (var relation in buttonsToRelations)
                {
                    if (relation.Value.Input1 is Conclusion && relation.Value.Output1 is Motivation)
                    {
                        buttonsToRelations.Remove(relation.Key);
                    }
                }
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
        float ySize = 50f;
        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenMotivationRelations.Count; i++)
        {
            float xPosition = 250f;
            float yPosition = ySize + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            string title = Data.ChoosenMotivationRelations[i].Output1.Title;

            GameObject newFinalDeductionButton = CreateNode(prefabConclusionButton, spawnPos);
            if (newFinalDeductionButton == null)
            {
                foreach (var relation in buttonsToRelations)
                {
                    if (relation.Value.Input1 is Motivation && relation.Value.Output1 is FinalDeduction)
                    {
                        buttonsToRelations.Remove(relation.Key);
                    }
                }
                finalDeductionButtons.Clear();
                DrawMotivations();
            }
            finalDeductionButtons.Add(newFinalDeductionButton);
            Text buttonText = (Text)newFinalDeductionButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            buttonsToRelations.Add(newFinalDeductionButton, Data.ChoosenMotivationRelations[i]);
        }
    }

    private void DrawConclusionsToFinalDeductions()
    {
        float ySize = 50f;

        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenConclusionToFinalDeductionRelations.Count; i++)
        {

            float xPosition = 250f;
            float yPosition = (ySize - 100f) + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            string title = Data.ChoosenConclusionToFinalDeductionRelations[i].Output1.Title;
            //Vector2 anchoredPosition = new Vector2(xPosition, yPosition);
            //GameObject circleGameObject = CreateCircle(anchoredPosition, title);    

            GameObject newConcButton = CreateNode(prefabConclusionButton, spawnPos);

            if (newConcButton == null)
            {
                foreach (var relation in buttonsToRelations)
                {
                    if (relation.Value.Input1 is Conclusion && relation.Value.Output1 is FinalDeduction)
                    {
                        buttonsToRelations.Remove(relation.Key);
                    }
                }
                conclusionsToFinalDeductionButtons.Clear();
                DrawConclusionsToFinalDeductions();
            }
            conclusionsToFinalDeductionButtons.Add(newConcButton);
            Text buttonText = (Text)newConcButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            buttonsToRelations.Add(newConcButton, Data.ChoosenConclusionToFinalDeductionRelations[i]);
            //buttonsToRelations[newConcButton] = Data.ChoosenClueRelations[i];
        }
    }

    private GameObject CreateNode(GameObject prefabConclusionButton, Vector3 spawnPos)
    {
        GameObject conclusionButton = Instantiate(prefabConclusionButton, spawnPos, Quaternion.identity);
        conclusionButton.transform.SetParent(GameObject.FindGameObjectWithTag("concCanv").transform, false);

        RectTransform rectTransform = conclusionButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = conclusionButton.GetComponent<RectTransform>().anchoredPosition;

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
        //rectTransform.anchorMin = new Vector2(0, 0);
        //rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

        connections.Add(gameObject);
    }
}
