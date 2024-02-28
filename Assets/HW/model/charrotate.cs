using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charrotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // 회전 속도

    void Update()
    {
        // y축 주위로 회전
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
