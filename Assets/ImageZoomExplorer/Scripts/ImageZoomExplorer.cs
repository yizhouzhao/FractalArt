/* 
 * Alejandro Cristo García
 * Himalaya Computing
 * Last modification: 14th March 2019
 * v1.0.0
 * Copyright 2019. All rights reserved.
 */

using UnityEngine;
using UnityEngine.UI;

public class ImageZoomExplorer : MonoBehaviour
{
    public float zoomSpeed = 1.01f;
    public float zoomLimMax = 10f;
    public float zoomLimMin = 1f;
    public bool allowDoubleClickToZoom = true;
    public Color32 backgroundColor = new Color32(0, 0, 0, 255);
    public Sprite closeButtonImage;

    Vector2 center;
    Vector2 centerPrev;

    float touchDistance;
    float touchDistancePrev;

    bool padding;
    bool zoom;
    bool touchZoom;
    bool action;
    float dXAcc;
    float dYAcc;
    float zoomAcc;
    float zoomOr;
    float zoomDest;
    float animTime;
    bool anim;
    int prevTouchCount;


    Vector2 imagePos;
    Vector2 imageSize;
    Vector2 imageSizeOr;

    float lastTimeClick;

    bool landscape;
    bool landscapeImage;

    bool isOpen;

    RectTransform imageRectTransform;
    Image image;
    GameObject mainPanel;
    GameObject closeButton;

    Image mainPanelImage;


    // Start is called before the first frame update
    void Awake()
    {
        landscape = Screen.width > Screen.height;
        isOpen = false;

        imageRectTransform = transform.Find("mainPanel/Image").GetComponent<RectTransform>();
        image = transform.Find("mainPanel/Image").GetComponent<Image>();
        mainPanel = transform.Find("mainPanel").gameObject;
        closeButton = transform.Find("closeButtonPanel").gameObject;

        mainPanelImage = mainPanel.GetComponent<Image>();
        closeButton.transform.Find("closeButtonH").GetComponent<Image>().sprite = closeButtonImage;
        closeButton.transform.Find("closeButtonV").GetComponent<Image>().sprite = closeButtonImage;

        ResetImageExplorer();
    }

    // Update is called once per frame
    void Update()
    {

        float d = 0f;

        if (Screen.width > Screen.height && !landscape)
        {
            landscape = true;
            ResetImageExplorer();
            return;
        }
        if (Screen.height > Screen.width && landscape)
        {
            landscape = false;
            ResetImageExplorer();
            return;
        }

        padding = false;
        zoom = false;
        touchZoom = false;

        if (anim)
        {
            d = (Time.time - animTime) / 0.25f;
            zoomAcc = Mathf.Lerp(zoomOr, zoomDest, d);
            if (d >= 1f)
            {
                zoomAcc = zoomDest;
                anim = false;
            }
            zoom = true;
            action = true;
        }
        else
        {

            if (allowDoubleClickToZoom)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    d = Time.time - lastTimeClick;
                    if (d < 0.25f)
                    {
                        if (zoomAcc > 1f)
                        {
                            zoomOr = zoomAcc;
                            zoomDest = 1f;
                        }
                        else
                        {
                            zoomOr = 1f;
                            zoomDest = 4f;
                        }
                        animTime = Time.time;
                        anim = true;
                        center = Input.mousePosition;
                        center.y = Screen.height - center.y;
                    }
                    else
                    {
                        lastTimeClick = Time.time;
                    }
                }
            }

# if UNITY_STANDALONE

            if (Input.GetMouseButton(0))
            {
                padding = true;
                center = Input.mousePosition;
                center.y = Screen.height - center.y;
                if (prevTouchCount != 1)
                {
                    action = false;
                }
                prevTouchCount = 1;
            }

#else

            if (Input.touchCount == 1)
            {
                padding = true;
                center = Input.touches[0].position;
                center.y = Screen.height - center.y;
                if (prevTouchCount != 1)
                {
                    action = false;
                }
                prevTouchCount = 1;
            }

#endif

            if (Input.touchCount == 2)
            {
                touchZoom = true;
                center = new Vector2((Input.touches[0].position.x + Input.touches[1].position.x) / 2f, (Input.touches[0].position.y + Input.touches[1].position.y) / 2f);
                center.y = Screen.height - center.y;
                touchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                if (prevTouchCount != 2)
                {
                    action = false;
                }
                prevTouchCount = 2;
            }

            if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
            {
                zoom = true;
                center = Input.mousePosition;
                center.y = Screen.height - center.y;
                zoomAcc += zoomSpeed;
            }

