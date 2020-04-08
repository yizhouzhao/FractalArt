using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PictureSelection : MonoBehaviour
{
    private GameObject[] pictureList;
    private int index;


    private void Start()
    {
        index = PlayerPrefs.GetInt("pictureSelected");

        pictureList = new GameObject[transform.childCount];

        //Fill the array with our models
        for (int i = 0; i < transform.childCount; i++)
        {
            pictureList[i] = transform.GetChild(i).gameObject;
        }
        //Toggle off their renderer
        foreach (GameObject go in pictureList)
        {
            go.SetActive(false);
        }

        //Toggle on the selected picture
        if (pictureList[index])
        {
            pictureList[index].SetActive(true);
        }
        //index = PlayerPrefs.GetInt("PictureSelected");
    }

    public void ToggleLeft()
    {

        //Toggle off the current model
        pictureList[index].SetActive(false);
        index--;
        if (index < 0)
            index = pictureList.Length - 1;

        //Toggle on the new model
        pictureList[index].SetActive(true);

    }

    public void ToggleRight()
    {

        //Toggle off the current model
        pictureList[index].SetActive(false);
        index++;
        if (index == pictureList.Length)
            index = 0;

        //Toggle on the new model
        pictureList[index].SetActive(true);
    }

    public void toGameScene()
    {
        PlayerPrefs.SetInt("pictureSelected", index);

        SceneManager.LoadScene("GameScene2222");
    }


}
