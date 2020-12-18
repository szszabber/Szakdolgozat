using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GraphDrawer : MonoBehaviour
{
    private RectTransform graphContainer;

    private List<GameObject> conclusionButtons = new List<GameObject>();
    private List<GameObject> motivationButtons = new List<GameObject>();
    private List<GameObject> conclusionAndMotivationToFinalDeductionButtons = new List<GameObject>();
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

        buttonsToRelations.ToList().ForEach(c => Destroy(c.Key));
        buttonsToRelations.Clear();

        connections.Clear();
        conclusionButtons.Clear();
        motivationButtons.Clear();
        conclusionAndMotivationToFinalDeductionButtons.Clear();
        conclusionsToFinalDeductionButtons.Clear();

        ShowGraph();
    }

    private void ShowGraph()
    {
        DrawConclusions();
        DrawMotivations();
        DrawConclusionAndMotivationToFinalDeductions();
        DrawConclusionsToFinalDeductions();

        DrawConnectionsBetweenConclusionsAndMotivations();
        DrawConnectionsBetweenConclusionAndMotivationToFinalDeductions();
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

    private void DrawConnectionsBetweenConclusionAndMotivationToFinalDeductions()
    {
        foreach (var finalDeductionButton in conclusionAndMotivationToFinalDeductionButtons)
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
        foreach (var finalDeductionButton in conclusionsToFinalDeductionButtons)
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
        float ySize = 60f;

        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenClueRelations.Count; i++)
        {

            float xPosition = -250f;
            float yPosition = (ySize - 200f) + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            GameObject newConcButton = CreateNode(prefabConclusionButton, spawnPos);   

            string title;
            if (Data.ChoosenClueRelations[i].Output2 == null)
            {
                title = Data.ChoosenClueRelations[i].Output1.Title;
            }
            else
            {
                Button newConcButtonClick = newConcButton.GetComponent<Button>();
                Relation relationForChoosing = Data.ChoosenClueRelations[i];
                newConcButtonClick.onClick.AddListener(() => HandleGraphButtonClick(relationForChoosing));
                if (Data.ChoosenClueRelations[i].SelectedOutput == null)
                {
                    title = "Válassz konklúziót!";
                    newConcButton.GetComponentInChildren<Text>().color = new Color(255, 255, 0);
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

    private void DrawMotivations()
    {
        float ySize = 90f;
        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenConclusionRelations.Count; i++)
        {
            float xPosition = 0f;
            float yPosition = (ySize - 50) + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            //string title = Data.ChoosenConclusionRelations[i].Output1.Title;
            GameObject newMotivationButton = CreateNode(prefabConclusionButton, spawnPos);

            string title;
            if (Data.ChoosenConclusionRelations[i].Output2 == null)
            {
                title = Data.ChoosenConclusionRelations[i].Output1.Title;
            }
            else
            {
                Button newConcButtonClick = newMotivationButton.GetComponent<Button>();
                Relation relationForChoosing = Data.ChoosenConclusionRelations[i];
                newConcButtonClick.onClick.AddListener(() => HandleGraphButtonClick(relationForChoosing));
                if (Data.ChoosenConclusionRelations[i].SelectedOutput == null)
                {
                    title = "Válassz!\nMotiváció vagy ártatlan?";
                    newMotivationButton.GetComponentInChildren<Text>().color = new Color(255, 255, 0);
                }
                else
                {
                    title = Data.ChoosenConclusionRelations[i].SelectedOutput.Title;
                }
            }

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

    private void DrawConclusionAndMotivationToFinalDeductions()
    {
        float ySize = 90f;
        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Count; i++)
        {
            float xPosition = 250f;
            float yPosition = ySize + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            GameObject newFinalDeductionButton = CreateNode(prefabConclusionButton, spawnPos);

            string title = Data.ChoosenConclusionAndMotivationToFinalDeductionRelations[i].Output1.Title;

            Button newFinalDedButtonClick = newFinalDeductionButton.GetComponent<Button>();
            newFinalDedButtonClick.onClick.AddListener(HandleConclusionAndMotivationToFinalDeductionClick);

            if (newFinalDeductionButton == null)
            {
                foreach (var relation in buttonsToRelations)
                {
                    if (relation.Value.Input1 is Motivation && relation.Value.Output1 is FinalDeduction)
                    {
                        buttonsToRelations.Remove(relation.Key);
                    }
                }
                conclusionAndMotivationToFinalDeductionButtons.Clear();
                DrawConclusionAndMotivationToFinalDeductions();
            }
            conclusionAndMotivationToFinalDeductionButtons.Add(newFinalDeductionButton);
            Text buttonText = (Text)newFinalDeductionButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            buttonsToRelations.Add(newFinalDeductionButton, Data.ChoosenConclusionAndMotivationToFinalDeductionRelations[i]);
        }
    }

    private void DrawConclusionsToFinalDeductions()
    {
        float ySize = 50f;

        GameObject prefabConclusionButton = GameObject.Find("ConclusionPrefabButton");

        for (int i = 0; i < Data.ChoosenConclusionsToFinalDeductionRelations.Count; i++)
        {
            float xPosition = 250f;
            float yPosition = (ySize - 100f) + i * ySize;
            Vector3 spawnPos = new Vector3(xPosition, yPosition, 0f);

            GameObject newFinalDeductionButton = CreateNode(prefabConclusionButton, spawnPos);

            string title = Data.ChoosenConclusionsToFinalDeductionRelations[i].Output1.Title;

            Button newFinalDedButtonClick = newFinalDeductionButton.GetComponent<Button>();
            newFinalDedButtonClick.onClick.AddListener(HandleConclusionsToFinalDeductionClick);

            if (newFinalDeductionButton == null)
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
            conclusionsToFinalDeductionButtons.Add(newFinalDeductionButton);
            Text buttonText = (Text)newFinalDeductionButton.GetComponentInChildren(typeof(Text));
            buttonText.text = title;

            buttonsToRelations.Add(newFinalDeductionButton, Data.ChoosenConclusionsToFinalDeductionRelations[i]);
            //buttonsToRelations[newConcButton] = Data.ChoosenClueRelations[i];
        }
    }

    private void HandleGraphButtonClick(Relation relation)
    {
        SetChoosingCanvas(true);

        GameObject outputButton1 = GameObject.Find("ChoosingPrefabButton1");
        Text buttonText1 = (Text)outputButton1.GetComponentInChildren(typeof(Text));

        GameObject outputButton2 = GameObject.Find("ChoosingPrefabButton2");
        Text buttonText2 = (Text)outputButton2.GetComponentInChildren(typeof(Text));

        Text panel1DescText = (Text)GameObject.Find("TextDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2DescText = (Text)GameObject.Find("TextDescPanel2").GetComponentInChildren(typeof(Text));

        buttonText1.text = relation.Output1.Title.ToString();
        panel1DescText.text = relation.Output1.Desription.ToString();

        buttonText2.text = relation.Output2.Title.ToString();
        panel2DescText.text = relation.Output2.Desription.ToString();

        Button pressedOutputButton1 = outputButton1.GetComponent<Button>();
        Button pressedOutputButton2 = outputButton2.GetComponent<Button>();

        pressedOutputButton1.onClick.RemoveAllListeners();
        pressedOutputButton2.onClick.RemoveAllListeners();

        pressedOutputButton1.onClick.AddListener(() => HandleChoosenConclusion(relation, relation.Output1));
        pressedOutputButton2.onClick.AddListener(() => HandleChoosenConclusion(relation, relation.Output2));
    }

    private void HandleConclusionAndMotivationToFinalDeductionClick()
    {
        SceneManager.LoadScene(5);
    }

    private void HandleConclusionsToFinalDeductionClick()
    {
        SceneManager.LoadScene(4);
    }

    private void HandleChoosenConclusion(Relation relation, InvestigationItem investigationItem)
    {
        // ugyan azt választotta, mint a múltkor?
        if(relation.SelectedOutput == investigationItem)
        {
            SetChoosingCanvas(false);
            return;
        }

        // a másikat választotta a múltkor?
        if (relation.SelectedOutput != null)
        {
            if(investigationItem is Conclusion)
            {
                ClearConsequencesOfSelectedConclusion(relation);
            }
            else
            {
                ClearConsequencesOfSelectedMotivation(relation);
            }
        }

        relation.SelectedOutput = investigationItem;
        if (investigationItem is Conclusion)
        {
            HandleConsequencesOfSelectedConclusion(relation);
        }
        else
        {
            HandleConsequencesOfSelectedMotivation(relation);
        }

        SetChoosingCanvas(false);
        Awake();
    }

    private void HandleConsequencesOfSelectedConclusion(Relation clueRelation)
    {
        // Meg kell nézni, hogy a kiválasztott konklúzió kapcsolatban áll e egy másik konklúióval, hogy motivációt alkosson
        Relation conclusionRelation = Data.ConclusionRelations.Find(concRel => concRel.Input1 == clueRelation.SelectedOutput || concRel.Input2 == clueRelation.SelectedOutput);
        if (conclusionRelation != null)
        {
            Relation previousClueRelation = Data.ChoosenClueRelations.Find(prevClueRel =>
                   prevClueRel.SelectedOutput == conclusionRelation.Input1
                || prevClueRel.SelectedOutput == conclusionRelation.Input2 
                && prevClueRel != clueRelation);
            if (previousClueRelation != null)
            {
                if (conclusionRelation.SelectedOutput != null)
                {
                    // Azt nézem, hogy a talált motiváció kapcsolatban áll e egy konlkúzióval, ami final deductiont ad ki
                    Relation conAndMotivationRelation = Data.ConclusionAndMotivationToFinalDeductionRelations.Find(concAndMotRelation =>
                          concAndMotRelation.Input2 == conclusionRelation.SelectedOutput);
                    if (conAndMotivationRelation != null)
                    {
                        // Megnézem, hogy ki van-e választva hozzá a konklúzió
                        Relation prevClueRelation = Data.ChoosenClueRelations.Find(prevClueRel => prevClueRel.SelectedOutput == conAndMotivationRelation.Input1);
                        if (prevClueRelation != null)
                        {
                            Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Add(conAndMotivationRelation);
                        }
                    }
                }

                Data.ChoosenConclusionRelations.Add(conclusionRelation);
            }
        }

        // Megnézem, hogy a kiválasztott konkllúzió párban áll e egy másik konklúzióval, ami final deductiot ad
        Relation conclusionsToFinalDeductionRelation = Data.ConclusionToFinalDeductionRelations.Find(concToFinalDedRel =>
               concToFinalDedRel.Input1 == clueRelation.SelectedOutput
            || concToFinalDedRel.Input2 == clueRelation.SelectedOutput);
        if (conclusionsToFinalDeductionRelation != null)
        {
            Relation previousClueRelation = Data.ChoosenClueRelations.Find(prevClueRel =>
              (prevClueRel.SelectedOutput == conclusionsToFinalDeductionRelation.Input1
            || prevClueRel.SelectedOutput == conclusionsToFinalDeductionRelation.Input2)
            && prevClueRel != clueRelation);
            if (previousClueRelation != null)
            {
                Data.ChoosenConclusionsToFinalDeductionRelations.Add(conclusionsToFinalDeductionRelation);
            }
        }

        // Meg kell nézni, hogy a kiválasztott konklúzió kapcsolatban áll e egy motivációval
        Relation conclusionAndMotivationRelation = Data.ConclusionAndMotivationToFinalDeductionRelations.Find(concAndMotRel => concAndMotRel.Input1 == clueRelation.SelectedOutput);
        if (conclusionAndMotivationRelation != null)
        {
            // Megnézem, hogy a vele kapcsolatban lévő motiváció ki van e választva
            Relation prevConclusionRelation = Data.ChoosenConclusionRelations.Find(prevConRel => prevConRel.SelectedOutput == conclusionAndMotivationRelation.Input2);
            if (prevConclusionRelation != null)
            {
                Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Add(conclusionAndMotivationRelation);
            }
        }
    }

    private void HandleConsequencesOfSelectedMotivation(Relation relation)
    {
        //Megnézem, hogy a kiválasztott motiváció kapcsolatban áll-e konklúzióval
        Relation motivationAndConclusionRelation = Data.ConclusionAndMotivationToFinalDeductionRelations.Find(motAndConcRel => motAndConcRel.Input2 == relation.SelectedOutput);
        if (motivationAndConclusionRelation != null)
        {
            //Megnézem, hogy a vele kapcsolatban lévő konklúzió ki van-e választva
            Relation prevConcRelation = Data.ChoosenConclusionRelations.Find(prevConcRel => prevConcRel.SelectedOutput == motivationAndConclusionRelation.Input1);
            if (prevConcRelation != null)
            {
                Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Add(motivationAndConclusionRelation);
            }
        }
    }

    private void ClearConsequencesOfSelectedConclusion(Relation clueRelation)
    {
        // Meg kell nézni, hogy a kiválasztott konklúzió kapcsolatban áll e egy másik konklúióval
        Relation choosenConclusionRelation = Data.ChoosenConclusionRelations.Find(choosenConcRel =>
              choosenConcRel.Input1 == clueRelation.SelectedOutput
           || choosenConcRel.Input2 == clueRelation.SelectedOutput);
        if (choosenConclusionRelation != null)
        {
            Data.ChoosenConclusionRelations.Remove(choosenConclusionRelation);
            Relation conclusionAndMotivationRelation = Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Find(concAndMotRel =>
                concAndMotRel.Input2 == choosenConclusionRelation.SelectedOutput);
            if (conclusionAndMotivationRelation != null)
            {
                Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Remove(conclusionAndMotivationRelation);
            }
        }

        Relation choosenConclusionAndMotivationRelation = Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Find(choosenConcAndMotRel =>
            choosenConcAndMotRel.Input1 == clueRelation.SelectedOutput);
        if (choosenConclusionAndMotivationRelation != null)
        {
            Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Remove(choosenConclusionAndMotivationRelation);
        }

        Relation choosenConclusionToFinalDeductionRelation = Data.ChoosenConclusionsToFinalDeductionRelations.Find(choosenConcToFinalRel =>
           choosenConcToFinalRel.Input1 == clueRelation.SelectedOutput
        || choosenConcToFinalRel.Input2 == clueRelation.SelectedOutput);
        if (choosenConclusionToFinalDeductionRelation != null)
        {
            Data.ChoosenConclusionsToFinalDeductionRelations.Remove(choosenConclusionToFinalDeductionRelation);
        }
    }

    private void ClearConsequencesOfSelectedMotivation(Relation conclusionRelation)
    {
        Relation choosenConclusionAndMoticationToFinalDedRel = Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Find(choosenConcAndMotToFinalRel =>
            choosenConcAndMotToFinalRel.Input2 == conclusionRelation.SelectedOutput);
        if (choosenConclusionAndMoticationToFinalDedRel != null)
        {
            Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Remove(choosenConclusionAndMoticationToFinalDedRel);
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
        gameObject.GetComponent<Image>().color = new Color(255, 215, 0, .1f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

        connections.Add(gameObject);
    }

    private void SetChoosingCanvas(bool activate)
    {
        GameObject graphCanvas = GameObject.Find("GraphCanvas");
        GameObject choosingCanvas = graphCanvas.transform.Find("ChoosingCanvas").gameObject;
        Vector3 position = choosingCanvas.transform.position;
        choosingCanvas.transform.position = new Vector3(position.x, position.y, -0.5f);
        choosingCanvas.SetActive(activate);
    }
}
