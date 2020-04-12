/* 
 * Alejandro Cristo García
 * Himalaya Computing
 * Last modification: 14th March 2019
 * v1.0.0
 * Copyright 2019. All rights reserved.
 */

using UnityEngine;

public class CanvasDrawer : MonoBehaviour
{

    public GameObject panelH;
    public GameObject panelV;

    // Start is called before the first frame update
    void Awake()
    {
        if (Screen.width > Screen.height)
        {
            setLandscape();
        }
        else
        {
            setPortrait();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width > Screen.height && !panelH.activeSelf)
        {
            setLandscape();
        }
        if (Screen.width <= Screen.height && !panelV.activeSelf)
        {
            setPortrait();
        }
    }

    void setLandscape()
    {
        panelH.SetActive(true);
        panelV.SetActive(false);
    }

    void setPortrait()
    {
        panelH.SetActive(false);
        panelV.SetActive(true);
    }
}
