using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Potal : MonoBehaviour
{

    public string sceneName;
    private void SceneLoad()
    {
        DataSave();
        LoadingSceneManager.LoadScene(sceneName);
    }

    private void DataSave()
    {
        Player.Instance.spawn = null;
        //Player.Instance.targetSet.Targets.Clear(); 
        //타겟범위에 들어와있는 오브젝트들 삭제
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            SceneLoad();
        }
        Invoke("", 0f);
    }
}
