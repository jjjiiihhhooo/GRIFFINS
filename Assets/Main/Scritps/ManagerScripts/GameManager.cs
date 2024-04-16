using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager Instance;

    public SoundManager soundManager;
    public InputData inputData;
    public StaminaManager staminaManager;
    public TimelineManager timelineManager;
    public TutorialManager tutorialManager;
    public GuideManager guideManager;
    public QuestManager questManager;
    public EventManager eventManager;
    public DialogueManager dialogueManager;
    public UIManager uiManager;
    public Transform predictionHitTransform;

    public Dictionary<string, UnityEvent> event_dictionary;

    public bool isMouseLock;

    private void Awake()
    {
        if(Instance == null)
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
        timelineManager.Init();
        dialogueManager.Init();
        uiManager.Init();
        MouseLocked(isMouseLock);
    }

    public void MouseLocked(bool _bool = false)
    {
        if(!_bool)
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
}