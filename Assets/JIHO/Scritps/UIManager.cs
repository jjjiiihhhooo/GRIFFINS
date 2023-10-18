using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image dashImage;
    [SerializeField] private Image superJumpImage;

    private void Update()
    {
        CoolTimeUiUpdate();
    }

    private void CoolTimeUiUpdate()
    {
        dashImage.fillAmount = CoolTimeManager.Instance.coolDic["Dash"].curCoolTime / CoolTimeManager.Instance.coolDic["Dash"].maxCoolTime;
        superJumpImage.fillAmount = CoolTimeManager.Instance.coolDic["SuperJump"].curCoolTime / CoolTimeManager.Instance.coolDic["Dash"].maxCoolTime;
    }
}
