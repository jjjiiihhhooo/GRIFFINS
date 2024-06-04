using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Image spacebarImage;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private TextMeshProUGUI[] teamName;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DOTweenAnimation dialogueDot;
    public UnityEvent exit_event;

    private Dialogue[] curDialogues;

    private bool isDialogue;
    private bool isChat;
    private int tempIndex = 0;
    private string questChatKey;

    public bool IsChat { get => isChat; }
    public Dialogue[] CurDialogues { get => curDialogues; }

    public void Init()
    {
        curDialogues = null;
    }

    public void ChangeDialogue(Dialogue[] logs, string chatKey)
    {
        Debug.Log("ChangeDialogue");
        curDialogues = null;
        questChatKey = chatKey;
        curDialogues = logs;
    }

    public void StartDialogue()
    {
        if (isDialogue) return;

        if (tempIndex >= curDialogues.Length)
        {
            EndDialogue();
            return;
        }

        spacebarImage.gameObject.SetActive(false);
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);

        if (!dialogueDot.gameObject.activeSelf)
        {
            isChat = true;
            dialogueDot.gameObject.SetActive(true);
            dialogueDot.DORestartById("Start");
        }

        for (int i = 0; i < 3; i++)
            teamName[i].gameObject.SetActive(false);

        if (curDialogues[tempIndex].teamIndex != -1)
            teamName[curDialogues[tempIndex].teamIndex].gameObject.SetActive(true);

        isDialogue = true;

        if (curDialogues[tempIndex].isLeft)
        {
            leftImage.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            rightImage.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else
        {
            leftImage.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            rightImage.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        }

        if (curDialogues[tempIndex].leftImage != null)
        {
            leftImage.sprite = curDialogues[tempIndex].leftImage;
            leftImage.gameObject.SetActive(true);
        }

        if (curDialogues[tempIndex].rightImage != null)
        {
            rightImage.sprite = curDialogues[tempIndex].rightImage;
            rightImage.gameObject.SetActive(true);
        }

        nameText.text = curDialogues[tempIndex].name;
        dialogueText.text = "";
        curDialogues[tempIndex].text = curDialogues[tempIndex].text.Replace("(줄바꿈)", "\n");
        dialogueText.DOKill();
        dialogueText.DOText(curDialogues[tempIndex].text, 0.5f).OnComplete(() => { isDialogue = false; tempIndex++; spacebarImage.gameObject.SetActive(true); });

    }

    public void EndDialogue()
    {
        Debug.Log("TestEnd");
        isChat = false;
        tempIndex = 0;
        curDialogues = null;
        GameManager.Instance.questManager.ChatQuestCheck(questChatKey);

        if (exit_event != null) exit_event.Invoke();
        //if (!string.IsNullOrEmpty(eventName))
        //{
        //    // eventName이 비어있지 않은 경우에만 이벤트 호출
        //    GameManager.Instance.event_dictionary[eventName]?.Invoke();
        //}
        dialogueDot.DORestartById("End");
    }

}
