using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Nyomok osztálya
public class Clue 
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

// Konklúziók osztálya (Szerintem a motiváció és a végső következtetés is konklúzió mert ugyanazok a tulajdonságaik)
public class Conclusion
{
    public string ID { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    public Conclusion(string iD, string title, string description)
    {
        ID = iD;
        Title = title;
        Description = description;
    }
}

// Párok osztályai
// Két nyom párosítása ad ki egy következtetést, ha jó a pár
public class ClueGroup
{
    Clue clue1;
    Clue clue2;
    Conclusion conclusion1;

    public ClueGroup(Clue clue1, Clue clue2, Conclusion conclusion1)
    {
        this.clue1 = clue1;
        this.clue2 = clue2;
        this.conclusion1 = conclusion1;
    }
}

// Két vagy három következtetés kapcsolata adja egy ember motivációját és/vagy bűnösségét
public class ConclusionGroup
{
    Conclusion conclusion1;
    Conclusion conclusion2;
    Conclusion conclusion3;
    Conclusion conclusion4;

    public ConclusionGroup(Conclusion conclusion1, Conclusion conclusion2, Conclusion conclusion3, Conclusion conclusion4)
    {
        this.conclusion1 = conclusion1;
        this.conclusion2 = conclusion2;
        this.conclusion3 = conclusion3;
        this.conclusion4 = conclusion4;
    }
}

// A végső következtetés karakterfüggő
// Daniel Johnson: 3 konklúzió adja ki a bűnösségét
// James Ryan: 1 Motiváció és egy konklúzió adja ki a bűnösségét
// John Miller: 1 Motiváció és egy konklúzió adja ki a bűnösségét



