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
        player.currentCharacter.NormalAttackExit();
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
        player.currentCharacter.StrongAttackExit();

    }

    public void Q_AttackExit()
    {
        player.currentCharacter.Q_AnimExit();
    }

    public void E_AttackExit()
    {
        player.currentCharacter.E_AnimExit();
    }

    public void R_AttackExit()
    {
        player.currentCharacter.R_AnimExit();
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

