using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Slider progressSlider;

    private void Start()
    {
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.isDestroy)
            {
                Destroy(Player.Instance.gameObject);
                Destroy(OnlySingleton.Instance.gameObject);
                Destroy(GameManager.Instance.gameObject);
            }
        }
        
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressSlider.value = Mathf.Lerp(progressSlider.value, op.progress, timer);
                if (progressSlider.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressSlider.value = Mathf.Lerp(progressSlider.value, 1f, timer);
                if (progressSlider.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}