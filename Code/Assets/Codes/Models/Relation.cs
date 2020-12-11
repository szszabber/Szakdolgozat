using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relation
{
    public int Id { get; private set; }

    public InvestigationItem Input1 { get; private set; }

    public InvestigationItem Input2 { get; private set; }

    public InvestigationItem Output1 { get; private set; }

    public InvestigationItem Output2 { get; private set; }

    public InvestigationItem SelectedOutput { get; set; }

    public Relation(int id, InvestigationItem input1, InvestigationItem input2, InvestigationItem output1, InvestigationItem output2 = null, InvestigationItem SelectedOutput = null)
    {
        Id = id;
        Input1 = input1;
        Input2 = input2;
        Output1 = output1;
        Output2 = output2;
        SelectedOutput = null;
    }
}
