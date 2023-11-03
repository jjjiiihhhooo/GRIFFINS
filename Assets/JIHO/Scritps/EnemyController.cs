using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : SerializedMonoBehaviour
{

    public DmgTxt dmgTxt;
    public Canvas canvas;
    public Enemy enemy;

    private void Awake()
    {
        enemy.Init(this);
    }

    private void Update()
    {
        CanvasMove();
    }

    private void CanvasMove()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("AttackCol"))
        {
            float damage = PlayerController.Instance.currentUnit.curDamage;
            
            Vector3 center1 = other.bounds.center;
            Vector3 center2 = transform.GetComponent<BoxCollider>().bounds.center;

            Vector3 finalCenter = (center1 + center2) / 2f;

            DamageEffect(damage, finalCenter);
            enemy.GetDamage(damage);
        }
    }

    private void DamageEffect(float damage, Vector3 targetPos)
    {
        DamageTxt(damage, targetPos);
        
    }

    private void DamageTxt(float damage, Vector3 targetPos)
    {
        if (canvas.worldCamera == null) canvas.worldCamera = Camera.main;
        GameObject temp = Instantiate(dmgTxt.gameObject, canvas.transform);
        temp.GetComponent<DmgTxt>().text.text = damage.ToString();
        temp.SetActive(false);
        temp.transform.position = targetPos;
        temp.SetActive(true);
    }

    private void DamageHitEffect()
    {

    }
}
