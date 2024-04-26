using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour
{
    [SerializeField] private Image guideImage;
    [SerializeField] private DOTweenAnimation dotAnim;
    [SerializeField] private DOTweenAnimation warningAnim;
    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private TextMeshProUGUI warningText;

    public void SetMessage(string text)
    {
        guideText.text = text;
        dotAnim.DORestartById("Start");
    }

    public void WarningSetMessage()
    {
        warningAnim.DORestartById("Start");
    }
}
