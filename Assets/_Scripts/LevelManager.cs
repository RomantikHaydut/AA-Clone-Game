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

    public bool levelWin;

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
        if (GameManager.lastLevel > level)
        {
            level = GameManager.lastLevel;
        }
        else
        {
            level = 1;
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
        yield return new WaitForSecondsRealtime(1.5f);
        FindObjectOfType<SpawnManager>().gameWinPanel.SetActive(true);
        FindObjectOfType<SpawnManager>().textBackground.SetActive(true);
        yield break;
    }
    IEnumerator WaitLoseScreen()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (!levelWin)
        {
            FindObjectOfType<SpawnManager>().gameOverPanel.SetActive(true);
            FindObjectOfType<SpawnManager>().textBackground.SetActive(true);
        }
        yield break;
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

        if (Input.GetKeyDown(KeyCode.Alpha2) && hackLevel != 1)
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
