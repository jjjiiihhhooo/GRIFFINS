using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyObjectController : SerializedMonoBehaviour
{
    public EnemyAttackObject attackObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().testHp--;
        }

        if (!collision.transform.CompareTag(this.transform.tag))
        {
            Destroy(this.gameObject);
        }
    }


}
