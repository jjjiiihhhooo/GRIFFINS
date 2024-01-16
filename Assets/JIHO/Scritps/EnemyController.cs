
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : SerializedMonoBehaviour
{

    public DmgTxt dmgTxt;
    public Canvas canvas;
    public Enemy enemy;
    public bool isHit;
    public float hitCool = 0.1f;

    private void Awake()
    {
        enemy.Init(this);
    }

    private void Update()
    {
        CanvasMove();

        if(isHit)
        {
            if (hitCool > 0) hitCool -= Time.deltaTime; 
            else isHit = false;
        }
    }

    private void CanvasMove()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("AttackCol"))
        {
            float damage = other.GetComponent<AttackCol>().damage;
            
            Vector3 center1 = other.bounds.center;
            Vector3 center2 = transform.GetComponent<BoxCollider>().bounds.center;

            Vector3 finalCenter = (center1 + center2) / 2f;

            DamageMessage(damage, finalCenter);
        }
    }

    public void DamageMessage(float damage, Vector3 targetPos)
    {
        if (isHit) return;
        isHit = true;
        hitCool = 0.1f;

        DamageEffect(damage, targetPos);
        enemy.GetDamage(damage);
    }

    private void DamageEffect(float damage, Vector3 targetPos)
    {
        //DamageHitTxt(damage, targetPos);
        DamageHitEffect(targetPos);
    }

    private void DamageHitTxt(float damage, Vector3 targetPos)
    {
        if (canvas.worldCamera == null) canvas.worldCamera = Camera.main;
        GameObject temp = Instantiate(dmgTxt.gameObject, canvas.transform);
        temp.GetComponent<DmgTxt>().text.text = damage.ToString();
        temp.SetActive(false);
        temp.transform.position = targetPos;
        temp.SetActive(true);
    }

    private void DamageHitEffect(Vector3 targetPos)
    {
        enemy.damageEffect.transform.position = targetPos;
        enemy.damageEffect.Play();
    }

    public void Dead()
    {
        QuestManager.instance.QuestMonsterCheck(enemy.name);
        Destroy(this.gameObject);
    }
}
