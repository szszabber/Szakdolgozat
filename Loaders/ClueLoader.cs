using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ClueLoader : MonoBehaviour
{
    public const string path = "Database";
    
    void Start(){
        ClueContainer ic = ClueContainer.Load(path);

        foreach(Clues clue in ic.clues){
            print(clue.ClueID);
        } 
    }
}
