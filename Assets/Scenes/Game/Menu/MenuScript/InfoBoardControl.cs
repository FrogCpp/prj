using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoBoardControl : MonoBehaviour
{
    private TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();

        text.text = "Вы — подводный психотерапевт, готовый погрузиться в пучины сознания морских обитателей.\r\nКаждая новая рыбка — новая история, спрятанная среди кораллов страхов и водорослей воспоминаний.";
    }

    public void Press(GameObject btn)
    {
        switch (btn.name)
        {
            case "LevelSprite (0)":
                text.text = "ВСТАВИТЬ ОПИСАНИЕ! 1";
                break;
            case "LevelSprite (1)":
                text.text = "ВСТАВИТЬ ОПИСАНИЕ! 2";
                break;
            case "LevelSprite (2)":
                text.text = "ВСТАВИТЬ ОПИСАНИЕ! 3";
                break;
            case "LevelSprite (3)":
                text.text = "ВСТАВИТЬ ОПИСАНИЕ! 4";
                break;
            case "LevelSprite (4)":
                text.text = "ВСТАВИТЬ ОПИСАНИЕ! 5";
                break;
        }
    }


    public void LoadScene(GameObject btn)
    {
        switch (btn.name)
        {
            case "1":
                SceneLoaderData.SceneToLoad = "Level1";
                break;
            case "2":
                SceneLoaderData.SceneToLoad = "Level2";
                break;
            case "3":
                SceneLoaderData.SceneToLoad = "Level3";
                break;
            case "4":
                SceneLoaderData.SceneToLoad = "Level4";
                break;
            case "5":
                SceneLoaderData.SceneToLoad = "Level5";
                break;
        }

        SceneManager.LoadScene("Load");
    }
}
