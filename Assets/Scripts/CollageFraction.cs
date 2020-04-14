using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollageFractionInfo
{
    public int collageId;
    public int positionId;
    public Vector3 currentPosition;
    public Texture2D currentTexture2d;
}

public class CollageFraction : MonoBehaviour
{
    //Collage
    [Header("Collage Information")]
    public static CollageOrganizer _Collage;
    public int collageId; //correct collage position id
    public int positionId; //current postion id
    public Texture2D currentTexture2d;

    [Header("Level Information")]
    public bool canEnterNextLevel = false;

    //Move by left button
    private Vector3 mOffset;
    private float mZCoord;

    [Header("Debug only")]
    public bool debug;
    private bool isRightSelected = false;

    //Get the summary information
    public CollageFractionInfo GetFractionInfo()
    {
        CollageFractionInfo cInfo = new CollageFractionInfo();
        cInfo.collageId = collageId;
        cInfo.positionId = positionId;
        cInfo.currentPosition = this.transform.position;
        cInfo.currentTexture2d = currentTexture2d;
        return cInfo;
    }

    public void SetTextureAndPositionFromInfo(CollageFractionInfo cInfo)
    {
        this.collageId = cInfo.collageId;
        this.positionId = cInfo.positionId;
        this.transform.position = cInfo.currentPosition;
        this.currentTexture2d = cInfo.currentTexture2d;
        SetImageFromTexture2D(currentTexture2d);
    }



    void Awake()
    {
        if(_Collage == null)
        {
            _Collage = GameObject.Find("Collage").GetComponent<CollageOrganizer>();
            //print(_Collage.gameObject.name);
        }
    }

    void Start()
    {
        StickToGrid();

        if (debug)
        {
            StartCoroutine(DelayStart());
        }
    }

    void Update()
    {

        //Debug right click
        if (isRightSelected)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (canEnterNextLevel)
                {
                    isRightSelected = false;
                    //Debug.Log("Pressed right click enter next level." + _Collage.currentLevelInfo.levelId);
                    //Debug.Log(_Collage.currentLevelInfo.childrenLevelCollageIndexes);
                    //Debug.Log(collageId);
                    int nextLevelIndex = _Collage.currentLevelInfo.childrenLevelCollageIndexes.IndexOf(collageId);
                    //Debug.Log("nextLevelIndex " + nextLevelIndex);
                    LevelCollageInfo nextLevel = _Collage.currentLevelInfo.childrenLevelInfo[nextLevelIndex];
                    _Collage.LoadNextLevel(nextLevel);
                }
            }
        }
    }

    IEnumerator DelayStart()
    {
       
        yield return new WaitForSeconds(1f);
        //Debug.Log("DDDDDDDEBUG");
        //For debug use
        Texture2D debugTex = _Collage.currentLevelInfo.GetSketch();

        SetImageFromTexture2D(CollageImage.ScaleTexture(debugTex, _Collage.gameBoard.collageSize, _Collage.gameBoard.collageSize));
        yield return null;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseUp()
    {
        StickToGrid();
        //Stick to grid
    }

    public void StickToGrid()
    {
        bool sticked = false;
        for (int i = 0; i < _Collage.gridPointList.Count; ++i)
        {
            Vector3 grid = _Collage.gridPointList[i];
            if (Vector3.Distance(this.transform.position, grid) < 0.32f)
            {
                this.transform.position = grid;
                this.positionId = i;
                sticked = true;
            }
        }
        if (!sticked)
        {
            this.positionId = -1;
        }
    }

    private void OnMouseDrag()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}
        transform.position = GetMouseWorldPos() + mOffset;


        //this.transform.position = GetGridPosition(transform.position);
    }

    private void OnMouseOver()
    {
        //print("on mouse over");
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();

        if (Input.GetMouseButtonDown(1))
        {
            isRightSelected = true;
        }
    }
    
    public void SetImageFromTexture2D(Texture2D tex2d)
    {
        int collageSize = _Collage.gameBoard.collageSize;
        Texture2D scaledTex = CollageImage.ScaleTexture(tex2d, collageSize, collageSize);
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(scaledTex, new Rect(0, 0, collageSize, collageSize), new Vector2(0.5f, 0.5f));
    }
}
