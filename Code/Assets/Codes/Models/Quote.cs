using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quote
{
    public int Id { get; private set; }

    public string Desription { get; private set; }

    public Quote(int id, string desription)
    {
        Id = id;
        Desription = desription;
    }
}
