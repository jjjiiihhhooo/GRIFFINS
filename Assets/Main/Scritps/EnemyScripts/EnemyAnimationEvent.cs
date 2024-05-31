
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private EnemyController enemy;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<EnemyController>();
    }

    public void NormalAttack()
    {
        enemy.BossNormalAttackAnim();
    }

}
