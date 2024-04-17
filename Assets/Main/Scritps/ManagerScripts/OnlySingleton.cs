using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlySingleton : MonoBehaviour
{
    public static OnlySingleton Instance;

    public CinemachineShake camShake;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
