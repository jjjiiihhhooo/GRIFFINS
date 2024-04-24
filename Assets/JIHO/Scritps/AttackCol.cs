using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] public float time;
    public float damage;
    public string otherTag;

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

            Vector3 finalCenter = (center1 + center2) / 2f;
            if(otherTag == "Enemy")
                other.GetComponent<EnemyController>().DamageMessage(Player.Instance.currentCharacter.curKnockback,Player.Instance.currentCharacter.curKnockbackDir, damage, finalCenter, Player.Instance.currentCharacter.curParticle);
            
            if(otherTag == "Player")
            {
                other.GetComponent<Player>().GetDamage(damage);
            }
            //OnlySingleton.Instance.camShake.ShakeCamera(5f, 0.1f);
        }
    }
}
