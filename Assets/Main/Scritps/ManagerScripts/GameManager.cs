using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SoundManager soundManager;
    public InputData inputData;
    public StaminaManager staminaManager;
    public TimelineManager timelineManager;
    public GameObject grappleImage;
    public GameObject normalImage;
    public GameObject crossHair;


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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        soundManager.Init();
        inputData.Init();
        staminaManager.Init();
        timelineManager.Init();
    }
}