using UnityEngine;

public class QuestContainer : MonoBehaviour
{

    public QuestBox curQuest;

    public void SetQuest()
    {
        GameManager.Instance.questManager.QuestInput(curQuest);
    }
}
