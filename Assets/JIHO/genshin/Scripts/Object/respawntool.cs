using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class respawntool : MonoBehaviour
        {[Header("리스폰장소")]

        public Transform destination; // 이동할 목적지 오브젝트(A 오브젝트)

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // 플레이어와 충돌했을 때만 실행
            {
                // 플레이어를 목적지로 순간 이동
                other.transform.position = destination.position;
            }
        }
    }
}
