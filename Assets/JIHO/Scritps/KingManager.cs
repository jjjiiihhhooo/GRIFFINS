using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace genshin
{
    public enum Game_Volume
    {
        data_Binding, 
        Spawn_Obj, 
        CutScene,
        Tutorial,
        GameStart,
        Pause,
    }

    public enum Tutorial
    {
        Move, 
        Jump,
        Atack,
        ChatNpc1,
        End,
    }

    public class KingManager 
    {
        //편지지 delagte / event 편지내용 
        public delegate void Game_Volume_Event(Game_Volume game); //편지지 이벤트가 들어갈 델리게이트 
        public static event Game_Volume_Event OnGame_Volume_Event;// ~~한것들을 여기에 담겠따
                                                                  // 
        public static void OnSend_GameEvent(Game_Volume game)//우체부
        {
            OnGame_Volume_Event.Invoke(game);
        }

        //튜토리얼전용
        public delegate void Game_tutoria_Event(Tutorial tutorial);
        public static event Game_tutoria_Event OnGame_Tutorial;

        public static void OnSend_Tutorial_GameEvent(Tutorial tutorial)
        {
            OnGame_Tutorial.Invoke(tutorial);
        }
    }

    class GameManager : MonoBehaviour
    {
        int Gameindex = 0;
        private void Awake()
        {
            KingManager.OnGame_Volume_Event += Game;
            KingManager.OnGame_Tutorial += Tutorials;
        }

        private void Tutorials(Tutorial tutorial)
        {
            switch (tutorial)
            {
                case Tutorial.Move:
                    break;
                case Tutorial.Jump:
                    break;
                case Tutorial.Atack:
                    break;
                case Tutorial.ChatNpc1:
                    break;
                case Tutorial.End:
                    break;
                default:
                    break;
            }
        }

        private void TutorialStart()
        {
            while (Gameindex > 4)
            {
                if (Gameindex == 0)
                {
                    KingManager.OnSend_Tutorial_GameEvent(Tutorial.Atack); // initGAme이호출 
                    Gameindex += 1;
                }
                else if (Gameindex == 1)
                {
                    KingManager.OnSend_Tutorial_GameEvent(Tutorial.Atack); // Spwan_Game이호출 
                    Gameindex += 1;
                }
                else if (Gameindex == 2)
                {
                    KingManager.OnSend_Tutorial_GameEvent(Tutorial.Atack); // Spwan_Game이호출 
                    Gameindex += 1;
                }
            }
            Gameindex = -1;
        }

        private void Start()
        {
            GameStart();
        }

        //이벤트가 받았을때 
        private void Game(Game_Volume game)
        {
            switch (game)
            {
                case Game_Volume.data_Binding:
                    init_Game();
                    break;
                case Game_Volume.Spawn_Obj:
                    Spwan_Game();
                    break;
                case Game_Volume.GameStart:
                    break;
                case Game_Volume.Tutorial:
                    TutorialStart();
                    break;
                case Game_Volume.Pause:
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            if (Gameindex == 3)
            {
                KingManager.OnSend_GameEvent(Game_Volume.GameStart); // Spwan_Game이호출 
            }

            if (Gameindex == 3)
            {
                KingManager.OnSend_GameEvent(Game_Volume.Pause);
            }
        }

        private void GameStart()
        {
            while (Gameindex > 4)
            {
                if (Gameindex == 0)
                {
                    KingManager.OnSend_GameEvent(Game_Volume.data_Binding); // initGAme이호출 
                    Gameindex += 1;
                }
                else if(Gameindex == 1)
                {
                    KingManager.OnSend_GameEvent(Game_Volume.Spawn_Obj); // Spwan_Game이호출 
                    Gameindex += 1;
                }
                else if (Gameindex == 2)
                {
                    KingManager.OnSend_GameEvent(Game_Volume.CutScene); // Spwan_Game이호출 
                    Gameindex += 1;
                }
            }
            Gameindex = -1;
        }

        public void init_Game()
        {
            // 초기화 관련 함수 내용 어쩌고~~
        }

        public void Spwan_Game()
        {

        }
    }

    class CutScene
    {
        public void init()
        {
            KingManager.OnGame_Volume_Event += Game;
        }

        private void Game(Game_Volume game)
        {
            switch (game)
            {
                case Game_Volume.CutScene:
                    CutScenestart();
                    break;
            }
        }

        private void CutScenestart()
        {

        }
    }

    class boss
    {
        public void init()
        {
            KingManager.OnGame_Volume_Event += Game;
        }
        private void Game(Game_Volume game)
        {
            switch (game)
            {
                case Game_Volume.Pause:
                    pasue_boss();
                    break;
            }
        }

        public void pasue_boss()
        {
            //보스멈충
        }
    }

    class Reawrad
    {
        public void init()
        {
            KingManager.OnGame_Volume_Event += Game;
        }

        private void Game(Game_Volume game)
        {
            switch (game)
            {
                case Game_Volume.Pause:
                    pasue_reward();
                    break;
            }
        }

        private void pasue_reward()
        {
            //보상 멈춤
        }
    }
}
