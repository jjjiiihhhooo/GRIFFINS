using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWave : MonoBehaviour
{
    public GameObject temp;

    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(temp, Player.Instance.transform.position, Quaternion.identity);
            FindObjectOfType<QuestManager>().QuestInput(FindObjectOfType<QuestContainer>().curQuest);
        }
    }
}
