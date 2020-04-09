using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAssetBundles : MonoBehaviour
{

    AssetBundle myLoadedAssetbundle;
    public string path;
    public int pictureIndex;

    void Start()
    {
        LoadAssetBundle(path);
        CreateObjectFrombundle(pictureIndex.ToString());
    }

    void LoadAssetBundle(string bundleUrl)
    {
        myLoadedAssetbundle = AssetBundle.LoadFromFile(bundleUrl); //could be anything: server, path, etc.
        Debug.Log(myLoadedAssetbundle == null ? "Failed to load AssetBundle" : "Successfully loaded AssetBundle");
    }

    void CreateObjectFrombundle(string assetName)
    {
        var prefab = myLoadedAssetbundle.LoadAsset(assetName);
        Instantiate(prefab);
    }


    public void ToggleLeft()
    {
        if (pictureIndex == 1)
        {
            pictureIndex = 1;
        }
        else
        {
            Object.Destroy(pictureIndex.ToString());
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
            Object.Destroy(pictureIndex);
            pictureIndex += 1;
            CreateObjectFrombundle(pictureIndex.ToString());
        }
    }

    public void toGameScene()
    {
        SceneManager.LoadScene("GameScene2222");
    }
}

