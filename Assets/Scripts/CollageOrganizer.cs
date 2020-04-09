using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollageOrganizer : MonoBehaviour
{
    //collage pieces 
    [Header("Collage Pieces")]
    public List<CollageFraction> collageFractionList;
    public List<Vector3> gridPointList;

    [Header("Picture")]
    public Texture2D currentTexture;
    public int currentTextureSize;

    void Awake()
    {

        //init grid positions
        gridPointList = new List<Vector3>();

        float boardLeft = GFractalArt.boardCenter.x - GFractalArt.boardWidth / 2;
        float boardRight = GFractalArt.boardCenter.x + GFractalArt.boardWidth / 2;

        float boardTop = GFractalArt.boardCenter.y + GFractalArt.boardHeight / 2;
        float boardBottom = GFractalArt.boardCenter.y - GFractalArt.boardHeight / 2;

        float step = GFractalArt.boardWidth / GFractalArt.gridCountPerLine;
        float z = this.transform.position.z;

        
        for (float y = boardBottom + step / 2; y <= boardTop - step / 2; y += step)
        {
            for (float x = boardLeft + step / 2; x <= boardRight - step / 2; x += step)
            {
                gridPointList.Add(new Vector3(x, y, z));
            }
        }

        //Set up collage id
        for(int i = 0; i < collageFractionList.Count; ++i)
        {
            collageFractionList[i].collageId = i;
        }
    }

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
