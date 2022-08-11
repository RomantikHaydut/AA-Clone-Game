using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI buttonLevelText;

    public TextMeshProUGUI failSuccessText;

    public GameObject firstLevelButton;


    void Start()
    {
        if (LevelManager.level == 0)
        {
            LevelManager.level = 1;
            buttonLevelText.text = "LeveL" + LevelManager.level;
        }
        else
        {
            buttonLevelText.text = "LeveL" + LevelManager.level;
        }

        failSuccessText.gameObject.SetActive(true);
        if (LevelManager.lastLevelWin)
        {
            failSuccessText.text = "Success :)";
        }
        else
        {
            failSuccessText.text = "Fail !!!";
        }

        if (GameManager.gameStarted)
        {
            Destroy(firstLevelButton);
            GameObject button = buttonLevelText.transform.parent.gameObject;
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            failSuccessText.gameObject.SetActive(false);
        }
        if (LevelManager.level == 1 || LevelManager.level == 0)
        {
            Destroy(firstLevelButton);
            GameObject button = buttonLevelText.transform.parent.gameObject;
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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
