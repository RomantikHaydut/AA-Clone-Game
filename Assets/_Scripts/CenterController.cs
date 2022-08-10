using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterController : MonoBehaviour
{
    public float rotateSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public IEnumerator GoBig()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.005f);
            transform.localScale += new Vector3(1, 0, 1) * Time.deltaTime;
        }
    }
    public IEnumerator GoSmall()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.005f);
            transform.localScale -= new Vector3(1.5f, 0, 1.5f) * Time.deltaTime;
        }
    }
}