            if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            {
                zoom = true;
                center = Input.mousePosition;
                center.y = Screen.height - center.y;
                zoomAcc -= zoomSpeed;
            }
        }

        if (padding || zoom || touchZoom)
        {

            if (action)
            {
                if (zoom)
                {
                    // Zoom

                    ApplyZoom();
                }

                if (touchZoom)
                {
                    zoomAcc += (touchDistance - touchDistancePrev) / (Screen.width * Screen.height) * 10000f;
                    ApplyZoom();
                }


                // Drag

                dXAcc = centerPrev.x - center.x;
                dYAcc = centerPrev.y - center.y;

                ApplyPan();

                PlaceImage();

            }

            centerPrev = center;
            touchDistancePrev = touchDistance;
            action = true;
        }
        else
        {

            if (Mathf.Abs(dXAcc) > 0f)
            {
                dXAcc *= 0.9f;
                if (Mathf.Abs(dXAcc) < 0.001f)
                {
                    dXAcc = 0f;
                }
            }
            if (Mathf.Abs(dYAcc) > 0f)
            {
                dYAcc *= 0.9f;
                if (Mathf.Abs(dYAcc) < 0.001f)
                {
                    dYAcc = 0f;
                }
            }

            ApplyPan();

            PlaceImage();

            action = false;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            CloseImageExplorer();
        }

        if (backgroundColor != mainPanelImage.color)
        {
            mainPanelImage.color = backgroundColor;
        }

    }

    void ApplyZoom()
    {
        float fX = 0f, fY = 0f, dX = 0f, dY = 0f;
        if (center.x < imagePos.x)
        {
            center.x = imagePos.x;
        }
        else
        {
            if (center.x > imagePos.x + imageSize.x)
            {
                center.x = imagePos.x + imageSize.x;
            }
        }
        fX = (center.x - imagePos.x) / imageSize.x;
        if (center.y < imagePos.y)
        {
            center.y = imagePos.y;
        }
        else
        {
            if (center.y > imagePos.y + imageSize.y)
            {
                center.y = imagePos.y + imageSize.y;
            }
        }
        fY = (center.y - imagePos.y) / imageSize.y;


        if (!anim)
        {
            if (zoomAcc > zoomLimMax)
            {
                zoomAcc = zoomLimMax;
            }
            if (zoomAcc < zoomLimMin)
            {
                zoomAcc = zoomLimMin;
            }
        }

        imageSize = new Vector2(imageSizeOr.x * zoomAcc, imageSizeOr.y * zoomAcc);
        dX = imagePos.x + imageSize.x * fX;
        dY = imagePos.y + imageSize.y * fY;
        imagePos = new Vector2(imagePos.x - (dX - center.x), imagePos.y - (dY - center.y));
    }

    void ApplyPan()
    {
        if (imageSize.x >= Screen.width)
        {

            imagePos.x = imagePos.x - dXAcc;
            if (imagePos.x > 0)
            {
                imagePos.x = 0;
            }
            if (imagePos.x + imageSize.x < Screen.width)
            {
                imagePos.x = imagePos.x - (imagePos.x + imageSize.x - Screen.width);
            }
        }
        else
        {
            imagePos.x = Screen.width * 0.5f - imageSize.x * 0.5f;
        }
        if (imageSize.y >= Screen.height)
        {

            imagePos.y = imagePos.y - dYAcc;
            if (imagePos.y > 0)
            {
                imagePos.y = 0;
            }
            if (imagePos.y + imageSize.y < Screen.height)
            {
                imagePos.y = imagePos.y - (imagePos.y + imageSize.y - Screen.height);
            }
        }
        else
        {
            imagePos.y = Screen.height * 0.5f - imageSize.y * 0.5f;
        }
    }

    public void ResetImageExplorer()
    {
        float f, w, h;

        w = Screen.width;
        f = w / (float)image.mainTexture.width;
        h = image.mainTexture.height * f;
        if (h > Screen.height)
        {
            h = Screen.height;
            f = h / (float)image.mainTexture.height;
            w = image.mainTexture.width * f;
            landscapeImage = false;
        }
        else
        {
            landscapeImage = true;
        }

        imageSize = new Vector2(w, h);
        imagePos = landscapeImage ? new Vector2(0, Screen.height * 0.5f - imageSize.y * 0.5f) : new Vector2(Screen.width * 0.5f - imageSize.x * 0.5f, 0);

        padding = false;
        zoom = false;
        touchZoom = false;
        action = false;
        zoomAcc = 1f;
        anim = false;
        prevTouchCount = 0;

        imageSizeOr = imageSize;

        lastTimeClick = -1;

        center = new Vector2(-1f, -1f);
        touchDistance = 0f;

        PlaceImage();
    }

    void PlaceImage()
    {
        imageRectTransform.anchoredPosition = new Vector3(imagePos.x, -imagePos.y, 0f);
        imageRectTransform.sizeDelta = new Vector2(imageSize.x, imageSize.y);
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public void OpenImageExplorer(Image image)
    {
        this.image.sprite = image.sprite;
        ResetImageExplorer();
        mainPanel.SetActive(true);
        closeButton.SetActive(true);
        isOpen = true;
    }

    public void OpenImageExplorer(Sprite sprite)
    {
        image.sprite = sprite;
        ResetImageExplorer();
        mainPanel.SetActive(true);
        closeButton.SetActive(true);
        isOpen = true;
    }

    public void CloseImageExplorer()
    {
        mainPanel.SetActive(false);
        closeButton.SetActive(false);
        isOpen = false;
    }
}
