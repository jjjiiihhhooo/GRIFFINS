using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    
    void Start()
    {
        Invoke("Spawn", 0.05f);
    }


    private void Spawn()
    {
        FindObjectOfType<Player>().transform.position = transform.position;
        Debug.Log("Spawn");
        Debug.Log("Player : " + FindObjectOfType<Player>().transform.position);
        Debug.Log("SpawnPoint : " + transform.position);
    }
}
