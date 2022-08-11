using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public GameObject touchButton;

    public static int level;

    private int hackLevel;

    public static int projectileCount;

    public static LevelManager Instance;

    public GameObject projectile;

    public GameObject projectileLighting;

    public GameObject obstacle;

    public ParticleSystem badHitEffect;

    public static bool levelWin;

    public static bool lastLevelWin;

    private AudioSource audio;

    public AudioClip throwSound;

    public AudioClip hitSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // GameObject buttonCanvas = Instantiate(touchButton, transform.position, touchButton.transform.rotation);
            // DontDestroyOnLoad(buttonCanvas);
            audio = GetComponent<AudioSource>();
            
            
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        GameManager.Instance.LoadLevel();

        if (GameManager.lastLevel > level)
        {
            level = GameManager.lastLevel;
        }
        else
        {
            level = 1;
        }

        if (level == 0)
        {
            level = 1;
            FindObjectOfType<MenuManager>().buttonLevelText.text = "LeveL" + level;
        }
        else
        {
            FindObjectOfType<MenuManager>().buttonLevelText.text = "LeveL" + level;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.Instance.SaveLevel(0);
        }

        GoLevelHack();
    }

    public void LevelWin()
    {
        level++;
        StartCoroutine(WaitWinScreen());
    }


    public void LevelLose()
    {
        StartCoroutine(WaitLoseScreen());
    }

    IEnumerator WaitWinScreen()
    {
        lastLevelWin = true;
        Destroy(FindObjectOfType<SpawnManager>().timeBar.transform.parent.gameObject);
        yield return new WaitForSecondsRealtime(2f);
        GoMenu();
        yield break;
    }
    IEnumerator WaitLoseScreen()
    {
        lastLevelWin = false;
        Destroy(FindObjectOfType<SpawnManager>().timeBar.transform.parent.gameObject);
        yield return new WaitForSecondsRealtime(2f);
        if (!levelWin)
        {
            GoMenu();
        }
        yield break;
    }

    public void StopSound()
    {
        audio.Stop();
    }

    public void PlayHitSound()
    {
        audio.clip = hitSound;
        audio.Play();
    }

    public void PlayThrowSound()
    {
        audio.clip = throwSound;
        audio.Play();
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }



    private void OnApplicationQuit()
    {
        if (level > GameManager.lastLevel)
        {
            GameManager.Instance.SaveLevel(level);
        }

    }

    void GoLevelHack()
    {

        if (Input.GetKeyDown(KeyCode.Alpha2) && hackLevel != 1 && hackLevel != 2)
        {
            hackLevel = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && hackLevel != 1)
        {
            hackLevel = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && hackLevel != 1)
        {
            hackLevel = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && hackLevel != 1)
        {
            hackLevel = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && hackLevel != 1)
        {
            hackLevel = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && hackLevel != 1)
        {
            hackLevel = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && hackLevel != 1)
        {
            hackLevel = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && hackLevel != 1)
        {
            hackLevel = 9;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) && hackLevel == 1)
        {
            hackLevel = 10;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && hackLevel == 1)
        {
            hackLevel = 11;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && hackLevel == 1)
        {
            hackLevel = 12;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && hackLevel == 1)
        {
            hackLevel = 13;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && hackLevel == 1)
        {
            hackLevel = 14;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && hackLevel == 1)
        {
            hackLevel = 15;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && hackLevel == 1)
        {
            hackLevel = 16;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && hackLevel == 1)
        {
            hackLevel = 17;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && hackLevel == 1)
        {
            hackLevel = 18;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && hackLevel == 1)
        {
            hackLevel = 19;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) && hackLevel == 2)
        {
            hackLevel = 20;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && hackLevel <= 9)
        {
            hackLevel = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && hackLevel != 0 && hackLevel != level)
        {
            level = hackLevel;
            hackLevel = 0;
            SceneManager.LoadScene(level);
        }


    }

}
