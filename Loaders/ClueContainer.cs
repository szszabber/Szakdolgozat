using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// forrás: https://www.youtube.com/watch?v=nYWlB7HRNSE

[XmlRoot("deduction")]
public class ClueContainer
{
   [XmlArray("clues")]
   [XmlArrayItem("clue")]
   public List<Clues> clues=new List<Clues>();

   public static ClueContainer Load(string path){
       TextAsset _xml = Resources.Load<TextAsset>(path);
       XmlSerializer serializer = new XmlSerializer(typeof(ClueContainer));
       StringReader reader = new StringReader(_xml.text);
       ClueContainer clues = serializer.Deserialize(reader) as ClueContainer;
       reader.Close();
       return clues;
   }
}
