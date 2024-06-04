
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private EnemyController _enemy;

    private void Awake()
    {
        _enemy = transform.parent.GetComponent<EnemyController>();
    }

    public void NormalAttack()
    {
        _enemy.NormalAttackAnim();
    }

    public void BombingExit()
    {
        _enemy.enemy.BombingAnimExit();
    }

    public void TrackingExit()
    {
        _enemy.enemy.TrackingAnimExit();
    }

}
