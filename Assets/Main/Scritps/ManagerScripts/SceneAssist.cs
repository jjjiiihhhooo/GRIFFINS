using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssist : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation dot;

    private void Start()
    {
        if(dot != null)
        {
            Time.timeScale = 0f;
            FindObjectOfType<CinemachineInputProvider>().enabled = false;
            GameManager.Instance.MouseLocked(true);
        }
        //GameManager.Instance.Scene_AssistAction(this.GetComponent<DOTweenAnimation>());
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        dot.DOPlayById("Exit");
    }

    public void Exit()
    {
        
        FindObjectOfType<CinemachineInputProvider>().enabled = true;
        GameManager.Instance.MouseLocked();
        this.gameObject.SetActive(false);
    }

}
