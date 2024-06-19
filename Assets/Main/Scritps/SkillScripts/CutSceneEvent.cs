using System.Collections;
using UnityEngine;

public class CutSceneEvent : MonoBehaviour
{
    public float time;

    public bool isBoss;

    private void Awake()
    {
        StartCoroutine(EnableCor());
    }

    private IEnumerator EnableCor()
    {
        GameManager.Instance.isCutScene = true;
        yield return new WaitForSecondsRealtime(time);
        GameManager.Instance.isCutScene = false;
        //Time.timeScale = 1f;
        if (!isBoss)
        {
            Player.Instance.currentCharacter.CutSceneEvent(this.gameObject);
        }
        else
        {
            if (isBoss)
            {
                GetComponentInParent<DialogueContainer>().StartEvent();
            }

            Destroy(this.gameObject);
        }
    }
}
