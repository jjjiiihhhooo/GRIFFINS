using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public string text;
    public Sprite leftImage;
    public Sprite rightImage;
    public int teamIndex;
    public bool isLeft;

}

public class DialogueContainer : MonoBehaviour
{
    public Dialogue[] dialogues;
    public string eventName;
    public string chatKey = "";

    public void SetDialogue()
    {
        Debug.Log("SetDialogue");
        GameManager.Instance.dialogueManager.ChangeDialogue(dialogues, chatKey);
    }

    public void StartEvent()
    {

        SetDialogue();
        GameManager.Instance.dialogueManager.StartDialogue();
    }
}
