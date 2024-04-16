using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool isNotSceneChange = false;


    void Start()
    {
        if(!isNotSceneChange)
        {
            Debug.Log("qqqq");
            Invoke("SetSpawn", 0.05f);
            Invoke("Spawn", 0.05f);
        }

    }

    public void SetSpawn()
    {
        if (Player.Instance.spawn != this)
        {
            Player.Instance.spawn = this;
            GameManager.Instance.event_dictionary["SetSpawn"]?.Invoke();
        }
    }

    public void Spawn()
    {
        Player.Instance.transform.position = transform.position;
    }
}
