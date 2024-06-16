
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
    public AttackCol hammerCol;
    public AttackCol runAttackCol;
    public GameObject targetUI_obj;

    public DOTweenAnimation anim_dot;

    public LayerMask animCheckLayer;
    public LayerMask deadLayer;

    public bool isHit;
    public bool isBoss;
    public bool isDead;
    public bool isBossStart;

    public float maxHitCool;

    public float uiShowDelay;

    public float hitCool = 0.1f;

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
        if (GameManager.Instance.isCutScene) return;
        if (isBoss)
        {
            transform.forward = Player.Instance.transform.position - transform.position;
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            if (!isBossStart)
                return;
        }

        enemy.EnemyUpdate();
        UIUpdate();

    }

    private void UIUpdate()
    {

        hpSlider.value = Mathf.Lerp(hpSlider.value, enemy.curHp / enemy.maxHp, Time.deltaTime * 5f);

        if (!isBoss)
            canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }


    public void CoroutineEvent(string name)
    {
        StopCoroutine(name);
        StartCoroutine(name);
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
        GameManager.Instance.questManager.EnemyQuestCheck(this.name);
        Destroy(this.gameObject);
    }

    private void DamageHitEffect(Vector3 targetPos, ParticleSystem particle)
    {
        if (particle == null) return;
        enemy.damageEffect = Instantiate(particle);
        enemy.damageEffect.transform.forward = Player.Instance.transform.position - transform.position;
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
        if (_bool)
        {
            targetUI_obj.SetActive(true);
        }
        else
        {
            targetUI_obj.SetActive(false);
        }
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

    public void NormalAttackAnim()
    {
        enemy.isAction = false;
        attackCol.gameObject.SetActive(true);
    }

    public void StartAttackAnim()
    {
        transform.LookAt(enemy.target.transform);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
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
