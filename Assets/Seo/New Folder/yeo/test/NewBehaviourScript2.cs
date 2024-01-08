using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace genshin
{
    public class NewBehaviourScript2 : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        public TimelineAsset timeline;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                PlayFromTimeline();
            }
        
        }
        public void PlayFromTimeline()
        {
            // ���ο� timeline�� ����
            playableDirector.Play(timeline);
        }
    }
}
