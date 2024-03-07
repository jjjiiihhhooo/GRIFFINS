using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Image spacebarImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DOTweenAnimation dialogueDot;

    private Dialogue[] curDialogues;

    private bool isDialogue;
    private bool isChat;
    private int tempIndex;
    private string eventName;

    public bool IsChat { get => isChat; }
    public Dialogue[] CurDialogues { get => curDialogues; }

    public void Init()
    {
        curDialogues = null;
    }

    public void ChangeDialogue(Dialogue[] logs, string name)
    {
        Debug.Log("ChangeDialogue");
        curDialogues = null;
        eventName = name;
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

        if (!dialogueDot.gameObject.activeSelf)
        {
            isChat = true;
            dialogueDot.gameObject.SetActive(true);
            dialogueDot.DORestartById("Start");
        }

        isDialogue = true;
        nameText.text = curDialogues[tempIndex].name;
        dialogueText.text = "";
        dialogueText.DOKill();
        dialogueText.DOText(curDialogues[tempIndex].text, 0.5f).OnComplete(() => { isDialogue = false; tempIndex++; spacebarImage.gameObject.SetActive(true); });

    }

    public void EndDialogue()
    {
        Debug.Log("TestEnd");
        isChat = false;
        tempIndex = 0;
        curDialogues = null;
        GameManager.Instance.event_dictionary[eventName].Invoke();
        dialogueDot.DORestartById("End");
    }

}
