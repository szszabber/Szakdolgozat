using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;

public class MainScene : MonoBehaviour
{
    //Vissza gomb a főmenübe
    public void BackToMain(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
    //Gráf scene betöltése
    /*public void LoadGraph()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }*/
}


