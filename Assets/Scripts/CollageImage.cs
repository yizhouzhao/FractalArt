using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollageImage : MonoBehaviour
{
    public SpriteRenderer PaintingSpriteRenderer;
    public Texture2D targetTexture; //the targeted texture with the puzzle image

    public CollageOrganizer collageOrg;

    void AWake()
    {
        
    }

    void Start()
    {
        //Load asset bundle for debug
        if(PlayerInfo.loadedAssetBundle == null)
        {
            string imageBundlePath = Application.persistentDataPath + "/raw_images";
            PlayerInfo.loadedAssetBundle = AssetBundle.LoadFromFile(imageBundlePath); //could be anything: server, path, etc.
            Debug.Log(PlayerInfo.loadedAssetBundle == null ? "Failed to load AssetBundle" : "Successfully loaded AssetBundle");
            PlayerInfo.questionTexture = PlayerInfo.loadedAssetBundle.LoadAsset<Texture2D>("question_mark");
        }

        LoadSprite();

        SetLevels();

        //Random select collage to move to candidate positions
        collageOrg.LoadLevel(collageOrg.currentLevelInfo);
        collageOrg.SaveLevel();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSprite()
    {
        print("Collage image load sprite");
        Texture2D texture; //= Resources.Load<Texture2D>(fileName);
        //print(texture.width);
        //print(texture.height);
        texture = PlayerInfo.loadedAssetBundle.LoadAsset<Texture2D>(PlayerInfo.pictureIndex.ToString());
        //Sprite pictureSprite = prefab.game

        //cut the image into square size
        int newTextureSize = Mathf.Min(texture.width, texture.height);
        Texture2D destTex = new Texture2D(newTextureSize, newTextureSize);
        Color[] pix = texture.GetPixels(0, 0, newTextureSize, newTextureSize);
        destTex.SetPixels(pix);
        destTex.Apply();

        targetTexture= ScaleTexture(destTex, GFractalArt.puzzleImageSize, GFractalArt.puzzleImageSize);
        PaintingSpriteRenderer.sprite = Sprite.Create(targetTexture, new Rect(0, 0, GFractalArt.puzzleImageSize, GFractalArt.puzzleImageSize), new Vector2(0.5f, 0.5f));
    }

    private void SetLevels()
    {
        int randomCollageIndex;

        //sent info to collage organizing
        LevelCollageInfo initLevel = new LevelCollageInfo();
        initLevel.levelId = 0;
        initLevel.levelTexture = targetTexture;
        initLevel.levelCollageSize = targetTexture.width / 4;

        collageOrg.currentLevelId = 0;
        collageOrg.currentLevelInfo = initLevel;

        //generate second level
        LevelCollageInfo secondLevel = new LevelCollageInfo();
        secondLevel.levelId = 1;
        randomCollageIndex = UnityEngine.Random.Range(0, 16);
        secondLevel.levelTexture = collageOrg.GetTexture2DForCollage(initLevel, randomCollageIndex);
        secondLevel.levelCollageSize = targetTexture.width / 4 / 4;

        //link parent
        secondLevel.parentLevelInfo = initLevel;
        initLevel.childrenLevelInfo.Add(secondLevel);
        initLevel.childrenLevelCollageIndexes.Add(randomCollageIndex);

        //generate third level
        LevelCollageInfo thirdLevel = new LevelCollageInfo();
        thirdLevel.levelId = 2;
        randomCollageIndex = UnityEngine.Random.Range(0, 16);
        thirdLevel.levelTexture = collageOrg.GetTexture2DForCollage(secondLevel, randomCollageIndex);
        thirdLevel.levelCollageSize = targetTexture.width / 4 / 4 / 4;

        //link parent
        thirdLevel.parentLevelInfo = secondLevel;
        secondLevel.childrenLevelInfo.Add(thirdLevel);
        secondLevel.childrenLevelCollageIndexes.Add(randomCollageIndex);
    }


    public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
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
