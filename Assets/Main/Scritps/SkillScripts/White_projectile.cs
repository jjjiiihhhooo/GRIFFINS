using UnityEngine;

public class White_projectile : MonoBehaviour
{
    public Transform target;
    public float speed;

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.MoveTowards(transform.position, target.position + Vector3.up, speed * Time.deltaTime);
        else
            Destroy(this.gameObject);
    }

    private void DestroyEvent()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Environment")
            Invoke("DestroyEvent", 0.5f);
    }
}
