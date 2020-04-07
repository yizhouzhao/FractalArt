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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
