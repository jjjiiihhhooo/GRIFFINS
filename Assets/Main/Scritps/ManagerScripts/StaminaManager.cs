using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using DG.Tweening;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private float maxStamina;                       //최대스태미나
    [SerializeField] private float curStamina;                       //현재스태미나            
    

    private Canvas playerCanvas;                                     //플레이어가 들고있는 캔버스

    
    public Image staminaImage;

    public void Init()
    {

    }

    private void Update()
    {
        DefaultPlusStamina();
        UpdateStaminaImage();
        ShowStamina();
        SprintMinusStamina();
    }

    private void UpdateStaminaImage()
    {
        if (playerCanvas == null) playerCanvas = Player.Instance.playerCanvas;
        if (staminaImage == null) staminaImage = Player.Instance.staminaFill;

        playerCanvas.transform.LookAt(playerCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        staminaImage.fillAmount = curStamina / maxStamina;
    }

    private void ShowStamina()
    {
        if (curStamina >= maxStamina)
        {
            if (staminaImage.transform.parent.gameObject.activeSelf) staminaImage.transform.parent.gameObject.SetActive(false);
            return;
        }
        else
        {
            if(!staminaImage.transform.parent.gameObject.activeSelf) staminaImage.transform.parent.gameObject.SetActive(true);
        }
    }


    private void SprintMinusStamina()
    {
        if (!Player.Instance.skillData.isSprint) return;
        //ShowStaminaTrigger();

        if (curStamina <= 0)
        {
            Player.Instance.movementStateMachine.ChangeState(Player.Instance.movementStateMachine.RunningState);
            curStamina = 0;
        }
        else
        {
            curStamina -= Time.deltaTime * 10f;
        }

    }

    private void DefaultPlusStamina()
    {
        if (curStamina == maxStamina) return;

        if (curStamina >= maxStamina)
        {
            curStamina = maxStamina;
            return;
        }
        else
        {
            curStamina += Time.deltaTime * 10f;
        }
    }

   
    public void PlusStamina(float value, bool isCor = false)
    {
        //ShowStaminaTrigger();
        if(isCor)
        {
            curStamina += value;
        }
        else
        {
            staminaImage.color = Color.cyan;
            DOTween.To(() => curStamina, data => curStamina = data, curStamina + value, 0.3f).OnComplete(() => staminaImage.color = Color.yellow);
        }
    }


    public void MinusStamina(float value)
    {
        //ShowStaminaTrigger();
        curStamina -= value;
    }

    public bool ChechStamina(float value)
    {
        return value < curStamina;
    }
}
