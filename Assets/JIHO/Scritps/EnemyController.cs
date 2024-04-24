
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : SerializedMonoBehaviour
{

    public DmgTxt dmgTxt;
    public Canvas canvas;
    public Enemy enemy;
    public Animator animator;
    public Rigidbody rigid;

    public Slider hpSlider;
    public Slider backHpSlider;

    public AttackCol attackCol;
    public GameObject targetUI_obj;

    public DOTweenAnimation anim_dot;

    public bool isHit;

    public float maxHitCool;

    public float uiShowDelay;

    private float hitCool = 0.1f;

    private Vector3 dummy = Vector3.zero;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        enemy.Init(this);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        enemy.EnemyUpdate();

        if(isHit)
        {
            if (hitCool > 0) hitCool -= Time.deltaTime; 
            else isHit = false;
        }
    }

    private void UIUpdate()
    {
        if (hpSlider == null) hpSlider = GameManager.Instance.uiManager.bossHp;
        if (backHpSlider == null) backHpSlider = GameManager.Instance.uiManager.bossBackHp;
        

        hpSlider.value = Mathf.Lerp(hpSlider.value, enemy.curHp / enemy.maxHp, Time.deltaTime * 5f);

        if(enemy.backHpHit)
        {
            backHpSlider.value = Mathf.Lerp(backHpSlider.value, hpSlider.value, Time.deltaTime * 6f);
            if(hpSlider.value >= backHpSlider.value - 0.001f)
            {
                enemy.backHpHit = false;
                backHpSlider.value = hpSlider.value;
            }
        }
        if(enemy.GetType().Name != "Boss_Enemy")
            canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("usingObject"))
        //{
        //    //float damage = other.GetComponent<AttackCol>().damage;
        //    float damage = 1f;

        //    Vector3 center1 = other.bounds.center;
        //    Vector3 center2 = transform.GetComponent<BoxCollider>().bounds.center;

        //    Vector3 finalCenter = (center1 + center2) / 2f;

        //    DamageMessage(damage, finalCenter);
        //}

        //if(other.CompareTag("AttackCol"))
        //{
        //    float damage = Player.Instance.currentCharacter.normalAttackDamage;

        //    Vector3 center1 = other.bounds.center;
        //    Vector3 center2 = transform.GetComponent<Collider>().bounds.center;

        //    Vector3 finalCenter = (center1 + center2) / 2f;


        //    DamageMessage(damage, finalCenter);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.transform.CompareTag("usingObject"))
        //{
        //    float damage = 1f;

        //    Vector3 center1 = collision.collider.bounds.center;
        //    Vector3 center2 = transform.GetComponent<BoxCollider>().bounds.center;

        //    Vector3 finalCenter = (center1 + center2) / 2f;

        //    DamageMessage(damage, finalCenter);
        //}
    }

    

    private void DamageEffect(float damage, Vector3 targetPos, ParticleSystem particle)
    {
        //DamageHitTxt(damage, targetPos);
        DamageHitEffect(targetPos, particle);
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
    private IEnumerator DeadCor()
    {
        yield return null;
    }

    private void Dead()
    {
        //QuestManager.instance.QuestMonsterCheck(enemy.name);
        FindObjectOfType<QuestManager>().EnemyQuestCheck(this.name);
        Destroy(this.gameObject);
    }

    private void DamageHitEffect(Vector3 targetPos, ParticleSystem particle)
    {
        if (particle == null) return;
        enemy.damageEffect = Instantiate(particle);
        enemy.damageEffect.transform.position = targetPos;
        enemy.damageEffect.Play();
        enemy.damageEffect = null;
    }


    public void DeadMessage()
    {
        Dead();
    }
    public void TargetCheck(bool _bool)
    {
        if (enemy.ToString() == "Boss_Enemy") return;
        if(_bool)
        {
            targetUI_obj.SetActive(true);
        }
        else
        {
            targetUI_obj.SetActive(false);
        }
    }

    public void DamageMessage(float _knockback, Vector3 _knockbackDir, float damage, Vector3 targetPos, ParticleSystem particle = null )
    {
        if (isHit) return;

        isHit = true;
        hitCool = maxHitCool;
        enemy.modelShakeTime = 0.1f;

        DamageEffect(damage, targetPos, particle);
        enemy.knockback = _knockback;
        enemy.knockbackDir = _knockbackDir;
        enemy.GetDamage(damage);
    }

    public void BackHpFunMessage()
    {
        enemy.backHpHit = true;
    }
        
    public void BossNormalAttackAnim()
    {
        enemy.isAction = false;
        attackCol.gameObject.SetActive(true);
    }

    public void CorEvent(IEnumerator cor)
    {
        StopCoroutine(cor);
        StartCoroutine(cor);
    }

    public void BossStart()
    {
        enemy.BossStart();
    }
}
