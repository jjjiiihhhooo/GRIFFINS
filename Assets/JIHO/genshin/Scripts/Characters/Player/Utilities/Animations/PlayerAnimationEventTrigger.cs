using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
    }

    public void TriggerOnMovementStateAnimationEnterEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }

        player.OnMovementStateAnimationEnterEvent();
    }

    public void TriggerOnMovementStateAnimationExitEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }

        player.OnMovementStateAnimationExitEvent();
    }

    public void TriggerOnMovementStateAnimationTransitionEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }

        player.OnMovementStateAnimationTransitionEvent();
    }

    private bool IsInAnimationTransition(int layerIndex = 0)
    {
        return player.Animator.IsInTransition(layerIndex);
    }

    public void ThrowExit()
    {
        player.skillData.isHand = false;
    }

    public void NormalAttackExit()
    {
        //player.isAttack = false;
        player.attackCol.gameObject.SetActive(true);
        //player.currentCharacter.animator.SetBool(name, false);
    }
}

