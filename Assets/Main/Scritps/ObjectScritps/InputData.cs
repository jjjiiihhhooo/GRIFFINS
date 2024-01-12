using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputData : MonoBehaviour
{


    private void Update()
    {
        QInput();
    }

    private void QInput()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Player.Instance.skillData.skillIndex = 0;
            Player.Instance.skillData.Invoke(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex], 0f);
        }
        else if(Input.GetMouseButtonDown(0))
        {
            Player.Instance.skillData.skillIndex = 1;
            Player.Instance.skillData.Invoke(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex], 0f);
            //Player.Instance.skillData.SendMessage(Player.Instance.skillData.skillName[Player.Instance.skillData.skillIndex]);
        }

        
    }
}
