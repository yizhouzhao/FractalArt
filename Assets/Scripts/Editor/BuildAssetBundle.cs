using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class BuildAssetBundle : Editor
{
    [MenuItem("Assets/Build AssetBundle")]

    static void BuildAllAssetBundles()
    {
        Debug.Log(Application.persistentDataPath);
        BuildPipeline.BuildAssetBundles(@Application.persistentDataPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
    }
}