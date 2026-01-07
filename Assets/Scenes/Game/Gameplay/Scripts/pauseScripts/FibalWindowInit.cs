using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FibalWindowInit : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;

    public void Init(bool winner)
    {
        if (winner)
        {
            txt.text = "Побеееда!";
        }
        else
        {
            txt.text = "Порожение.";
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
