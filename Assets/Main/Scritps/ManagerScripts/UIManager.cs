using DG.Tweening;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class UIManager : MonoBehaviour
{

    public GameObject crossHair;

    [Header("Player")]
    public Image playerHp;
    public Image playerBackHp;

    [Header("Boss")]
    public Slider bossBackHp;
    public Slider bossHp;
    public Slider bossTiming;


    [Header("DotweenAnimation")]
    public DOTweenAnimation playerHpDotween;
    public DOTweenAnimation bossHpDot;

    [Header("Image")]
    public Image[] characterFaces;
    public Image[] characterCools;
    public Image[] skillCools;
    public Image mainImage;

    [Header("Sprite")]
    public Sprite[] nonColorCharacters;
    public Sprite[] colorCharacters;
    public Sprite[] mainSprites;

    private Player player;

    

    private void Update()
    {
        if (!GameManager.Instance.gameStart) return;
        PlayerHPUpdate();
        CharacterCoolUpdate();
        SkillCoolUpdate();
        //CoolCheck();
    }
    private void PlayerHPUpdate()
    {
        if (player == null) player = Player.Instance;
        playerHp.fillAmount = Mathf.Lerp(playerHp.fillAmount, player.curHp / player.maxHp, 0.5f);
        //if (player.backHpHit)
        //{
        //    playerBackHp.fillAmount = Mathf.Lerp(playerBackHp.fillAmount, playerHp.fillAmount, Time.deltaTime * 6f);

        //    if (playerHp.fillAmount >= playerBackHp.fillAmount - 0.001f)
        //    {
        //        player.backHpHit = false;
        //        playerBackHp.fillAmount = playerHp.fillAmount;
        //    }
        //}
    }

    private void CharacterCoolUpdate()
    {
        characterCools[player.currentCharacter.index].fillAmount = 0;

        if (player.currentCharacter.index != 0) characterCools[0].fillAmount = player.currentCharacter.Change_Cool();
        if (player.currentCharacter.index != 1) characterCools[1].fillAmount = player.currentCharacter.Change_Cool();
        if (player.currentCharacter.index != 2) characterCools[2].fillAmount = player.currentCharacter.Change_Cool();
    }

    private void SkillCoolUpdate()
    {
        skillCools[0].fillAmount = player.currentCharacter.Right_Cool();
        skillCools[1].fillAmount = player.currentCharacter.Q_Cool();
        skillCools[2].fillAmount = player.currentCharacter.E_Cool();
        skillCools[3].fillAmount = player.currentCharacter.R_Cool();
    }

    public void PlayerHitUI()
    {
        playerHpDotween.DORestartById("Hit");
    }

    public void CharacterCharacter(int index)
    {
        for(int i = 0; i < 3; i++)
        {
            //characterFaces[i].sprite = nonColorCharacters[i];

            if(i != index) characterCools[i].fillAmount = 1;
        }

        //characterFaces[index].sprite = colorCharacters[index];
        characterCools[index].fillAmount = 0;
        mainImage.sprite = mainSprites[index];
    }

    public void Init()
    {
        //ChangeCharacterUI(0);
    }

    
}
