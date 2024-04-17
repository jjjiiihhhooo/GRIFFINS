using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCol : MonoBehaviour
{
    [SerializeField] public float time;
    public ParticleSystem effect;
    public float damage;

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
        if (other.CompareTag("Enemy"))
        {
            
            Vector3 center1 = other.bounds.center;
            Vector3 center2 = transform.GetComponent<Collider>().bounds.center;

            Vector3 finalCenter = (center1 + center2) / 2f;

            other.GetComponent<EnemyController>().DamageMessage(damage, finalCenter, effect);
            //OnlySingleton.Instance.camShake.ShakeCamera(5f, 0.1f);
        }
    }
}
