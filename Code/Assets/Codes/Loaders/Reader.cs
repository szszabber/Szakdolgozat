using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Xml.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UIElements;

public class Reader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    private List<GameObject> clueButtons;

    public Clue selectedClue1;

    public Clue selectedClue2;

    void Start()
    {
        string data = xmlRawFile.text;

        ReadClues(data);

        ReadConclusions(data);

        ReadMotivations(data);

        ReadFinalDeductions(data);

        ReadClueRelations(data);

        ReadConclusionRelations(data);

        ReadFinalDeductionsRelations(data);

        GenerateClueButtons();
    }

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue");

        foreach (XElement clueXelement in clues)
        {
            int id = Convert.ToInt32(clueXelement.Attribute("ID").Value);
            string title = clueXelement.Element("ClueTitle").Value;
            string description = clueXelement.Element("ClueDescription").Value;
            Clue clue = new Clue(id, title, description);
            Data.Clues.Add(clue);
        }
    }

    private void ReadConclusions(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> conclusions = xmlDoc.Root.Element("Conclusions").Elements("Conclusion").ToList();

        foreach (XElement conclusionXelement in conclusions)
        {
            int id = Convert.ToInt32(conclusionXelement.Attribute("ID").Value);
            string title = conclusionXelement.Element("ConclusionTitle").Value;
            string description = conclusionXelement.Element("ConclusionDescription").Value;
            Conclusion conclusion = new Conclusion(id, title, description);
            Data.Conclusions.Add(conclusion);
        }
    }

    private void ReadMotivations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> motivations = xmlDoc.Root.Element("Motivations").Elements("Motivation").ToList();

        foreach (XElement motivationXelement in motivations)
        {
            int id = Convert.ToInt32(motivationXelement.Attribute("ID").Value);
            string title = motivationXelement.Element("MotivationTitle").Value;
            string description = motivationXelement.Element("MotivationDescription").Value;
            Motivation motivation = new Motivation(id, title, description);
            Data.Motivations.Add(motivation);
        }
    }

    private void ReadFinalDeductions(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> finalDeductions = xmlDoc.Root.Element("FinalDeductions").Elements("FinalDeduction").ToList();

        foreach (XElement finalDeductionXelement in finalDeductions)
        {
            int id = Convert.ToInt32(finalDeductionXelement.Attribute("ID").Value);
            string title = finalDeductionXelement.Element("FinalDeductionTitle").Value;
            string description = finalDeductionXelement.Element("FinalDeductioDescription").Value;
            FinalDeduction finalDeduction = new FinalDeduction(id, title, description);
            Data.FinalDeductions.Add(finalDeduction);
        }
    }

    private void ReadClueRelations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> clueRelations = xmlDoc.Root.Element("CluePairs").Elements("CluePair").ToList();

        foreach (XElement clueRelationXelement in clueRelations)
        {
            int id = Convert.ToInt32(clueRelationXelement.Attribute("pID").Value);
            int clueInput1Id = Convert.ToInt32(clueRelationXelement.Attribute("input1ID").Value);
            int clueInput2Id = Convert.ToInt32(clueRelationXelement.Attribute("input2ID").Value);
            int conclusionOutputId = Convert.ToInt32(clueRelationXelement.Attribute("outputID").Value);

            InvestigationItem clueInput1 = Data.Clues.Find(clue => clue.Id == clueInput1Id);
            InvestigationItem clueInput2 = Data.Clues.Find(clue => clue.Id == clueInput2Id);
            InvestigationItem conclusionOutput = Data.Clues.Find(clue => clue.Id == conclusionOutputId);

            Relation clueRelation = new Relation(id, clueInput1, clueInput2, conclusionOutput);
            Data.ClueRelations.Add(clueRelation);
        }
    }

    private void ReadConclusionRelations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> conclusionRelations = xmlDoc.Root.Element("ConclusionPairs").Elements("ConclusionPair").ToList();

        foreach (var conclsionRelationXelement in conclusionRelations)
        {
            int id = Convert.ToInt32(conclsionRelationXelement.Attribute("cID").Value);
            int conclusionInput1Id = Convert.ToInt32(conclsionRelationXelement.Attribute("concInput1ID").Value);
            int conclusionInput2Id = Convert.ToInt32(conclsionRelationXelement.Attribute("concInput2ID").Value);
            int motivationOutputId = Convert.ToInt32(conclsionRelationXelement.Attribute("motivationOutput").Value);

            InvestigationItem conclusionInput1 = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionInput1Id);
            InvestigationItem conclusionInput2 = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionInput2Id);
            InvestigationItem motivationOutput = Data.Conclusions.Find(conclusion => conclusion.Id == motivationOutputId);

            Relation conclusionRelation = new Relation(id, conclusionInput1, conclusionInput2, motivationOutput);
            Data.ConclusionRelations.Add(conclusionRelation);
        }
    }

    private void ReadFinalDeductionsRelations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> finalDeductionRelations = xmlDoc.Root.Element("FinalDeductionPairs").Elements("FinalDeductionGuiltyPerson").ToList();

        foreach (var finalDeductionRelationsXelement in finalDeductionRelations)
        {
            int id = Convert.ToInt32(finalDeductionRelationsXelement.Attribute("gID").Value);
            int guiltInput1Id = Convert.ToInt32(finalDeductionRelationsXelement.Attribute("guiltInput1").Value);
            int guiltInput2Id = Convert.ToInt32(finalDeductionRelationsXelement.Attribute("guiltInput2").Value);
            int guiltOutputId = Convert.ToInt32(finalDeductionRelationsXelement.Attribute("guiltOutput").Value);

            InvestigationItem guiltInput1 = Data.FinalDeductions.Find(finalDeduction => finalDeduction.Id == guiltInput1Id);
            InvestigationItem guiltInput2 = Data.FinalDeductions.Find(finalDeduction => finalDeduction.Id == guiltInput2Id);
            InvestigationItem guiltOutput = Data.FinalDeductions.Find(finalDeduction => finalDeduction.Id == guiltOutputId);

            Relation finalDeductionRelation = new Relation(id, guiltInput1, guiltInput2, guiltOutput);
            Data.FinalDeductionRelations.Add(finalDeductionRelation);
        }
    }

    private void GenerateClueButtons()
    {
        clueButtons = new List<GameObject>();

        GameObject prefabButton = GameObject.Find("CluePrefabButton");
        //GameObject prefabButton = GameObject.Find("ClueButton");

        UnityEngine.UI.Button plusButton = GameObject.Find("PlusButton").GetComponent<UnityEngine.UI.Button>();
        plusButton.onClick.AddListener(HandlePlusButtonClick);

        for (int i = 0; i < Data.Clues.Count; i++)
        {
            Clue clue = Data.Clues[i];

            GameObject newButton = GenerateNewClueButton(prefabButton);

            if (newButton == null)
            {
                clueButtons.Clear();
                GenerateClueButtons();
            }

            // newButton.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
            clueButtons.Add(newButton);
            UnityEngine.UI.Button button = newButton.GetComponent<UnityEngine.UI.Button>();
            //button.onClick.AddListener(HandleClueButtonClick);
            button.onClick.AddListener(() => HandleClueButtonClick(button));

            Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
            buttonText.text = clue.Title;
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().Render();
        }

        foreach (var button in clueButtons)
        {
            button.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
        }
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

        GameObject newButton = Instantiate(prefabClueButton, spawnPosition + (spawnPosition), Quaternion.identity) as GameObject;
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
            float xPos = UnityEngine.Random.Range(minX, maxX);
            float yPos = UnityEngine.Random.Range(minY, maxY);
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

        if (tryCount == 100000)
        {
            return null;
        }

        return newButton;
    }

    public void HandleClueButtonClick(UnityEngine.UI.Button button)
    {
        Text panel1Text = (Text)GameObject.Find("SelectedClueDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2Text = (Text)GameObject.Find("SelectedClueDescPanel2").GetComponentInChildren(typeof(Text));

        Text buttonText = (Text)button.GetComponentInChildren(typeof(Text));
        Clue clue = Data.Clues.Find(c => c.Title == buttonText.text);


        if (selectedClue1 == null)
        {
            selectedClue1 = clue;
            panel1Text.text = clue.Desription;
        }
        else if (selectedClue2 == null && selectedClue1 != clue)
        {
            selectedClue2 = clue;
            panel2Text.text = clue.Desription;
        }
        else
        {
            selectedClue1 = clue;
            panel1Text.text = clue.Desription;
            selectedClue2 = null;
            panel2Text.text = "";
        }
    }

    public void HandlePlusButtonClick()
    {
        Text panel1Text = (Text)GameObject.Find("SelectedClueDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2Text = (Text)GameObject.Find("SelectedClueDescPanel2").GetComponentInChildren(typeof(Text));

        if (selectedClue1 == null || selectedClue2 == null)
        {
            return;
        }

        Relation relation = Data.ClueRelations.Find(crelation => crelation.Input1 == selectedClue1 || crelation.Input2 == selectedClue1);

        if (relation != null && (selectedClue2 == relation.Input1 || selectedClue2 == relation.Input2))
        {
            Data.ChoosenRelations.Add(relation);
        }

        selectedClue1 = null;
        panel1Text.text = "";
        selectedClue2 = null;
        panel2Text.text = "";
    }
}