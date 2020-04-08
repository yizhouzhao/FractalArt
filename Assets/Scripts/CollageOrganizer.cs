using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollageOrganizer : MonoBehaviour
{
    //collage pieces 
    public List<CollageFraction> collageFractionList;

    public Texture2D currentTexture;
    public int currentTextureSize;

    public void SetCollagePieces()
    {
        for(int i = 0; i < collageFractionList.Count; i++)
        {
            Texture2D cTex = GetTexture2DForCollage(i);
            collageFractionList[i].SetImageFromTexture2D(cTex);
        }
    }

    public Texture2D GetTexture2DForCollage(int collageIndex)
    {
        int collageTempSize = GFractalArt.puzzleImageSize / 4;
        Texture2D collageTex =  new Texture2D(collageTempSize, collageTempSize, currentTexture.format, true);
        int xOffset = collageIndex % 4;
        int yOffset = collageIndex / 4;
        for (int i = 0; i < collageTempSize; i++)
        {
            for (int j = 0; j < collageTempSize; ++j)
            {
                Color color = currentTexture.GetPixel(xOffset * collageTempSize + i, yOffset * collageTempSize + j);
                collageTex.SetPixel(i, j, color);
            }
        }
        collageTex.Apply();

        return CollageImage.ScaleTexture(collageTex, GFractalArt.collageSize, GFractalArt.collageSize);
    }
}
