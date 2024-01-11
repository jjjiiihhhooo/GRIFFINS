using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using genshin;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SoundManager soundManager;

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
        
    }
}