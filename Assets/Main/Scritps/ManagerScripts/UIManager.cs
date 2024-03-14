using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject grappleImage;
    public GameObject normalImage;
    public GameObject crossHair;

    public Image device_Image;
    public Image skill_Image;

    public Slider playerHp;
    public Slider playerBackHp;
    public Slider bossBackHp;
    public Slider bossHp;
    public Slider bossTiming;

    public DOTweenAnimation playerHpDot;
    public DOTweenAnimation bossHpDot;

    public void Init()
    {

    }
}
