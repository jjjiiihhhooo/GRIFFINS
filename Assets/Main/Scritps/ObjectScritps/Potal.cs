using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{

    public string sceneName;


    private void SceneLoad()
    {
        LoadingSceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            SceneLoad();
        }
    }
}
