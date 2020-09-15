using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    // Ezzel a függvénnyel oldjuk meg a zenét, hogy ne álljon meg a jelenetek váltásakor
    void Awake(){
        DontDestroyOnLoad(transform.gameObject);
    }
}
