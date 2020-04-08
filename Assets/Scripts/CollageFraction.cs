using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollageFraction : MonoBehaviour
{

    //Move by left button
    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 currentPosition;

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
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
