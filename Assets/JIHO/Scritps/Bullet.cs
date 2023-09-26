using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float scopeSpeed;
    [SerializeField] private float noScopeSpeed;
    [SerializeField] private float time;
    [SerializeField] private LayerMask platform;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private GameObject effect;

    public Vector3 direction;
    public Vector3 endPos;

    private void OnEnable()
    {
        time = 3f;
    }

    private void Update()
    {
        //if(!PlayerController.Instance.IsScope)
        //{
        //    time -= Time.deltaTime;
        //    if (time <= 0) Exit();
        //}
        
        //transform.position += direction * speed * Time.deltaTime;

        //if (PlayerController.Instance.IsScope) speed = scopeSpeed;
        //else speed = noScopeSpeed;
    }

    public void Exit()
    {
        Managers.Instance.BulletSpawner.ReturnQueue(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Exit();
        }

        if (other.gameObject.CompareTag("Object"))
        {
            GameObject temp = Instantiate(effect, transform.position, Quaternion.identity);
            Exit();
        }
    }

}
