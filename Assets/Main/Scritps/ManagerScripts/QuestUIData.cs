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
        quest_Dot.DORestartById("Start");
    }

    public void Exit()
    {
        Logo.color = Color.white;
    }

    public void QuestClear()
    {
        Logo.color = Color.green;
        logo_Dot.DORestartById("Clear");
    }

    public void QuestEnd()
    {
        quest_Dot.DORestartById("End");
    }

}
