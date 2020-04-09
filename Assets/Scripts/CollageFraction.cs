using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollageFraction : MonoBehaviour
{
    //Collage
    [Header("Collage Information")]
    public static CollageOrganizer _Collage;
    public int collageId; //correct collage position id
    public int positionId; //current postion id

    //Move by left button
    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 currentPosition;

    void Awake()
    {
        if(_Collage == null)
        {
            _Collage = GameObject.Find("Collage").GetComponent<CollageOrganizer>();
            print(_Collage.gameObject.name);
        }
    }

    void Start()
    {
        StickToGrid();
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
            if (Vector3.Distance(this.transform.position, grid) < 0.3f)
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
        currentPosition = this.transform.position;
    }
    
    public void SetImageFromTexture2D(Texture2D tex2d)
    {
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(tex2d, new Rect(0, 0, GFractalArt.collageSize, GFractalArt.collageSize), new Vector2(0.5f, 0.5f));
    }
}
