using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFractalArt
{
    public static float boardHeight = 4.5f;
    public static float boardWidth = 4.5f;

    public static Vector2 boardCenter = new Vector2(0f, 0f);
    public static int gridCountPerLine = 4;

    public static int puzzleImageSize = 512;
    public static int collageSize = 100;
}


public class PlayerInfo
{
    public static string playerName;

    //image
    public static AssetBundle loadedAssetBundle;
    public static int pictureIndex = 1;

}