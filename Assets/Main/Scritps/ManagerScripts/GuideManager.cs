using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Notice
{
    public Sprite[] faces;
    public string text;
}

public class GuideManager : MonoBehaviour
{
    [SerializeField] private Image guideImage;
    [SerializeField] private Image npc_Image;
    [SerializeField] private Image npc_face_delay;
    [SerializeField] private DOTweenAnimation dotAnim;
    [SerializeField] private DOTweenAnimation warningAnim;
    [SerializeField] private DOTweenAnimation noticeAnim;
    [SerializeField] private DOTweenAnimation notice_dialogueAnim;

    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TextMeshProUGUI notice_dialogue;
    [SerializeField] private Sprite[] default_faces;

    private Notice[] curNotice;

    public void SetMessage(string text)
    {
        guideText.text = text;
        dotAnim.DORestartById("Start");
    }

    public void WarningSetMessage()
    {
        warningAnim.DORestartById("Start");
    }

    public void SetNotice(Notice[] notices)
    {
        curNotice = notices;
        noticeAnim.DORestartById("Start");
    }

    public void NpcAnim()
    {
        StartCoroutine(NpcAnimCor());
    }

    public IEnumerator NpcAnimCor()
    {
        for (int i = 0; i < 2; i++)
        {
            npc_face_delay.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            if (i == 0) npc_Image.sprite = default_faces[1];
            else if (i == 1) npc_Image.sprite = curNotice[0].faces[0];
            npc_face_delay.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }

        notice_dialogueAnim.DORestartById("Start");
    }

    public void PlayNotice()
    {
        StartCoroutine(NoticeCor());
    }

    public void PlayerDead()
    {
        if(Player.Instance.isDead)
        {
            Player.Instance.DeadNotice();
        }
    }

    private void EndNotice()
    {
        npc_Image.sprite = default_faces[0];
        notice_dialogueAnim.DORestartById("End");
    }

    private IEnumerator NoticeCor()
    {
        for (int i = 0; i < curNotice.Length; i++)
        {
            notice_dialogue.text = "";
            npc_Image.sprite = curNotice[i].faces[0];


            notice_dialogue.gameObject.SetActive(true);

            notice_dialogue.DOText(curNotice[i].text, 0.5f);

            for (int j = 0; j < curNotice[i].faces.Length; j++)
            {
                npc_Image.sprite = curNotice[i].faces[j];
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2f);
            notice_dialogue.gameObject.SetActive(false);
        }

        EndNotice();
    }
}
