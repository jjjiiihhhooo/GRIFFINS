using System.Collections;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] public float time;
    public float damage;
    public string otherTag;
    public bool isParticle;
    private void OnEnable()
    {
        StopCoroutine(AttackActiveCor());
        StartCoroutine(AttackActiveCor());
    }

    private IEnumerator AttackActiveCor()
    {
        float curTime = time;
        while (curTime > 0)
        {
            yield return new WaitForEndOfFrame();
            curTime -= Time.deltaTime;
        }

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(otherTag))
        {

            Vector3 center1 = other.bounds.center;
            Vector3 center2 = transform.GetComponent<Collider>().bounds.center;
            float temp = Vector3.Distance(other.transform.position, transform.position);
            temp = temp / 2;
            Vector3 dir = other.transform.position - transform.position;

            Vector3 finalCenter = transform.position + dir.normalized * temp;
            if (!isParticle) finalCenter.y = finalCenter.y + 1;

                //finalCenter.y = finalCenter.y + 1;
            if (otherTag == "Enemy")
            {
                other.GetComponent<EnemyController>().DamageMessage(Player.Instance.currentCharacter.curKnockback, Player.Instance.currentCharacter.curKnockbackDir, damage, finalCenter, Player.Instance.currentCharacter.curParticle);
                if (isParticle) Destroy(this.gameObject);
            }

            if (otherTag == "Player")
            {
                other.GetComponent<Player>().GetDamage(damage);
            }
            //OnlySingleton.Instance.camShake.ShakeCamera(5f, 0.1f);
        }
    }
}
