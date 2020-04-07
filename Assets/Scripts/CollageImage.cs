using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollageImage : MonoBehaviour
{
    public SpriteRenderer PaintingSpriteRenderer;

    void AWake()
    {
        
    }

    void Start()
    {
        LoadSprite("imgs/1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSprite(string fileName)
    {
        print("Collage image load sprite");
        Texture2D texture = Resources.Load<Texture2D>(fileName);
        print(texture.width);
        print(texture.height);

        //cut the image into square size
        int newTextureSize = Mathf.Min(texture.width, texture.height);
        Texture2D destTex = new Texture2D(newTextureSize, newTextureSize);
        Color[] pix = texture.GetPixels(0, 0, texture.width, texture.width);
        destTex.SetPixels(pix);
        destTex.Apply();

        Texture2D resizedTex = ScaleTexture(destTex, 400, 400);

        PaintingSpriteRenderer.sprite = Sprite.Create(resizedTex, new Rect(0, 0, 400, 400), new Vector2(0.5f, 0.5f));
    }


    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }


}
