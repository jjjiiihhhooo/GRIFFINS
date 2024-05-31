
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().GetDamage(damage);
        }
    }
}
