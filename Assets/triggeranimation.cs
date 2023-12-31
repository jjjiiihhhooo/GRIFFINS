using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class triggeranimation : MonoBehaviour
    {
        public Animator animator; // ����� �ִϸ�����
        
        private void OnTriggerEnter(Collider other)
        {
            // �浹�� ������Ʈ�� Player �±׸� ������ �ִٸ� �ִϸ������� Ʈ���Ÿ� Ȱ��ȭ�մϴ�.
            if (other.CompareTag("Player"))
            {
                animator.SetTrigger("trigger"); // ���⼭ "YourTriggerName"�� �ִϸ������� Ʈ���� �̸��Դϴ�.
            }
        }
    }
}
