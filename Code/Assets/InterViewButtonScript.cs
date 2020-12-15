using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterViewButtonScript : MonoBehaviour
{
    public void BackToInterViews()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        //SceneManager.LoadScene(3);
    }
}
