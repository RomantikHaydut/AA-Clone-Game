using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public Light light;

    private GameObject projectile;

    private GameObject obstacle;

    private GameObject center;

    public static bool canShoot;

    private Vector3 spawnPosition;

    public static float projectileSpeed;

    private float duration;

    private float timeLimit = 17f;

    public static int sendedProjectile;

    public GameObject textBackground;

    TextMeshProUGUI projectileText;

    TextMeshProUGUI levelText;

    TextMeshProUGUI dangerText;

    TextMeshProUGUI timeText;

    private CenterController centerController;

    private LevelManager levelManager;

    public GameObject gameWinPanel;

    public GameObject gameOverPanel;

    public bool reverserLevel;


    void Start()
    {

        projectileSpeed = Random.Range(1.3f, 2.5f);

        levelManager = FindObjectOfType<LevelManager>();

        levelManager.levelWin = false;

        obstacle = levelManager.obstacle;

        centerController = FindObjectOfType<CenterController>();

        sendedProjectile = 0;

        center = GameObject.FindGameObjectWithTag("Center");

        Vector3 direction = (center.transform.position - transform.position).normalized;

        spawnPosition = transform.position + direction / 2;

        LevelManager.projectileCount = ProjectileCountForLevel();

        projectileText = GameObject.Find("Projectile Text").GetComponent<TextMeshProUGUI>();

        projectileText.text = (LevelManager.projectileCount - sendedProjectile).ToString() + " Arrows Left";

        levelText = GameObject.FindGameObjectWithTag("Level Text").GetComponent<TextMeshProUGUI>();

        timeText = GameObject.Find("Time Text").GetComponent<TextMeshProUGUI>();

        levelText.text = LevelManager.level.ToString();

        if (LevelManager.level >= 10)
        {
            levelText.rectTransform.rect.Set(3,0,44,44);
        }

        dangerText = GameObject.Find("Danger Text").GetComponent<TextMeshProUGUI>();

        dangerText.gameObject.SetActive(false);

        textBackground = GameObject.FindGameObjectWithTag("Purple Background");

        textBackground.SetActive(false);

        timeText.gameObject.SetActive(false);

        RotateSpeedForLevel();


        gameWinPanel = GameObject.FindGameObjectWithTag("Game Win Panel");
        gameWinPanel.SetActive(false);
        gameOverPanel = GameObject.FindGameObjectWithTag("Game Over Panel");
        gameOverPanel.SetActive(false);

        GameManager.gameOver = false;

        canShoot = true;

        reverserLevel = false;

        if (LevelManager.level > 4)
        {
            if (LevelManager.level / 3 <= 5)
            {
                SpawnObstacles(obstacle, (LevelManager.level / 3) + 1);
            }
            else
            {
                SpawnObstacles(obstacle, 5);
            }
        }

        if (LevelManager.level % 5 == 0 && LevelManager.level % 3 != 0 && LevelManager.level > 4)
        {
            reverserLevel = true;
            StartCoroutine(OpenDangerText());
            dangerText.text = "Speed Will Change When You Fire ";
        }

        if (LevelManager.level % 4 == 0 && LevelManager.level%12 != 0 && LevelManager.level > 3)
        {
            StartCoroutine(OpenDangerText());
            StartCoroutine(AutoChangingSpeed());

        }


        if (LevelManager.level %3 == 0 && LevelManager.level > 2)
        {
            StartCoroutine(TimeLimitLevel());
        }

        if (LevelManager.level % 7 == 0 && LevelManager.level > 6)
        {
            StartCoroutine(AutoReversingSpeed());
        }



        if (LevelManager.level == 16) // Dark Level
        {
            projectile = levelManager.projectileLighting;
            light = GameObject.Find("Directional Light").GetComponent<Light>();
            StartCoroutine(AutoLight());

        }
        else if (LevelManager.level == 4)
        {
            projectile = levelManager.projectileLighting;
        }
        else // other levels
        {
            projectile = levelManager.projectile;
        }



        Time.timeScale = 1f;

    }


    void Update()
    {
        if (SimpleInput.GetMouseButtonDown(0) && !GameManager.gameOver && canShoot && !reverserLevel)
        {
            SpawnProjectile(projectile);
            sendedProjectile++;
            projectileText.text = (LevelManager.projectileCount - sendedProjectile).ToString() + " Arrows Left";
            if (LevelManager.projectileCount - sendedProjectile == 0)
            {
                Destroy(projectileText.gameObject);
            }
            else if (LevelManager.projectileCount - sendedProjectile == 1)
            {
                projectileText.text = "1 Arrow Left";
            }
            canShoot = false;

        }
        else if (SimpleInput.GetMouseButtonDown(0) && !GameManager.gameOver && canShoot && reverserLevel)
        {
            SpawnReverserProjectile(projectile);
            sendedProjectile++;
            projectileText.text = (LevelManager.projectileCount - sendedProjectile).ToString() + " Arrows Left";
            if (LevelManager.projectileCount - sendedProjectile == 0)
            {
                Destroy(projectileText.gameObject);
            }
            else if (LevelManager.projectileCount - sendedProjectile == 1)
            {
                projectileText.text = "1 Arrow Left";
            }
            canShoot = false;
        }

    }

    void SpawnProjectile(GameObject selectedProjectile)
    {
        if (LevelManager.level != 16)
        {
            Instantiate(selectedProjectile, spawnPosition, selectedProjectile.transform.rotation);

        }
        else
        {
           GameObject lightArrow = Instantiate(selectedProjectile, spawnPosition, selectedProjectile.transform.rotation);
          // Destroy(lightArrow.transform.GetComponentInChildren(typeof(Light)).gameObject , 2f);
        }
    }

    void SpawnReverserProjectile(GameObject selectedProjectile)
    {
        Instantiate(selectedProjectile, spawnPosition, selectedProjectile.transform.rotation);
        centerController.rotateSpeed = -centerController.rotateSpeed;
    }

    void SpawnObstacles(GameObject obstacle, int obstacleCount) // Burada spawn olacak objeyi ve obje say�s�n� al�yoruz.
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            float angle = i * Mathf.PI * 2f / obstacleCount;    // Burada obje say�s�na g�re de�i�en bir a�� tan�ml�yoruz.

            float radius = 2f; // merkezden ne kadar uzakta spawn olmas�n� istedi�imiz de�er.

            Vector3 spawnPos = new Vector3((Mathf.Cos(angle) * radius) + center.transform.localPosition.x, center.transform.position.y , (Mathf.Sin(angle) * radius)+ center.transform.localPosition.z); 
            // Buray� T�rk�e'ye �evirmem biraz zor deneme yan�lma ile yazd�m bu form�l� :D , de�i�en a��ya g�re center'�n etraf�nda pozisyonlar olu�turuyoruz.

            GameObject createdObstacle = Instantiate(obstacle, spawnPos, obstacle.transform.rotation, center.transform); // Olu�an pozisyonlarda objelerimizi spawnl�yoruz ve onlara createdObstacle ad�n� at�yoruz.

            createdObstacle.transform.rotation = Quaternion.Euler(0, -Mathf.Rad2Deg * angle, 90f);  // Burada olu�an objelerin rotasyonlar�n� de�i�tiriyoruz. -Rad2Deg ile �arpmay� da deneme yan�lma ile buldum.

        }
    }

    IEnumerator OpenDangerText()
    {
        dangerText.gameObject.SetActive(true);
        textBackground.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        dangerText.gameObject.SetActive(false);
        textBackground.SetActive(false);
        yield break;
    }

    IEnumerator AutoChangingSpeed()
    {

        while (true)
        {
            float randomTime = Random.Range(2.3f, 3.7f);
            yield return new WaitForSecondsRealtime(randomTime);
            centerController.rotateSpeed = Random.Range(-90, 90f);
        }
    }

    IEnumerator AutoReversingSpeed()
    {
        StartCoroutine(OpenDangerText());
        dangerText.text = "Speed Can Be Reversed !";
        while (true)
        {
            float changeTime = 3f;
            if (centerController.rotateSpeed <= 0)
            {
                yield return new WaitForSecondsRealtime(changeTime);
            }
            else
            {
                yield return new WaitForSecondsRealtime(changeTime * 2f);

            }
            centerController.rotateSpeed = -centerController.rotateSpeed;

        }
    }

    IEnumerator TimeLimitLevel()
    {
        StartCoroutine(OpenDangerText());
        timeText.gameObject.SetActive(true);
        timeText.text = timeLimit.ToString() + " Seconds Left";
        dangerText.text = "You Have " + (int)timeLimit + "Seconds To Pass This Level !";
        while (true)
        {
            yield return new WaitForFixedUpdate();
            timeText.text = (int)timeLimit + " Seconds Left";
            if (sendedProjectile >=1)
            {
                timeLimit -= Time.deltaTime;
                if (timeLimit <= 0)
                {
                    GameManager.gameOver = true;
                    levelManager.LevelLose();
                    Time.timeScale = 0.04f;
                    yield break;
                }
            }
        }
    }

    public IEnumerator AutoLight()
    {
        while (true)
        {
            if (GameManager.gameOver)
            {
                light.gameObject.SetActive(true);
                yield break;
            }
            yield return new WaitForSecondsRealtime(2.5f);
            light.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.35f);
            light.gameObject.SetActive(false);
        }
    }


    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }

    private int ProjectileCountForLevel()
    {
        if (LevelManager.level == 1)
        {
            return 15;
        }
        else if (LevelManager.level == 2)
        {
            return 20;
        }
        else if (LevelManager.level == 3)
        {
            return 25;
        }
        else if (LevelManager.level == 4)
        {
            return 18;
        }
        else if (LevelManager.level == 5)
        {
            return 20;
        }
        else if (LevelManager.level == 6)
        {
            return 24;
        }
        else
        {
            int x;
            if (LevelManager.level >= 10)
            {
                x = Random.Range(14, 17);

            }
            else
            {
                x = Random.Range(14, 21);

            }
            return x;
        }
    }

    void RotateSpeedForLevel()
    {
        if (LevelManager.level == 1)
        {
            centerController.rotateSpeed = 45f;
        }
        else if (LevelManager.level == 2)
        {
            centerController.rotateSpeed = -52f;
        }
        else if (LevelManager.level == 3)
        {
            centerController.rotateSpeed = 35f;
        }
        else if (LevelManager.level == 4)
        {
            centerController.rotateSpeed = -90f;
        }
        else if (LevelManager.level == 5)
        {
            centerController.rotateSpeed = 45f;
        }
        else if (LevelManager.level == 6)
        {
            centerController.rotateSpeed = -42f;
        }
        else
        {
            if (centerController)
            {
                float x =Random.Range(-77, 77);
                if (Mathf.Abs(x) <= 10 && Mathf.Abs(x) >= 5)
                {
                    x *= 3.4f;
                }
                else if (Mathf.Abs(x) < 5)
                {
                    x = 33;
                }

                centerController.rotateSpeed = x;
            }
        }
    }

}