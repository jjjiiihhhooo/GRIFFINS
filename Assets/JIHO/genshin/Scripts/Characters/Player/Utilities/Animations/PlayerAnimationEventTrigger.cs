using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //player.normalAttackCol.effect = player.currentCharacter.normalAttackEffect;
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack1"], false);
        player.currentCharacter.normalAttackCol.gameObject.SetActive(true);
        //player.currentCharacter.animator.SetBool(name, false);
    }

    public void PauseCorEvent(float time)
    {
        GameManager.Instance.Pause(time);
    }

    public void PauseCorEventT(float time)
    {
        GameManager.Instance.PauseT(time);
    }

    public void NormalAttackExit_2()
    {
        OnlySingleton.Instance.camShake.ShakeCamera(7f, 0.1f);
        GameManager.Instance.soundManager.Play(GameManager.Instance.soundManager.audioDictionary["red_normalAttack2"], false);
        player.currentCharacter.normalAttackCol_2.gameObject.SetActive(true);
    }

    public void GrappleReady()
    {
        player.currentCharacter.isGrappleReady = false;
        player.currentCharacter.StartGrapple();
    }

    public void GrappleExit()
    {
        player.currentCharacter.StopGrapple();
    }
}

