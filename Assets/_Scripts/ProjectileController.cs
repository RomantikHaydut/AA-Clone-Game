using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody rb;

    private float speed;

    private GameObject center;

    private GameObject myParent;

    private GameObject tails;

    private bool trailOn;

    private GameObject spawner;

    private ParticleSystem badHitEffect;

    private LevelManager levelManager;

    private bool onCenter;

    private Vector3 forceWay;

    private float turnSpeed;


    void Start()
    {
        trailOn = false;
        if (LevelManager.level > 7)
        {
            trailOn = true;
        }
        spawner = FindObjectOfType<SpawnManager>().gameObject;
        tails = transform.GetChild(0).gameObject;
        tails.SetActive(false);
        levelManager = FindObjectOfType<LevelManager>();
        badHitEffect = levelManager.badHitEffect;
        levelManager.PlayThrowSound();
        rb = GetComponent<Rigidbody>();
        center = GameObject.FindGameObjectWithTag("Center");
        onCenter = false;
        forceWay = center.transform.position - transform.position;
        myParent = transform.parent.gameObject;
        myParent.transform.LookAt(center.transform);
        speed = SpawnManager.projectileSpeed;
        if (speed != 0)
        {
            rb.AddForce(forceWay * speed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(forceWay * 15, ForceMode.Impulse);
        }

        if (LevelManager.level % 5 == 0 && LevelManager.level > 3)
        {
            myParent.transform.localScale = new Vector3(myParent.transform.localScale.x * 1.4f, myParent.transform.localScale.y * 1.4f, myParent.transform.localScale.z * 1.15f);
        }
        print(speed);

    }

    private void FixedUpdate()
    {
        if (!onCenter && !myParent.GetComponent<CenterController>())
        {
            turnSpeed = 450;
            myParent.transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Center") && !GameManager.gameOver)
        {
            rb.velocity = Vector3.zero;
            transform.parent = other.gameObject.transform;
            Destroy(myParent);
            levelManager.PlayHitSound();
            if (SpawnManager.sendedProjectile >= LevelManager.projectileCount)
            {
                Destroy(GameObject.FindGameObjectWithTag("Thrower"));
                StartCoroutine(FindObjectOfType<CenterController>().GoBig());
                levelManager.levelWin = true;
                levelManager.LevelWin();
                GameManager.gameOver = true;
            }
            onCenter = true;
            if (!trailOn)
            {
                for (int i = 0; i < tails.transform.childCount; i++)
                {
                    tails.transform.GetChild(i).gameObject.GetComponent<TrailRenderer>().enabled = false;
                }
            }
            SpawnManager.canShoot = true;
        }
        else if ((other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Obstacle")) && !levelManager.levelWin && !GameManager.gameOver && rb.velocity.magnitude > 0f)
        {
            rb.velocity = Vector3.zero;
            GameManager.gameOver = true;
            transform.parent = GameObject.FindGameObjectWithTag("Center").transform;
            Destroy(myParent);
            levelManager.PlayHitSound();
            StartCoroutine(FindObjectOfType<CenterController>().GoSmall());
            ParticleSystem effect = Instantiate(badHitEffect, other.ClosestPoint(transform.position), badHitEffect.transform.rotation,other.gameObject.transform.root);
            effect.Play();
            StartCoroutine(DestroyObject(effect.gameObject,1f));
            levelManager.LevelLose();
            if (!trailOn)
            {
                for (int i = 0; i < tails.transform.childCount; i++)
                {
                    tails.transform.GetChild(i).gameObject.GetComponent<TrailRenderer>().enabled = false;
                }
            }
            Time.timeScale = 0.04f;
        }


    }

    

    IEnumerator DestroyObject(GameObject destroyingObject , float destroyTime)
    {
        yield return new WaitForSecondsRealtime(destroyTime);
        Destroy(destroyingObject);
        yield break;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Thrower"))
        {
            tails.SetActive(true);
        }
    }
}
