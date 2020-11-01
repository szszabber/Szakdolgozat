using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Animator animator;
    public int levelToLoad;

    void Update()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (Input.GetButtonDown("NewGameButton"))
        {
            SceneChange(1);
        }
        
    }

    public void SceneChange(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void SceneChangeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
