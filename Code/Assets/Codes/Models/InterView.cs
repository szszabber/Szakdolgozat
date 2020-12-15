using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterView
{
    public string ImageName { get; private set; }
    public string Name { get; private set; }
    public string Text { get; private set; }

    public InterView(string imageName, string name, string text)
    {
        ImageName = imageName;
        Name = name;
        Text = text;
    }
}
