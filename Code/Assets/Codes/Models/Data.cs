using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static List<Clue> Clues { get; set; } = new List<Clue>();

    public static List<Conclusion> Conclusions { get; set; } = new List<Conclusion>();

    public static List<Motivation> Motivations { get; set; } = new List<Motivation>();

    public static List<FinalDeduction> FinalDeductions { get; set; } = new List<FinalDeduction>();

    public static List<Relation> ClueRelations { get; set; } = new List<Relation>();

    public static List<Relation> ConclusionRelations { get; set; } = new List<Relation>();

    public static List<Relation> MotivationRelations { get; set; } = new List<Relation>();

    public static List<Relation> ConclusionToFinalDeductionRelations { get; set; } = new List<Relation>();

    public static List<Relation> ChoosenClueRelations { get; set; } = new List<Relation>();

    public static List<Relation> ChoosenConclusionRelations { get; set; } = new List<Relation>();

    public static List<Relation> ChoosenMotivationRelations { get; set; } = new List<Relation>();

    public static List<Relation> ChoosenConclusionToFinalDeductionRelations { get; set; } = new List<Relation>();

}
