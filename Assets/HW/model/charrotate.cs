using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charrotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // ȸ�� �ӵ�

    void Update()
    {
        // y�� ������ ȸ��
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
