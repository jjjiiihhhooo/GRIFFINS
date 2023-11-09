using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private BulletSpawner bulletSpawner;
    [SerializeField] private CoolTimeManager coolTimeManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Particles particles;

    public InputManager InputManager { get => inputManager; }
    public SoundManager SoundManager { get => soundManager; }
    public BulletSpawner BulletSpawner { get => bulletSpawner; }
    public CoolTimeManager CoolTimeManager { get => coolTimeManager; }
    public UIManager UiManager { get => uiManager; }
    public Particles Particles { get => particles; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
