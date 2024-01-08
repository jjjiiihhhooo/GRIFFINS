using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace genshin
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        public TimelineAsset timeline;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                PlayFromTimeline();
            }
        
        }
        public void PlayFromTimeline()
        {
            // 새로운 timeline을 시작
            playableDirector.Play(timeline);
        }
    }
}
