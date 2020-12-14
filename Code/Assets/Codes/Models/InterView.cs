using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterView
{
    public string Name { get; private set; }
    public string Text { get; private set; }

    public InterView(string name, string text)
    {
        Name = name;
        Text = text;
    }
}
