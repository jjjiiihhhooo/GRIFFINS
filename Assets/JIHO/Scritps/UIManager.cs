using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image dashImage;
    [SerializeField] private Image superJumpImage;
    [SerializeField] private Image playerHpImage;
    [SerializeField] private Image[] playerCharacterImage;
    [SerializeField] private Image[] playerSelectImage;

    private void Update()
    {
        UIUpdate();
    }

    private void Awake()
    {
        UIInit();
    }

    private void UIInit()
    {
        for(int i = 0; i < playerCharacterImage.Length; i++)
        {
            playerCharacterImage[i].fillAmount = 0;
        }

        PlayerSelectUIUpdate(0);
    }

    private void UIUpdate()
    {
        CoolTimeUIUpdate();
    }

    private void CoolTimeUIUpdate()
    {
        dashImage.fillAmount = Managers.Instance.CoolTimeManager.coolDic["Dash"].curCoolTime / Managers.Instance.CoolTimeManager.coolDic["Dash"].maxCoolTime;
        superJumpImage.fillAmount = Managers.Instance.CoolTimeManager.coolDic["SuperJump"].curCoolTime / Managers.Instance.CoolTimeManager.coolDic["Dash"].maxCoolTime;
        
        if(Managers.Instance.CoolTimeManager.coolDic["CharacterChange"].curCoolTime > 0)
        {
            for (int i = 0; i < playerCharacterImage.Length; i++)
            {
                playerCharacterImage[i].fillAmount = Managers.Instance.CoolTimeManager.coolDic["CharacterChange"].curCoolTime / Managers.Instance.CoolTimeManager.coolDic["CharacterChange"].maxCoolTime;
            }
        }
    }

    public void PlayerHpUIUpdate()
    {
        playerHpImage.fillAmount = PlayerController.Instance.CurrentHp / PlayerController.Instance.MaxHp;
    }

    public void PlayerSelectUIUpdate(int index)
    {
        for(int i = 0; i < playerSelectImage.Length; i++)
        {
            playerSelectImage[i].color = Color.black;
        }

        playerSelectImage[index].color = Color.yellow;
    }
}
