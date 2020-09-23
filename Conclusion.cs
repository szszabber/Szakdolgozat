using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

 public class Conclusions
 {
    [XmlAttribute("ConclusionID")]
    public string ConclusionID;

    [XmlAttribute("ConclusionTitle")]
    public string ConclusionTitle;
}
