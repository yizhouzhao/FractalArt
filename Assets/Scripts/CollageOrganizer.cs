using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollageOrganizer : MonoBehaviour
{
    //collage pieces 
    [Header("Collage Pieces")]
    public List<CollageFraction> collageFractionList;
    public List<CollageFraction> candidateList;
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

    void Start()
    {
        //Random select collage to move to candidate positions
        ShuffleCollagePieces();
        
    }

    private void ShuffleCollagePieces()
    {
        //ReservoirSampling 
        //see more https://www.geeksforgeeks.org/reservoir-sampling/

        int n = collageFractionList.Count;
        int k = candidateList.Count;

        int i;
        List<int> reservoir = new List<int>();
        List<int> not_in_rervoir = new List<int>();
        for (i = 0; i < k; i++)
            reservoir.Add(i);

        for (; i < n; i++)
        {
            // Pick a random index from 0 to i. 
            int j = UnityEngine.Random.Range(0, i + 1);

            // If the randomly picked index  
            // is smaller than k, then replace  
            // the element present at the index 
            // with new element from stream 
            if (j < k)
                reservoir[j] = i;
        }

        //shuffle position
        for (i = 0; i < k; i++)
        {
            collageFractionList[reservoir[i]].transform.position = candidateList[i].transform.position;

            //!!!!!!!!!CAREFUL MAY CAUSE BUG HERE FOR GRID, candidate must not be at grid position
            collageFractionList[reservoir[i]].positionId = -1;

            //delete placeholder
            candidateList[i].gameObject.SetActive(false);
        }

        //interchange position
        for (i = 0; i < n; i++)
        {
            if (!reservoir.Contains(i))
            {
                not_in_rervoir.Add(i);
            }
        }

        //shuffle collage pieces 
        for(i = 0; i < n-k; i++)
        {
            Vector3 tempPosition = collageFractionList[not_in_rervoir[i]].transform.position;
            int randomIndex = UnityEngine.Random.Range(i, n-k);
            collageFractionList[not_in_rervoir[i]].transform.position = collageFractionList[not_in_rervoir[randomIndex]].transform.position;
            collageFractionList[not_in_rervoir[i]].StickToGrid();
            collageFractionList[not_in_rervoir[randomIndex]].transform.position = tempPosition;
            collageFractionList[not_in_rervoir[randomIndex]].StickToGrid();
        }

    }

    //Get current game score
    public int GetCollageScore()
    {
        int score = 0;
        foreach(CollageFraction collageFraction in collageFractionList)
        {
            if(collageFraction.positionId == collageFraction.collageId)
            {
                score++;
            }
        }
        return score;
    }

    //Set up pictures for collage pieces
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
