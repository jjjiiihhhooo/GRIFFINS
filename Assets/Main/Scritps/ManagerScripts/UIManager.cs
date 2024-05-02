using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;

    [Header("Player")]
    public Image playerHp;
    public Image playerBackHp;

    [Header("Boss")]
    public Slider bossBackHp;
    public Slider bossHp;
    public Slider bossTiming;

    public GameObject Q_Skill_Icon;
    public GameObject E_Skill_Icon;

    [Header("DotweenAnimation")]
    public DOTweenAnimation playerHpDotween;
    public DOTweenAnimation bossHpDot;

    [Header("Image")]
    public Image[] characterFaces;
    public Image[] characterCools;
    public Image[] skillCools;
    public Image mainImage;
    public Image[] characterChangeAnim;
    public Image characterMainAnim;

    [Header("Sprite")]
    public Sprite[] nonColorCharacters;
    public Sprite[] colorCharacters;
    public Sprite[] mainSprites;

    public Sprite characterChangeWhite;
    public Sprite characterChangeLogo;
    public Sprite characterChangeMainWhite;
    public Sprite characterChangeMainLogo;

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
        for (int i = 0; i < 3; i++)
        {
            //characterFaces[i].sprite = nonColorCharacters[i];

            if (i != index) characterCools[i].fillAmount = 1;
        }

        //characterFaces[index].sprite = colorCharacters[index];
        characterCools[index].fillAmount = 0;
        mainImage.sprite = mainSprites[index];
        ChangeUICorEvent();
    }

    public void Init()
    {
        //ChangeCharacterUI(0);
    }

    public void ChangeAnim()
    {
        int index = Player.Instance.currentCharacter.index;

        for (int i = 0; i < 3; i++)
        {
            if (index != i) StartCoroutine(CharacterChange(i));
        }

    }

    private IEnumerator CharacterChange(int index)
    {
        Color a = characterChangeAnim[index].color;
        characterChangeAnim[index].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.01f);
        float count = 1f;

        while (count > 0)
        {
            a.a = count;
            characterChangeAnim[index].color = a;

            count -= 0.3f;
            yield return new WaitForFixedUpdate();
        }
        characterChangeAnim[index].gameObject.SetActive(false);
        a.a = 1f;
        characterChangeAnim[index].color = a;
    }

    public void ChangeUICorEvent()
    {
        StartCoroutine(ChangeUICor(0));
        StartCoroutine(ChangeUICor(1));
        StartCoroutine(ChangeUICor(2));
        StartCoroutine(MainCor());
    }

    private IEnumerator MainCor()
    {
        Color a = characterMainAnim.color;
        characterMainAnim.gameObject.SetActive(true);
        characterMainAnim.sprite = characterChangeMainWhite;

        yield return new WaitForSecondsRealtime(0.01f);
        characterMainAnim.sprite = characterChangeMainLogo;

        yield return new WaitForSecondsRealtime(0.02f);
        characterMainAnim.sprite = characterChangeMainWhite;

        float count = 1f;

        while (count > 0)
        {
            a.a = count;
            characterMainAnim.color = a;
            count -= 0.3f;
            yield return new WaitForFixedUpdate();
        }
        characterMainAnim.gameObject.SetActive(false);
        a.a = 1f;
        characterMainAnim.color = a;

    }

    public IEnumerator ChangeUICor(int index)
    {
        Color a = characterChangeAnim[index].color;

        characterChangeAnim[index].sprite = characterChangeWhite;
        characterChangeAnim[index].gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.01f);
        characterChangeAnim[index].sprite = characterChangeLogo;

        yield return new WaitForSecondsRealtime(0.02f);
        characterChangeAnim[index].sprite = characterChangeWhite;

        float count = 1f;

        while (count > 0)
        {
            a.a = count;
            characterChangeAnim[index].color = a;
            count -= 0.3f;
            yield return new WaitForFixedUpdate();
        }
        characterChangeAnim[index].gameObject.SetActive(false);
        a.a = 1f;
        characterChangeAnim[index].color = a;


    }
}
