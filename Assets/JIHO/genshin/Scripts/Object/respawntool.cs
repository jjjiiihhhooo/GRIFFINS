using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class respawntool : MonoBehaviour
        {[Header("���������")]

        public Transform destination; // �̵��� ������ ������Ʈ(A ������Ʈ)

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // �÷��̾�� �浹���� ���� ����
            {
                // �÷��̾ �������� ���� �̵�
                other.transform.position = destination.position;
            }
        }
    }
}
