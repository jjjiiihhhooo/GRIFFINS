using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image dashImage;
    [SerializeField] private Image superJumpImage;
    [SerializeField] private Image playerHpImage;

    private void Update()
    {
        UIUpdate();
    }

    private void UIUpdate()
    {
        CoolTimeUIUpdate();
    }

    private void CoolTimeUIUpdate()
    {
        dashImage.fillAmount = Managers.Instance.CoolTimeManager.coolDic["Dash"].curCoolTime / Managers.Instance.CoolTimeManager.coolDic["Dash"].maxCoolTime;
        superJumpImage.fillAmount = Managers.Instance.CoolTimeManager.coolDic["SuperJump"].curCoolTime / Managers.Instance.CoolTimeManager.coolDic["Dash"].maxCoolTime;
    }

    public void PlayerHpUpdate()
    {
        playerHpImage.fillAmount = PlayerController.Instance.CurrentHp / PlayerController.Instance.MaxHp;
    }
}
