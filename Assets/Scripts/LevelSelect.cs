using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private int preScene;
    private void Start()
    {
        preScene = SceneManager.GetActiveScene().buildIndex - 1;

    }
    public void GoBack()
    {
        SceneManager.LoadScene(preScene);
    }


}