using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelCollageInfo
{
    public int levelId;
    public List<CollageFractionInfo> collageFractionInfoList = new List<CollageFractionInfo>();
    public Texture2D levelTexture;
    public int levelCollageSize;

    public bool visited = false;

    //Tree structure To NEXT LEVEL
    public LevelCollageInfo parentLevelInfo;
    public List<LevelCollageInfo> childrenLevelInfo = new List<LevelCollageInfo>();
    public List<int> childrenLevelCollageIndexes = new List<int>();

    public Texture2D GetSketch()
    {
        //Debug.Log("here" + collageFractionInfoList.Count + " " + levelTexture.height + " " + levelTexture.width);
        Texture2D sketch = new Texture2D(levelTexture.width, levelTexture.height, levelTexture.format, true);
        foreach(CollageFractionInfo collage in collageFractionInfoList)
        {
            int nextIndex = childrenLevelCollageIndexes.IndexOf(collage.collageId);
            int positionId = collage.positionId;
            if(positionId != -1)
            {
                //Debug.Log("here has some ids");
                int xOffset = positionId % 4;
                int yOffset = positionId / 4;
                Texture2D tex = collage.currentTexture2d;
                if (nextIndex != -1)
                {
                    LevelCollageInfo childLevel = childrenLevelInfo[nextIndex];
                    if (!childLevel.visited)
                    {
                        tex = CollageImage.ScaleTexture(PlayerInfo.questionTexture, tex.width, tex.height);
                    }
                    else
                    {
                        tex = CollageImage.ScaleTexture(childLevel.GetSketch(), tex.width, tex.height);
                    }
                }
                //send picture to sketches
                
                for (int i = 0; i < tex.width; i++)
                {
                    for (int j = 0; j < tex.height; j++)
                    {
                        Color color = tex.GetPixel(i, j);
                        sketch.SetPixel(xOffset * levelCollageSize + i, yOffset * levelCollageSize + j, color);
                    }
                }
            }
        }
        sketch.Apply();
        return sketch;
    }
}

public class CollageOrganizer : MonoBehaviour
{
    //collage pieces 
    [Header("Gameboard")]
    public GameBoard gameBoard;

    [Header("Collage Pieces")]
    public List<CollageFraction> collageFractionList;
    public List<CollageFraction> candidateList;
    public List<Vector3> gridPointList;

    //[Header("Picture")]
    //public Texture2D currentTexture;
    //public int currentTextureSize;

    [Header("Level")]
    //public List<LevelCollageInfo> levelInfoList = new List<LevelCollageInfo>();
    public int currentLevelId;
    public LevelCollageInfo currentLevelInfo;

