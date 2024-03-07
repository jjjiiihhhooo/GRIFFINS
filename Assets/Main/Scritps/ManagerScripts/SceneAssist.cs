using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssist : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Scene_AssistAction(this.GetComponent<DOTweenAnimation>());
        
    }
}
