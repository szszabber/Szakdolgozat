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
using UnityEngine.SceneManagement;
using System.Threading;

public class Reader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    private List<GameObject> clueButtons = new List<GameObject>();

    public Clue selectedClue1;

    public Clue selectedClue2;

    public UnityEngine.UI.Button selectedButton1;

    public UnityEngine.UI.Button selectedButton2;

    public void Start()
    {
        string data = xmlRawFile.text;

        Clear();

        ReadClues(data);

        ReadConclusions(data);

        ReadMotivations(data);

        ReadFinalDeductions(data);

        ReadClueRelations(data);

        ReadConclusionRelations(data);

        ReadFinalDeductionMotivationRelations(data);

        ReadFinalDeductionConclusionRelations(data);

        GenerateClueButtons();
    }

    private void Clear()
    {
        clueButtons.Clear();

        selectedButton1 = null;

        selectedButton2 = null;

        selectedClue1 = null;

        selectedClue2 = null;

        Data.Clues.Clear();

        Data.Conclusions.Clear();

        Data.Motivations.Clear();

        Data.FinalDeductions.Clear();

        Data.ClueRelations.Clear();

        Data.ConclusionRelations.Clear();

        Data.MotivationRelations.Clear();

        Data.ChoosenClueRelations.Clear();

        Data.ChoosenConclusionRelations.Clear();

        Data.ChoosenMotivationRelations.Clear();
    }

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue");

        foreach (XElement clueXelement in clues)
        {
            int id = Convert.ToInt32(clueXelement.Attribute("ID").Value);
            string title = clueXelement.Element("Title").Value;
            string description = clueXelement.Element("Description").Value;
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
            string title = conclusionXelement.Element("Title").Value;
            string description = conclusionXelement.Element("Description").Value;
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
            string title = motivationXelement.Element("Title").Value;
            string description = motivationXelement.Element("Description").Value;
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
            string title = finalDeductionXelement.Element("Title").Value;
            string description = finalDeductionXelement.Element("Description").Value;
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
            int id = Convert.ToInt32(clueRelationXelement.Attribute("ID").Value);
            int clueInput1Id = Convert.ToInt32(clueRelationXelement.Attribute("input1ID").Value);
            int clueInput2Id = Convert.ToInt32(clueRelationXelement.Attribute("input2ID").Value);
            int conclusionOutput1Id = Convert.ToInt32(clueRelationXelement.Attribute("output1ID").Value);


            InvestigationItem clueInput1 = Data.Clues.Find(clue => clue.Id == clueInput1Id);
            InvestigationItem clueInput2 = Data.Clues.Find(clue => clue.Id == clueInput2Id);
            InvestigationItem conclusionOutput1 = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionOutput1Id);

            InvestigationItem conclusionOutput2 = null;
            if (clueRelationXelement.Attribute("output2ID") != null)
            {
                int conclusionOutput2Id = Convert.ToInt32(clueRelationXelement.Attribute("output2ID").Value);
                conclusionOutput2 = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionOutput2Id);
            }

            Relation clueRelation = new Relation(id, clueInput1, clueInput2, conclusionOutput1, conclusionOutput2);
            Data.ClueRelations.Add(clueRelation);
        }
    }

    private void ReadConclusionRelations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> conclusionRelations = xmlDoc.Root.Element("ConclusionPairs").Elements("ConclusionPair").ToList();

        foreach (var conclsionRelationXelement in conclusionRelations)
        {
            int id = Convert.ToInt32(conclsionRelationXelement.Attribute("ID").Value);
            int conclusionInput1Id = Convert.ToInt32(conclsionRelationXelement.Attribute("input1ID").Value);
            int conclusionInput2Id = Convert.ToInt32(conclsionRelationXelement.Attribute("input2ID").Value);
            int motivationOutput1Id = Convert.ToInt32(conclsionRelationXelement.Attribute("output1ID").Value);

            InvestigationItem conclusionInput1 = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionInput1Id);
            InvestigationItem conclusionInput2 = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionInput2Id);
            InvestigationItem motivationOutput1 = Data.Motivations.Find(motivation => motivation.Id == motivationOutput1Id);

            InvestigationItem motivationOutput2 = null;
            if (conclsionRelationXelement.Attribute("output2ID") != null)
            {
                int motivationOutput2Id = Convert.ToInt32(conclsionRelationXelement.Attribute("output2ID").Value);
                motivationOutput2 = Data.Motivations.Find(motivation => motivation.Id == motivationOutput2Id);
            }

            Relation conclusionRelation = new Relation(id, conclusionInput1, conclusionInput2, motivationOutput1, motivationOutput2);
            Data.ConclusionRelations.Add(conclusionRelation);
        }
    }

    private void ReadFinalDeductionMotivationRelations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> finalDeductionMotivationRelations = xmlDoc.Root.Element("FinalDeductionMotivationPairs").Elements("FinalDeductionGuiltyPerson").ToList();

        foreach (var finalDeductionMotivationRelationsXelement in finalDeductionMotivationRelations)
        {
            int id = Convert.ToInt32(finalDeductionMotivationRelationsXelement.Attribute("ID").Value);
            int guiltInput1Id = Convert.ToInt32(finalDeductionMotivationRelationsXelement.Attribute("input1ID").Value);
            int guiltInput2Id = Convert.ToInt32(finalDeductionMotivationRelationsXelement.Attribute("input2ID").Value);
            int guiltOutputId = Convert.ToInt32(finalDeductionMotivationRelationsXelement.Attribute("output1ID").Value);

            InvestigationItem guiltInput1 = Data.Motivations.Find(motivation => motivation.Id == guiltInput1Id);
            InvestigationItem guiltInput2 = Data.Motivations.Find(motivation => motivation.Id == guiltInput2Id);
            InvestigationItem guiltOutput = Data.FinalDeductions.Find(finalDeduction => finalDeduction.Id == guiltOutputId);

            Relation finalDeductionMotivationRelation = new Relation(id, guiltInput1, guiltInput2, guiltOutput);
            Data.MotivationRelations.Add(finalDeductionMotivationRelation);
        }
    }

    private void ReadFinalDeductionConclusionRelations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> finalDeductionConclusionRelations = xmlDoc.Root.Element("FinalDeductionConclusionPairs").Elements("FinalDeductionGuiltyPerson").ToList();

        foreach (var finalDeductionConclusionRelationsXelement in finalDeductionConclusionRelations)
        {
            int id = Convert.ToInt32(finalDeductionConclusionRelationsXelement.Attribute("ID").Value);
            int guiltInput1Id = Convert.ToInt32(finalDeductionConclusionRelationsXelement.Attribute("input1ID").Value);
            int guiltInput2Id = Convert.ToInt32(finalDeductionConclusionRelationsXelement.Attribute("input2ID").Value);
            int guiltOutputId = Convert.ToInt32(finalDeductionConclusionRelationsXelement.Attribute("output1ID").Value);

            InvestigationItem guiltInput1 = Data.Conclusions.Find(conclusion => conclusion.Id == guiltInput1Id);
            InvestigationItem guiltInput2 = Data.Conclusions.Find(conclusion => conclusion.Id == guiltInput2Id);
            InvestigationItem guiltOutput = Data.FinalDeductions.Find(finalDeduction => finalDeduction.Id == guiltOutputId);

            Relation finalDeductionConclusionRelation = new Relation(id, guiltInput1, guiltInput2, guiltOutput);
            Data.ConclusionToFinalDeductionRelations.Add(finalDeductionConclusionRelation);
        }
    }

    private void GenerateClueButtons()
    {
        GameObject prefabButton = GameObject.Find("CluePrefabButton");

        UnityEngine.UI.Button plusButton = GameObject.Find("PlusButton").GetComponent<UnityEngine.UI.Button>();
        plusButton.onClick.RemoveAllListeners();
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

            // Ezzel itt gond volt mert valamiért megnövelte a méretet --> newButton.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
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
        // Ha a megtalált GameObject nem ClueButton tag-el van ellátva akkor false-t ad vissza
        if (gameObjectA.tag != "ClueButton")
        {
            return false;
        }

        RectTransform rectTransformA = gameObjectA.transform as RectTransform;
        RectTransform rectTransformB = gameObjectB.transform as RectTransform;

        Rect rectA = CalculateRect(rectTransformA);
        Rect rectB = CalculateRect(rectTransformB);

        // Megvizsgáljuk, hogy a két gameObject generálás után fedésben van e
        return rectA.Overlaps(rectB, true);
    }

    public static Vector3[] AddPaddingToRect(Vector3[] corners)
    {
        // Kibővítem a clueButton-ok szélességét és magasságát, hogy a generálásnál, ne kerüljenek közvetlen egymás mellé

        corners[0].x -= 2;
        corners[0].y -= 2;
        corners[1].x -= 2;
        corners[1].y += 2;
        corners[2].x += 2;
        corners[2].y += 2;
        corners[3].x += 2;
        corners[3].y -= 2;

        return corners;
    }

    public static Rect CalculateRect(RectTransform rectTransform)
    {
        //a recttransform 4 sarkát lekérdezem worldspace-ben
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        corners = AddPaddingToRect(corners);

        //kiszámítom a szélességét és magasságát
        Vector2 size = new Vector2(corners[3].x - corners[0].x, corners[1].y - corners[0].y); //Corners[0] a bal alsó, Corners[1] a bal felső, Corners[2] a jobb felső, Corners[3] a jobb alsó sarok

        //létrehozok egy Rect-et. 
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

        int minX = -210;
        int maxX = 210;
        int minY = -130;
        int maxY = 130;

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

        // Megkeresem a képernyő alján levő panelek text komponenseit
        Text panel1TitleText = (Text)GameObject.Find("SelectedClueTitlePanel1").GetComponentInChildren(typeof(Text));
        Text panel2TitleText = (Text)GameObject.Find("SelectedClueTitlePanel2").GetComponentInChildren(typeof(Text));
        Text panel1DescText = (Text)GameObject.Find("SelectedClueDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2DescText = (Text)GameObject.Find("SelectedClueDescPanel2").GetComponentInChildren(typeof(Text));

        // ClueButton Text komponense
        Text buttonText = (Text)button.GetComponentInChildren(typeof(Text));
        Clue clue = Data.Clues.Find(c => c.Title == buttonText.text);

        // Mikor kerül az első panelre és mikor a másodikra a Clue leírása
        if (selectedClue1 == null)
        {
            selectedClue1 = clue;
            panel1DescText.text = clue.Desription;
            panel1TitleText.text = clue.Title;
            selectedButton1 = button;
        }
        else if (selectedClue2 == null && selectedClue1 != clue)
        {
            selectedClue2 = clue;
            panel2DescText.text = clue.Desription;
            panel2TitleText.text = clue.Title;
            selectedButton2 = button;
        }
        else if (selectedClue1 == clue && selectedClue2 == null)
        {
            selectedClue1 = null;
            panel1DescText.text = "";
            panel1TitleText.text = "";
            selectedButton1 = null;
        }
        else if (selectedClue2 == clue && selectedClue1 != null)
        {
            selectedClue2 = null;
            panel2DescText.text = "";
            panel2TitleText.text = "";
            selectedButton2 = null;
        }
        //else if(selectedClue1 != null && selectedClue2 != null)
        //{
        //    selectedClue1 = clue;
        //    panel1DescText.text = clue.Desription;
        //    panel1TitleText.text = clue.Title;
        //    selectedButton1 = button;

        //    selectedClue2 = null;
        //    panel2DescText.text = "";
        //    panel2TitleText.text = "";
        //    selectedButton2 = null;
        //}
        else
        {
            selectedClue1 = clue;
            panel1DescText.text = clue.Desription;
            panel1TitleText.text = clue.Title;
            selectedButton1 = button;

            selectedClue2 = null;
            panel2DescText.text = "";
            panel2TitleText.text = "";
            selectedButton2 = null;
        }
    }

    public void HandlePlusButtonClick()
    {
        // Megkeresem a képernyő alján levő panelek text komponenseit
        Text panel1TitleText = (Text)GameObject.Find("SelectedClueTitlePanel1").GetComponentInChildren(typeof(Text));
        Text panel2TitleText = (Text)GameObject.Find("SelectedClueTitlePanel2").GetComponentInChildren(typeof(Text));
        Text panel1DescText = (Text)GameObject.Find("SelectedClueDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2DescText = (Text)GameObject.Find("SelectedClueDescPanel2").GetComponentInChildren(typeof(Text));

        if (selectedClue1 == null || selectedClue2 == null)
        {
            //Toast Unity pack a pop up üzenet megjelenítésére (akkor jelenik meg, ha nem választott ki semmit a felhasználó)
            Toast.Instance.Show("A párosításhoz nyomokat kell választani!", 1f, Toast.ToastColor.Dark);
            return;
        }

        // A listából kikeressük azt a kapcsolatot, amiben szerepel a lenyomott clue
        Relation clueRelation = Data.ClueRelations.Find(crelation => crelation.Input1 == selectedClue1 || crelation.Input2 == selectedClue1);
        if (clueRelation != null && (selectedClue2 == clueRelation.Input1 || selectedClue2 == clueRelation.Input2))
        {
            Relation conclusionRelation = Data.ConclusionRelations.Find(cRelation => cRelation.Input1 == clueRelation.Output1 || cRelation.Input2 == clueRelation.Output1);
            if (conclusionRelation != null)
            {
                Relation previousClueRelation = Data.ChoosenClueRelations.Find(clueRel => clueRel.Output1 == conclusionRelation.Input1 || clueRel.Output1 == conclusionRelation.Input2);
                if (previousClueRelation != null)
                {
                    Relation motivationRelation = Data.MotivationRelations.Find(motivationRel => motivationRel.Input1 == conclusionRelation.Output1 || motivationRel.Input2 == conclusionRelation.Output1);
                    if (motivationRelation != null)
                    {
                        Data.ChoosenMotivationRelations.Add(motivationRelation);
                    }

                    Data.ChoosenConclusionRelations.Add(conclusionRelation);
                }
            }

            Relation conclusionToFinalDeductionRelation = Data.ConclusionToFinalDeductionRelations.Find(concToFinalDed => concToFinalDed.Input1 == clueRelation.Output1 || concToFinalDed.Input2 == clueRelation.Output1);
            if (conclusionToFinalDeductionRelation != null)
            {
                Relation previousClueRelation = Data.ChoosenClueRelations.Find(clueRel => clueRel.Output1 == conclusionToFinalDeductionRelation.Input1 || clueRel.Output1 == conclusionToFinalDeductionRelation.Input2);
                if (previousClueRelation != null)
                {
                    Data.ChoosenConclusionToFinalDeductionRelations.Add(conclusionToFinalDeductionRelation);
                }
            }

            Data.ChoosenClueRelations.Add(clueRelation);

            //A sikeresen párba állított nyomok eltűnnek a képernyőről
            selectedButton1.gameObject.SetActive(false);
            selectedButton2.gameObject.SetActive(false);

            //Toast Unity pack a pop up üzenet megjelenítésére (akkor jelenik meg, ha sikeres a párosítás)
            Toast.Instance.Show("Sikeres párosítás!\nA konklúzió felkerült a gráfra!", 2f, Toast.ToastColor.Green);

            //GraphDrawer graphDrawer = (GraphDrawer)FindObjectOfType<GraphDrawer>();
            //graphDrawer.Awake();
        }
        else
        {
            //Letöltött Toast Unity pack a pop up üzenet megjelenítésére (akkor jelenik meg, ha nincs párban a két kiválasztott nyom)
            Toast.Instance.Show("A kiválasztott nyomok nem alkotnak párt!", 2f, Toast.ToastColor.Red);
        }

        //Ha sikerül a párosítás ha nem, az elmentett gombokat, panelTexteket és nyomokat alaphelyzetbe állítom
        selectedClue1 = null;
        panel1DescText.text = "";
        panel1TitleText.text = "";
        selectedButton1 = null;
        selectedClue2 = null;
        panel2DescText.text = "";
        panel2TitleText.text = "";
        selectedButton2 = null;
    }
}