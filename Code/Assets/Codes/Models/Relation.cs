using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relation
{
    public int Id { get; private set; }

    public InvestigationItem Input1 { get; private set; }

    public InvestigationItem Input2 { get; private set; }

    public InvestigationItem Output { get; private set; }

    public Relation(int id, InvestigationItem input1, InvestigationItem input2, InvestigationItem output)
    {
        Id = id;
        Input1 = input1;
        Input2 = input2;
        Output = output;
    }
}
