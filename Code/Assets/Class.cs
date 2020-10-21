using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue //Nyomok
{
    public string ID { get; private set; }
    public string Title { get; private set; }
    public string Desription { get; private set; }

    public Clue(string iD, string title, string desription)
    {
        ID = iD;
        Title = title;
        Desription = desription;
    }
}

public class Conclusion //A motiváció is egyfajta konklúzió
{
    public string ID { get; private set; }
    public string Description { get; private set; }

    public Conclusion(string iD, string description)
    {
        ID = iD;
        Description = description;
    }
}

//public class Motivation : Conclusion
//{
    
//}

// Párok osztályai

// Két nyom ad ki egy következtetést
public class ClueGroup
{
    //Bemenet
    Clue clue1;
    Clue clue2;
    //Kimenet
    Conclusion conclusion1;

    public ClueGroup(Clue clue1, Clue clue2, Conclusion conclusion1)
    {
        this.clue1 = clue1;
        this.clue2 = clue2;
        this.conclusion1 = conclusion1;
    }
}

// Két következtetés kapcsolata adja egy ember motivációját
public class ConclusionGroup
{
    //Következtetés bemenet
    Conclusion conclusion1;
    Conclusion conclusion2;
    //Motiváció, mint kimenet
    Conclusion conclusion3;

    public ConclusionGroup(Conclusion conclusion1, Conclusion conclusion2, Conclusion conclusion3)
    {
        this.conclusion1 = conclusion1;
        this.conclusion2 = conclusion2;
        this.conclusion3 = conclusion3;
    }
}

//public class FinalDeduction
//{
//    //Bemenet
//    Conclusion conclusion1;
//    Conclusion conclusion2;
//    //Kimenet
//    Conclusion conclusion3;

//    public FinalDeduction(Conclusion conclusion1, Conclusion conclusion2, Conclusion conclusion3)
//    {
//        this.conclusion1 = conclusion1;
//        this.conclusion2 = conclusion2;
//        this.conclusion3 = conclusion3;
//    }
//}

// A végső következtetés karakterfüggő
// Daniel Johnson: 3 konklúzió adja ki a bűnösségét
// James Ryan: 1 Motiváció és egy konklúzió adja ki a bűnösségét
// John Miller: 1 Motiváció és egy konklúzió adja ki a bűnösségét



