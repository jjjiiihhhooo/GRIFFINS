using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
        if (playerCanvas == null)
        {
            if (Player.Instance == null) return;
            playerCanvas = Player.Instance.playerCanvas;
        }
        if (staminaImage == null)
        {
            if (Player.Instance == null) return;
            staminaImage = Player.Instance.staminaFill;
        }

        if (playerCanvas != null)
            playerCanvas.transform.LookAt(playerCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        if (staminaImage != null)
            staminaImage.fillAmount = curStamina / maxStamina;
    }

    private void ShowStamina()
    {
        if (staminaImage == null) return;

        if (curStamina >= maxStamina)
        {
            if (staminaImage.transform.parent.gameObject.activeSelf) staminaImage.transform.parent.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (!staminaImage.transform.parent.gameObject.activeSelf) staminaImage.transform.parent.gameObject.SetActive(true);
        }
    }


    private void SprintMinusStamina()
    {
        if (Player.Instance == null) return;

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
            curStamina += Time.deltaTime * 30f;
        }
    }


    public void PlusStamina(float value, bool isCor = false)
    {
        //ShowStaminaTrigger();
        if (isCor)
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
