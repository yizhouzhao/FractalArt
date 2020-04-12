using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        SceneManager.LoadScene("LevelSelection");
    }
}
