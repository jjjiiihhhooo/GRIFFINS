using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIData : MonoBehaviour
{
    public Image Logo;
    public Image background;
    public TextMeshProUGUI text;
    public DOTweenAnimation quest_Dot;
    public DOTweenAnimation logo_Dot;
    public DOTweenAnimation background_Dot;


    public void Create()
    {
        this.quest_Dot.DORestartById("Start");
    }

    public void Exit()
    {
        this.Logo.color = Color.white;
    }

    public void QuestClear()
    {
        this.Logo.color = Color.green;
        this.logo_Dot.DORestartById("Clear");
    }

    public void QuestEnd()
    {
        this.text.gameObject.SetActive(false);
        this.quest_Dot.DORestartById("End");
    }

}
