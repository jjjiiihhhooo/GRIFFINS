
using UnityEngine;

public class TitleLogic : MonoBehaviour
{
    private void Awake()
    {
        //Manager.Instance.soundManager.Play(Manager.Instance.soundManager.audioDictionary["TitleBGM"], true);
    }

    public void NextScene(string name)
    {
        LoadingSceneManager.LoadScene(name);
    }
}
