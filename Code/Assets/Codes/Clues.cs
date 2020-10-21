using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class Clues
{
    [XmlAttribute("ID")]
    public string ClueID;

    [XmlElement("ClueTitle")]
    public string ClueTitle;

    [XmlElement("ClueDesc")]
    public string ClueDesc;
}

