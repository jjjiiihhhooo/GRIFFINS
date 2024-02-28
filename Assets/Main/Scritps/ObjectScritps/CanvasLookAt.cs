using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAt : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private bool isWorld;

    private void Awake()
    {

        if (isWorld) canvas.worldCamera = Camera.main;
    }

    void Update()
    {

        canvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
