using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public string text;
}

public class DialogueContainer : MonoBehaviour
{
    public Dialogue[] dialoges;

    public void SetDialoge()
    {

    }
}
