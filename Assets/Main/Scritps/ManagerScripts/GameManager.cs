using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager Instance;

    public SoundManager soundManager;
    public InputData inputData;
    public StaminaManager staminaManager;
    public GuideManager guideManager;
    public DialogueManager dialogueManager;
    public UIManager uiManager;
    public CoolTimeManager coolTimeManager;
    public QuestManager questManager;
    public MiniMapManager miniMapManager;
    public EnemyManager enemyManager;
    public DestinationManager destinationManager;

    public bool gameStart;
    public bool isMouseLock;
    public bool isCutScene;

    public bool isDestroy;
    public bool isMenu = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        soundManager.Init();
        inputData.Init();
        staminaManager.Init();
        dialogueManager.Init();
        uiManager.Init();
        coolTimeManager.Init();
        MouseLocked(isMouseLock);
    }

    public void MouseLocked(bool _bool = false)
    {
        if (!_bool)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Scene_AssistAction(DOTweenAnimation dot)
    {
        dot.DOPlayById("Start");
    }

    public void Pause(float time, float scale = 0f)
    {
        Time.timeScale = scale;
        StartCoroutine(PauseCor(time));
    }
  
    public void GameExit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        if (isMenu)
        {
            isMenu = false;
            OnlySingleton.Instance.camShake.GetComponent<CinemachineInputProvider>().enabled = true;
            MouseLocked(false);
            uiManager.GamePause(true);
            Time.timeScale = 1f;
        }
        else
        {
            isMenu = true;
            OnlySingleton.Instance.camShake.GetComponent<CinemachineInputProvider>().enabled = false;
            MouseLocked(true);
            uiManager.GamePause(false);
            Time.timeScale = 0f;
        }

    }

    public void ReStart(string name)
    {
        Time.timeScale = 1f;
        isDestroy = true;
        LoadingSceneManager.LoadScene(name);
    }

    public void PauseT(float time)
    {
        Time.timeScale = 0.6f;
        StartCoroutine(PauseCor(time));
    }

    public void SuperPause(float time)
    {
        Time.timeScale = 0.05f;
        StartCoroutine(PauseCor(time));
    }

    private IEnumerator PauseCor(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

    

}