    void Awake()
    {

        //init grid positions
        gridPointList = new List<Vector3>();

        float boardLeft = GFractalArt.boardCenter.x - GFractalArt.boardWidth / 2;
        float boardRight = GFractalArt.boardCenter.x + GFractalArt.boardWidth / 2;

        float boardTop = GFractalArt.boardCenter.y + GFractalArt.boardHeight / 2;
        float boardBottom = GFractalArt.boardCenter.y - GFractalArt.boardHeight / 2;

        float step = GFractalArt.boardWidth / gameBoard.gridCountPerLine;
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

    //set piece to grid one by one
    public void SequentiallySetPieceToGrid()
    {
        for(int i = 0; i < collageFractionList.Count; ++i)
        {
            collageFractionList[i].gameObject.transform.position = gridPointList[i];
            collageFractionList[i].StickToGrid();
        }
    }

    void Start()
    {
        
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

        //To do
        return score;
    }

    //Set up pictures for collage pieces
    public void SetCollagePieces(LevelCollageInfo levelInfo)
    {
        for(int i = 0; i < collageFractionList.Count; i++)
        {
            Texture2D cTex = GetTexture2DForCollage(levelInfo, i);
            collageFractionList[i].currentTexture2d = cTex;
            if (currentLevelInfo.childrenLevelCollageIndexes.Contains(i))
            {
                LevelCollageInfo child = currentLevelInfo.childrenLevelInfo[currentLevelInfo.childrenLevelCollageIndexes.IndexOf(i)];
                if (!child.visited)
                {
                    collageFractionList[i].canEnterNextLevel = true;
                    collageFractionList[i].SetImageFromTexture2D(PlayerInfo.questionTexture);
                }
                else
                {
                    Texture2D sketch = child.GetSketch();
                    collageFractionList[i].SetImageFromTexture2D(sketch);
                }
            }
            else
            {      
                collageFractionList[i].SetImageFromTexture2D(cTex);
            }

        }
    }

    public Texture2D GetTexture2DForCollage(LevelCollageInfo levelInfo, int collageIndex)
    {
        Texture2D currentTexture = levelInfo.levelTexture;
        int collageTempSize = levelInfo.levelCollageSize;
        Texture2D collageTex =  new Texture2D(collageTempSize, collageTempSize, currentTexture.format, true);
        int xOffset = collageIndex % gameBoard.gridCountPerLine;
        int yOffset = collageIndex / gameBoard.gridCountPerLine;
        for (int i = 0; i < collageTempSize; i++)
        {
            for (int j = 0; j < collageTempSize; ++j)
            {
                Color color = currentTexture.GetPixel(xOffset * collageTempSize + i, yOffset * collageTempSize + j);
                collageTex.SetPixel(i, j, color);
            }
        }
        collageTex.Apply();

        return collageTex;
    }

    public void LoadLevel(LevelCollageInfo levelInfo)
    {
        currentLevelInfo = levelInfo;
        currentLevelInfo.visited = true;
        //Debug.Log("Collage ORG Load level: " + levelIndex.ToString() + " Count: " + levelInfoList[currentLevelId].collageFractionInfoList.Count);
        if (levelInfo.collageFractionInfoList.Count == 0)
        {
            //see this level for the first time for empty list generate pieces and shuffle
            SetCollagePieces(levelInfo);
            SequentiallySetPieceToGrid();
            ShuffleCollagePieces();

        }
        else
        {
            for(int i = 0; i < collageFractionList.Count; ++i)
            {
                collageFractionList[i].SetTextureAndPositionFromInfo(levelInfo.collageFractionInfoList[i]);
                if (currentLevelInfo.childrenLevelCollageIndexes.Contains(i))
                {
                    LevelCollageInfo child = currentLevelInfo.childrenLevelInfo[currentLevelInfo.childrenLevelCollageIndexes.IndexOf(i)];
                    if (!child.visited)
                    {
                        collageFractionList[i].SetImageFromTexture2D(PlayerInfo.questionTexture);
                    }
                    else
                    {
                        Texture2D sketch = child.GetSketch();
                        collageFractionList[i].SetImageFromTexture2D(sketch);
                    }
                }
            }
        }
    }

    public void SaveLevel()
    {
        //Debug.Log("Collage ORG save level: " + currentLevelId.ToString());

        if (currentLevelInfo.collageFractionInfoList.Count == 0)
        {
            foreach (CollageFraction cFraction in collageFractionList)
            {
                currentLevelInfo.collageFractionInfoList.Add(cFraction.GetFractionInfo());
            }
        }
        else
        {
            for (int i = 0; i < collageFractionList.Count; ++i)
            {
                currentLevelInfo.collageFractionInfoList[i] = collageFractionList[i].GetFractionInfo();
            }
        }
    }

    public void LoadNextLevel(LevelCollageInfo levelInfo)
    {
        SaveLevel();
        LoadLevel(levelInfo);
    }

    public void ToLastLevel()
    {
        SaveLevel();
        if(currentLevelInfo.parentLevelInfo != null)
        {
            LoadLevel(currentLevelInfo.parentLevelInfo);
        }
    }
}
