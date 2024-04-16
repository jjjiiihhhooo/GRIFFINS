using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject grappleImage;
    public GameObject normalImage;
    public GameObject crossHair;
    public GameObject tapTitle_obj;
    public GameObject tapMain_obj;
    public GameObject tapSmall_obj;


    public DOTweenAnimation skillTitle_dot;

    public Image leftImage;
    public Image leftImageCool;
    public Image rightImage;
    public Image rightImageCool;

    public Image curCharacterImage;

    public Image[] characterImages;
    public Sprite[] leftImages;
    public Sprite[] rightImages;

    public Image redFaceImage;
    public Sprite redFaceSprite;
    public Image greenFaceImage;
    public Sprite greenFaceSprite;

    public Slider playerHp;
    public Slider playerBackHp;
    public Slider bossBackHp;
    public Slider bossHp;
    public Slider bossTiming;

    public DOTweenAnimation playerHpDot;
    public DOTweenAnimation bossHpDot;

    

    private void Update()
    {
        CoolCheck();
    }

    public void Init()
    {
        ChangeCharacterUI(0);
    }

    public void ChangeCharacterUI(int index)
    {
        Color b = Color.black;
        Color y = Color.yellow;
        for(int i = 0; i < 3; i++)
        {
            characterImages[i].color = b;
        }

        characterImages[index].color = y;

        SkillChangeUI(index);
    }

    private void CoolCheck()
    {
        if (Player.Instance == null) return;

        if(Player.Instance.currentCharacter.GetType() == typeof(GreenCharacter))
        {
            leftImageCool.fillAmount = Player.Instance.skillData.grapplingCdTimer / Player.Instance.skillData.grapplingCd;
        }
        else if(Player.Instance.currentCharacter.GetType() == typeof(WhiteCharacter))
        {
            leftImageCool.fillAmount = 0;
            rightImageCool.fillAmount = 0;
        }
        else if(Player.Instance.currentCharacter.GetType() == typeof(RedCharacter))
        {
            leftImageCool.fillAmount = 0;
            rightImageCool.fillAmount = 0;
        }
    }

    public void SkillChangeUI(int index)
    {
        leftImage.sprite = leftImages[index];
        rightImage.sprite = rightImages[index];
    }


    public void AddCharacterUI()
    {
        greenFaceImage.sprite = greenFaceSprite;
        redFaceImage.sprite = redFaceSprite;
    }
}
