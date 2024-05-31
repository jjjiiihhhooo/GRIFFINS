using System.Collections;
using UnityEngine;

public class CutSceneEvent : MonoBehaviour
{
    public float time;

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
        Player.Instance.currentCharacter.CutSceneEvent(this.gameObject);

    }
}
