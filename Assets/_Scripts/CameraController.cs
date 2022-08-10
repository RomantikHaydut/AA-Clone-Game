using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float followHeight;
    float followDistance;
    float followHeightSpeed;

    Transform target;


    private float targetHeight;
    private float currentHeight;
    private float currentRotation;
    Vector3 offset;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Center").GetComponent<Transform>();
        offset = target.position - transform.position;

        followHeight = 11f;
        followDistance = 6f;
        followHeightSpeed = 0.9f;
    }

    
    void LateUpdate()
    {
        targetHeight = target.position.y + followHeight;

        currentRotation = transform.eulerAngles.y;

        currentHeight = Mathf.Lerp(transform.position.y, targetHeight, followHeightSpeed * Time.deltaTime);

        //Quaternion euler = Quaternion.Euler(0, currentRotation, 0);

        Vector3 targetPos = target.position /*- (euler * Vector3.forward)*/;

        targetPos.y = currentHeight;

        transform.position = targetPos;

        transform.LookAt(target);

    }
}
