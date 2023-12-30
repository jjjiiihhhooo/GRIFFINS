using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace genshin
{
    public class ObjectTool : MonoBehaviour
    {
        [Header("---------�ʿ� ����---------")]
        [Header("��ǥ Ƚ��")]
        public int completeCollisionCount;
        [Header("���� ����")]
        public int boolIndex;
        [Header("�뽬 ����")]
        public ObjectForward forward;
        [Header("�뽬 �ӵ�")]
        public float speed;
        [Header("��ǥ Ÿ��")]
        public Transform targetTransform;

        private int currentCollisionCount;
        private int currentBoolIndex;

        private bool currentIsDash;
        private bool currentIsCollision;

        private Rigidbody rigid;

        [Header("---------���� Ÿ��---------")]
        [Header("�뽬 ����")]
        public bool isDash;
        [Header("���� Ƚ��")]
        public bool isCollsion;

        [Header("---------��� Ÿ��---------")]
        [Header("�ı� ���")]
        public bool actionDestroy;
        [Header("�뽬 ���")]
        public bool actionDash;
        [Header("�߷� ���")]
        public bool actionGravity;
        [Header("�̵� ���")]
        public bool actionMove;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        private void BoolCheck(Collider collider)
        {
            if(isDash)
            {
                if(collider.transform.GetComponentInParent<Player>().movementStateMachine.GetCurrentStateType() == typeof(PlayerDashingState) && !currentIsDash)
                {
                    currentIsDash = true;
                    currentBoolIndex++;
                }
            }

            if(isCollsion)
            {
                currentCollisionCount++;

                if (completeCollisionCount == currentCollisionCount && !currentIsCollision)
                {
                    currentIsCollision = true;
                    currentBoolIndex++;
                }
            }

            if(boolIndex == currentBoolIndex)
            {
                Action(collider);
                currentBoolIndex = 0;
            }
        }

        private void Action(Collider collider)
        {
            if(actionDash)
            {
                ActionDash(collider);
                return;
            }
            else if(actionDestroy)
            {
                ActionDestroy();
                return;
            }
            else if(actionGravity)
            {
                ActionGravity();
                return;
            }
            else if(actionMove)
            {
                ActionMove();
                return;
            }
        }



        private void ActionDestroy()
        {
            Destroy(this.gameObject);
        }

        private void ActionDash(Collider collider)
        {
            Player player = collider.transform.GetComponentInParent<Player>();
            player.Data.AirborneData.ObjectDashData.Direction = forward.forward;
            player.Data.AirborneData.ObjectDashData.SpeedModifier = speed;

            player.movementStateMachine.ChangeState(player.movementStateMachine.ObjectDashState);
        }

        private void ActionGravity()
        {
            rigid.useGravity = true;
            rigid.constraints = ~RigidbodyConstraints.FreezeAll;
        }

        private void ActionMove()
        {
            StartCoroutine(ActionMoveCor());
        }

        private IEnumerator ActionMoveCor()
        {
            while (Vector3.Distance(transform.position, targetTransform.position) > 0.3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.tag == "AttackCol") BoolCheck(collider);

        }
    }
}
