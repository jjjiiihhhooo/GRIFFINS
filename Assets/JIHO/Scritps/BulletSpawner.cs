using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private int initCount;

    [SerializeField] private GameObject bulletPrefab;

    private Queue<GameObject> bullet_Queue;

    private void Awake()
    {
        InitQueue();
    }

    private void InitQueue()
    {
        bullet_Queue = new Queue<GameObject>();

        for (int i = 0; i < initCount; i++)
        {
            InsertQueue(bulletPrefab);
        }
    }

    private void InsertQueue(GameObject bulletPrefab)
    {
        GameObject temp = Instantiate(bulletPrefab, transform);

        temp.SetActive(false);
        bullet_Queue.Enqueue(temp);
    }

    public void ReturnQueue(GameObject bullet_obj)
    {
        bullet_obj.transform.parent = this.transform;
        bullet_obj.SetActive(false);
        bullet_Queue.Enqueue(bullet_obj);
    }

    public GameObject PopQueue()
    {
        if (bullet_Queue.Count == 0) InsertQueue(bulletPrefab);
        return bullet_Queue.Dequeue();
    }

}
