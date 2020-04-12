using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class imageZoom : MonoBehaviour
{
    public ImageZoomExplorer imageZoomExplorerObject;

    public void OnClickButton()
    {
        Sprite TargetSprite;
        TargetSprite = PlayerInfo.loadedAssetBundle.LoadAsset<Sprite>(PlayerInfo.pictureIndex.ToString());

        imageZoomExplorerObject.OpenImageExplorer(TargetSprite);
    }
}
