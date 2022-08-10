using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI buttonLevelText;

    public GameObject firstLevelButton;


    void Start()
    {
        if (LevelManager.level == 0)
        {
            buttonLevelText.text = "LeveL" + 1;
        }
        else
        {
            buttonLevelText.text = "LeveL" + LevelManager.level;
        }


        if (GameManager.gameStarted)
        {
            Destroy(firstLevelButton);
        }
        if (LevelManager.level == 1 || LevelManager.level == 0)
        {
            Destroy(firstLevelButton);
        }
    }


    public void NextLevel()
    {
        GameManager.gameStarted = true;
        SceneManager.LoadScene(LevelManager.level);
    }

    public void GoLevelOne()
    {
        GameManager.gameStarted = true;
        LevelManager.level = 1;
        SceneManager.LoadScene(LevelManager.level);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}
