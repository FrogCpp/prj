using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
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
