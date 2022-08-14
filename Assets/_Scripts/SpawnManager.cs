using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public Light light;

    private GameObject projectile;

    public GameObject[] arrowCountImages;

    private GameObject obstacle;

    private GameObject center;

    public static bool canShoot;

    private Vector3 spawnPosition;

    public static float projectileSpeed;

    public Image timeBar;

    private float timeLimit = 17f;

    public static int sendedProjectile;

    public GameObject textBackground;

    TextMeshProUGUI levelText;

    TextMeshProUGUI dangerText;

    private CenterController centerController;

    private LevelManager levelManager;

    public bool reverserLevel;


    void Start()
    {

        projectileSpeed = Random.Range(2.2f, 3f);

        levelManager = FindObjectOfType<LevelManager>();

        LevelManager.levelWin = false;

        obstacle = levelManager.obstacle;

        centerController = FindObjectOfType<CenterController>();

        sendedProjectile = 0;

        center = GameObject.FindGameObjectWithTag("Center");

        Vector3 direction = (center.transform.position - transform.position).normalized;

        spawnPosition = transform.position + direction / 2;

        LevelManager.projectileCount = ProjectileCountForLevel();

        levelText = GameObject.FindGameObjectWithTag("Level Text").GetComponent<TextMeshProUGUI>();

        levelText.text = LevelManager.level.ToString();

        if (LevelManager.level >= 10)
        {
            //levelText.rectTransform.rect.Set(3, 0, 44, 44);
            levelText.rectTransform.anchoredPosition = new Vector2(3, levelText.rectTransform.anchoredPosition.y);
        }

        dangerText = GameObject.Find("Danger Text").GetComponent<TextMeshProUGUI>();

        dangerText.gameObject.SetActive(false);

        textBackground = GameObject.FindGameObjectWithTag("Purple Background");

        textBackground.SetActive(false);

        RotateSpeedForLevel();

        GameManager.gameOver = false;

        canShoot = true;

        reverserLevel = false;

        if (LevelManager.level > 4 && LevelManager.level != 8 && LevelManager.level != 14 && LevelManager.level != 3 && LevelManager.level != 18)
        {
            if(LevelManager.level == 20)
            {
                SpawnObstacles(obstacle, 3);
            }
            else
            {
                SpawnObstacles(obstacle, (LevelManager.level / 3) + 1);
            }
        }

        if (LevelManager.level % 5 == 0 && LevelManager.level % 3 != 0 && LevelManager.level %4 != 0 && LevelManager.level > 4)
        {
            reverserLevel = true;
            StartCoroutine(OpenDangerText());
            dangerText.text = "Speed Will Change When You Fire ";
        }

        if (LevelManager.level % 4 == 0 && LevelManager.level % 12 != 0 && LevelManager.level > 8)
        {
            StartCoroutine(OpenDangerText());
            StartCoroutine(AutoChangingSpeed());
        }


        if (LevelManager.level % 3 == 0 && LevelManager.level > 3 && LevelManager.level != 18 || LevelManager.level == 2)
        {
            StartCoroutine(TimeLimitLevel());
        }

        if (LevelManager.level % 7 == 0 && LevelManager.level > 6 && LevelManager.level != 14)
        {
            StartCoroutine(AutoReversingSpeed());
        }



        if (LevelManager.level == 3) // Dark Level
        {
            projectile = levelManager.projectileLighting;
        }
        else if (LevelManager.level == 8)
        {
            projectile = levelManager.projectileLighting;
        }
        else if (LevelManager.level == 14)
        {
            projectile = levelManager.projectileLighting;
        }
        else if (LevelManager.level == 18)
        {
            projectile = levelManager.projectileLighting;
        }
        else if (LevelManager.level == 20)
        {
            projectile = levelManager.projectileLighting;
            light = GameObject.Find("Directional Light").GetComponent<Light>();
            StartCoroutine(AutoLight());
        }
        else // other levels
        {
            projectile = levelManager.projectile;
        }

        CalculateArrowCount();
        Time.timeScale = 1f;

    }


    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && !GameManager.gameOver && canShoot && !reverserLevel) // Use SýmpleInput for mobile.
            {
                SpawnProjectile(projectile);
                sendedProjectile++;
                DestroyArrowImage();
                canShoot = false;

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began && !GameManager.gameOver && canShoot && reverserLevel)
            {
                SpawnReverserProjectile(projectile);
                sendedProjectile++;
                DestroyArrowImage();
                canShoot = false;
            }
        }

    }

    void CalculateArrowCount()
    {
        int y = 0;
        for (int i = 1; i < arrowCountImages.Length + 1; i++)
        {
            if (i >= LevelManager.projectileCount)
            {
                y = i;
                if (y < arrowCountImages.Length)
                {
                    Destroy(arrowCountImages[y].gameObject);
                }
            }
        }
    }

    void DestroyArrowImage()
    {
        int index = LevelManager.projectileCount - sendedProjectile;
        Destroy(arrowCountImages[index].gameObject);
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

    void SpawnObstacles(GameObject obstacle, int obstacleCount) // Burada spawn olacak objeyi ve obje sayýsýný alýyoruz.
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            float angle = i * Mathf.PI * 2f / obstacleCount;    // Burada obje sayýsýna göre deðiþen bir açý tanýmlýyoruz.

            float radius = 2f; // merkezden ne kadar uzakta spawn olmasýný istediðimiz deðer.

            Vector3 spawnPos = new Vector3((Mathf.Cos(angle) * radius) + center.transform.localPosition.x, center.transform.position.y, (Mathf.Sin(angle) * radius) + center.transform.localPosition.z);
            // Burayý Türkçe'ye çevirmem biraz zor deneme yanýlma ile yazdým bu formülü :D , deðiþen açýya göre center'ýn etrafýnda pozisyonlar oluþturuyoruz.

            GameObject createdObstacle = Instantiate(obstacle, spawnPos, obstacle.transform.rotation, center.transform); // Oluþan pozisyonlarda objelerimizi spawnlýyoruz ve onlara createdObstacle adýný atýyoruz.

            createdObstacle.transform.rotation = Quaternion.Euler(0, -Mathf.Rad2Deg * angle, 90f);  // Burada oluþan objelerin rotasyonlarýný deðiþtiriyoruz. -Rad2Deg ile çarpmayý da deneme yanýlma ile buldum.

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
            float randomTime = Random.Range(2.3f, 3.4f);
            yield return new WaitForSecondsRealtime(randomTime);
            centerController.rotateSpeed *= 2f;
            yield return new WaitForSecondsRealtime(randomTime);
            centerController.rotateSpeed /= 2f;
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
        dangerText.text = "You Have " + (int)timeLimit + "Seconds To Pass This Level !";
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (sendedProjectile >= 1)
            {
                timeLimit -= Time.deltaTime;
                timeBar.fillAmount = timeLimit / 17;
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
            yield return new WaitForSecondsRealtime(0.4f);
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
            return 23;
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
            return 22;
        }
        else if (LevelManager.level == 14)
        {
            return 23;
        }
        else if (LevelManager.level == 18)
        {
            return 25;
        }
        else if (LevelManager.level == 20)
        {
            return 14;
        }
        else
        {
            int x;
            if (LevelManager.level >= 7 && LevelManager.level < 10)
            {
                x = Random.Range(15, 17);
            }
            else if (LevelManager.level >= 10 && LevelManager.level < 13)
            {
                x = Random.Range(15, 19);

            }
            else if(LevelManager.level >= 13 && LevelManager.level < 17)
            {
                x = Random.Range(16, 22);

            }
            else if (LevelManager.level >= 17 && LevelManager.level <20)
            {
                x = Random.Range(19, 22);

            }
            else
            {
                x = 18;
            }
            return x;
        }
    }

    void RotateSpeedForLevel()
    {
        if (centerController != null)
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
                centerController.rotateSpeed = -40f;
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
                if (centerController !=null)
                {
                    float x = Random.Range(-77, 77);
                    if (Mathf.Abs(x) <= 10 && Mathf.Abs(x) >= 5)
                    {
                        x *= 3.4f;
                    }
                    else if (Mathf.Abs(x) < 5)
                    {
                        x = 33;
                    }
                    else if ((Mathf.Abs(x) <= 15 && Mathf.Abs(x) > 10))
                    {
                        x *= 3.5f;
                    }
                    else if ((Mathf.Abs(x) <= 25 && Mathf.Abs(x) > 15))
                    {
                        x *= 2f;
                    }
                    centerController.rotateSpeed = x;
                }
            }
        }
    }
        

}
