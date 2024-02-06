using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool isNotSceneChange = false;


    void Start()
    {
        if(!isNotSceneChange) Invoke("Spawn", 0.05f);

    }


    public void Spawn()
    {
        FindObjectOfType<Player>().transform.position = transform.position;
        Debug.Log("Spawn");
        Debug.Log("Player : " + FindObjectOfType<Player>().transform.position);
        Debug.Log("SpawnPoint : " + transform.position);
    }
}
