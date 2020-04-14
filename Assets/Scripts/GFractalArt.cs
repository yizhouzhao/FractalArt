using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFractalArt
{
    //board settings
    public static float boardHeight = 4.5f;
    public static float boardWidth = 4.5f;

    public static Vector2 boardCenter = new Vector2(0f, 0f);
    public static int gridCountPerLine = 4;

    //image and collage settings
    public static int puzzleImageSize = 512;
    public static int collageSize = 100;


    //choose k item from list with length n; n >= k
    public static List<T> ChooseFrom<T>(List<T> myList, int n, int k)
    {
        List<T> retList = new List<T>();
        for(int i = 0; i < k; ++i)
        {
            retList.Add(myList[i]);
        }

        for(int i = k; i < n; ++i)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            if(j < k)
            {
                retList[j] = myList[i];
            }
        }
        return retList;
    }
     
}


public class PlayerInfo
{
    public static string playerName;

    //image
    public static AssetBundle loadedAssetBundle;
    public static int pictureIndex = 2;
    public static Texture2D questionTexture; //hold question template


    //difficult_settings
    public static int first_level_width = 4; //how many pieces need action in one level
    public static int second_level_width = 2;

}