using UnityEngine;


public class Potal : MonoBehaviour
{

    public string sceneName;
    private void SceneLoad()
    {
        DataSave();
        LoadingSceneManager.LoadScene(sceneName);
    }

    public void SceneLoadEvent()
    {
        LoadingSceneManager.LoadScene(sceneName);
    }

    private void DataSave()
    {
        Player.Instance.spawn = null;
        //Player.Instance.targetSet.Targets.Clear(); 
        //Ÿ�ٹ����� �����ִ� ������Ʈ�� ����
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            SceneLoad();
        }
        Invoke("", 0f);
    }
}
