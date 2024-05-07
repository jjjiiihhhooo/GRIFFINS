
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
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

    public float hitCool = 0.1f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        enemy.Init(this);
    }

    private void Update()
    {
        enemy.EnemyUpdate();

    }

    private void DamageEffect(float damage, Vector3 targetPos, ParticleSystem particle)
    {
        //DamageHitTxt(damage, targetPos);
        DamageHitEffect(targetPos, particle);
    }

    //private void DamageHitTxt(float damage, Vector3 targetPos)
    //{
    //    if (canvas.worldCamera == null) canvas.worldCamera = Camera.main;
    //    GameObject temp = Instantiate(dmgTxt.gameObject, canvas.transform);
    //    temp.GetComponent<DmgTxt>().text.text = damage.ToString();
    //    temp.SetActive(false);
    //    temp.transform.position = targetPos;
    //    temp.SetActive(true);
    //}


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
    
    public void DamageMessage(float _knockback, Vector3 _knockbackDir, float damage, Vector3 targetPos, ParticleSystem particle = null)
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
