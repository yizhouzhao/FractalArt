using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{

    //double tap
    public static bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 0.6f;
        float VariancePosition = 1.0f;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            if (hit)
            {
                Debug.Log("hit something: " + hit.collider.gameObject.name);
                //OR with Tag
                if (hit.collider.gameObject.tag == "pintu")
                {
                    CollageFraction lastSelectedFraction = CollageFraction.selectedCollageFraction;
                    CollageFraction.selectedCollageFraction = hit.collider.gameObject.GetComponent<CollageFraction>();
                    Debug.Log("some pintu clicked: " + CollageFraction.selectedCollageFraction.collageId);

                    if(lastSelectedFraction == CollageFraction.selectedCollageFraction)
                    {
                        float DeltaTime = Input.GetTouch(0).deltaTime;
                        float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

                        if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                            result = true;
                    }
                }
            }
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDoubleTap())
        {
            if (CollageFraction.selectedCollageFraction.canEnterNextLevel)
            {
                int nextLevelIndex = CollageFraction._Collage.currentLevelInfo.childrenLevelCollageIndexes.IndexOf(CollageFraction.selectedCollageFraction.collageId);
                //Debug.Log("nextLevelIndex " + nextLevelIndex);
                LevelCollageInfo nextLevel = CollageFraction._Collage.currentLevelInfo.childrenLevelInfo[nextLevelIndex];
                CollageFraction._Collage.LoadNextLevel(nextLevel);
            }
        }
    }
}
