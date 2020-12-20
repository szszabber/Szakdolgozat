using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class DataReader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // Minden alkalommal amikor a MainMenu scene betölt minden listát kiürítek
        Data.Clues.Clear();

        Data.Conclusions.Clear();

        Data.Motivations.Clear();

        Data.FinalDeductions.Clear();

        Data.ClueRelations.Clear();

        Data.ConclusionRelations.Clear();

        Data.ConclusionAndMotivationToFinalDeductionRelations.Clear();

        Data.ChoosenClueRelations.Clear();

        Data.ChoosenConclusionRelations.Clear();

        Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Clear();

        Data.ChoosenConclusionsToFinalDeductionRelations.Clear();

        Data.InterViews.Clear();
    }

    public void Start()
    {
        string data = xmlRawFile.text;

        //A program indításakor beolvasom az adatokat az xml-ből
        ReadClues(data);

        ReadConclusions(data);

        ReadMotivations(data);

        ReadFinalDeductions(data);

        ReadClueRelations(data);

        ReadConclusionRelations(data);

        ReadFinalDeductionConclusionAndMotivationRelations(data);

        ReadFinalDeductionConclusionRelations(data);
    }

    public void ReadClues(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> clues = xmlDoc.Root.Element("Clues").Elements("Clue").ToList();

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

            InvestigationItem selectedOutput = null;
            InvestigationItem conclusionOutput2 = null;

            if (clueRelationXelement.Attribute("output2ID") != null)    
            {
                int conclusionOutput2Id = Convert.ToInt32(clueRelationXelement.Attribute("output2ID").Value);
                conclusionOutput2 = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionOutput2Id);
            }
            else
            {
                selectedOutput = conclusionOutput1;
            }

            Relation clueRelation = new Relation(id, clueInput1, clueInput2, conclusionOutput1, conclusionOutput2, selectedOutput);
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

            InvestigationItem selectedOutput = null;
            InvestigationItem motivationOutput2 = null;

            if (conclsionRelationXelement.Attribute("output2ID") != null)
            {
                int motivationOutput2Id = Convert.ToInt32(conclsionRelationXelement.Attribute("output2ID").Value);
                motivationOutput2 = Data.Motivations.Find(motivation => motivation.Id == motivationOutput2Id);
            }
            else
            {
                selectedOutput = motivationOutput1;
            }

            Relation conclusionRelation = new Relation(id, conclusionInput1, conclusionInput2, motivationOutput1, motivationOutput2, selectedOutput);
            Data.ConclusionRelations.Add(conclusionRelation);
        }
    }

    private void ReadFinalDeductionConclusionAndMotivationRelations(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> finalDeductionConclusionAndMotivationRelations = xmlDoc.Root.Element("FinalDeductionClueAndMotivationPairs").Elements("ClueAndMotivationPair").ToList();

        foreach (var finalDeductionConclusionAndMotivationRelationsXelement in finalDeductionConclusionAndMotivationRelations)
        {
            int id = Convert.ToInt32(finalDeductionConclusionAndMotivationRelationsXelement.Attribute("ID").Value);
            int conclusionInputId = Convert.ToInt32(finalDeductionConclusionAndMotivationRelationsXelement.Attribute("inputConclusionId").Value);
            int motivationInputId = Convert.ToInt32(finalDeductionConclusionAndMotivationRelationsXelement.Attribute("inputMotivationId").Value);
            int finalDeductionOutputId = Convert.ToInt32(finalDeductionConclusionAndMotivationRelationsXelement.Attribute("output1ID").Value);

            InvestigationItem conclusionInput = Data.Conclusions.Find(conclusion => conclusion.Id == conclusionInputId);
            InvestigationItem motivationInput = Data.Motivations.Find(motivation => motivation.Id == motivationInputId);
            InvestigationItem finalDeductionOutput = Data.FinalDeductions.Find(finalDeduction => finalDeduction.Id == finalDeductionOutputId);

            Relation conclusionAndMotivationToFinalDeductionRelation = new Relation(id, conclusionInput, motivationInput, finalDeductionOutput);
            Data.ConclusionAndMotivationToFinalDeductionRelations.Add(conclusionAndMotivationToFinalDeductionRelation);
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

}
