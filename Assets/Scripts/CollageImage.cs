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
        var texture = Resources.Load<Texture2D>(fileName);
        print(texture.width);
        print(texture.height);
        PaintingSpriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    } 


}
