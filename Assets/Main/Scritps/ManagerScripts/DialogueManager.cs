using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue[] curDialoges;

    public void ChangeDialouge(Dialogue[] logs)
    {
        curDialoges = null;
        curDialoges = logs;
    }

}
