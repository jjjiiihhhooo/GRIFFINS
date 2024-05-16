using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeContainer : MonoBehaviour
{
    public Notice[] notices;

    public void StartNotice()
    {
        GameManager.Instance.guideManager.SetNotice(notices);

    }
}
