using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public bool gameStart;
    public bool isMouseLock;
    public bool isCutScene;

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


    public void PauseT(float time)
    {
        Time.timeScale = 0.6f;
        StartCoroutine(PauseCor(time));
    }
    private IEnumerator PauseCor(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

}