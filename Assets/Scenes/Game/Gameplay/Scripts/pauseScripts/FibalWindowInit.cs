using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FibalWindowInit : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite[] imgs;

    public void Init(bool winner)
    {
        if (winner)
        {
            img.sprite = imgs[0];
        }
        else
        {
            img.sprite = imgs[1];
        }
    }

    public void Quit()
    {
        SceneLoaderData.SceneToLoad = "";
        SceneManager.LoadScene("Load");
    }

    public void restart()
    {
        SceneManager.LoadScene("Load");
    }
}
