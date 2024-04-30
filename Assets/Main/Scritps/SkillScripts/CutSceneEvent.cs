using System.Collections;
using System.Collections.Generic;
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
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
        Player.Instance.currentCharacter.CutSceneEvent(this.gameObject);
        
    }
}
