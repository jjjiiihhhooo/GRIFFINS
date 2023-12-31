using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class triggeranimation : MonoBehaviour
    {
        public Animator animator; // 사용할 애니메이터
        
        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 오브젝트가 Player 태그를 가지고 있다면 애니메이터의 트리거를 활성화합니다.
            if (other.CompareTag("Player"))
            {
                animator.SetTrigger("trigger"); // 여기서 "YourTriggerName"은 애니메이터의 트리거 이름입니다.
            }
        }
    }
}
