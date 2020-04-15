using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadAssetBundles : MonoBehaviour
{
    public GameObject loadingScreenObj;
    public Slider sliderBar;
    public GameObject LevelSelect;

    AssetBundle myLoadedAssetbundle;
    private string path;
    public int pictureIndex = 1;
  
    public GameObject img;

    void Start()
    {
        path = Application.persistentDataPath + "/raw_images";
        LoadAssetBundle(path);
        CreateObjectFrombundle(pictureIndex.ToString());
    }

    void LoadAssetBundle(string bundleUrl)
    {
        myLoadedAssetbundle = AssetBundle.LoadFromFile(bundleUrl); //could be anything: server, path, etc.
        Debug.Log(myLoadedAssetbundle == null ? "Failed to load AssetBundle" : "Successfully loaded AssetBundle");

        //Save asset bundle to player info
        PlayerInfo.loadedAssetBundle = myLoadedAssetbundle;
        PlayerInfo.questionTexture = PlayerInfo.loadedAssetBundle.LoadAsset<Texture2D>("question_mark");
    }

    void CreateObjectFrombundle(string assetName)
    {
        Texture2D texture = myLoadedAssetbundle.LoadAsset<Texture2D>(assetName);
        SpriteRenderer spriteRenderer = img.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }


    public void ToggleLeft()
    {
        if (pictureIndex == 1)
        {
            pictureIndex = 1;
        }
        else
        {
            pictureIndex -= 1;
            CreateObjectFrombundle(pictureIndex.ToString());
        }
    }

    public void ToggleRight()
    {
        if (pictureIndex == 4)
        {
            pictureIndex = 4;
        }
        else
        {
            pictureIndex += 1;
            CreateObjectFrombundle(pictureIndex.ToString());
        }
    }

    public void toGameScene()
    {
        //save picture index
        PlayerInfo.pictureIndex = pictureIndex;
        loadingScreenObj.SetActive(true);
        LevelSelect.SetActive(false);
        StartCoroutine(LoadNewScene());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while(!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            Debug.Log(progress);
            sliderBar.value = progress;
            yield return null;
        }
    }

    public void GoBack()
    {
     //   AssetBundle.UnloadAllAssetBundles(true);
        SceneManager.LoadScene("Menu");
    }
}
