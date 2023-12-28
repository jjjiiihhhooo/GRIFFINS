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
        private Vector3 targetPos;
        private Vector3 startPos;
        private GameObject parent_object;

        private bool currentIsDash;
        private bool currentIsCollision;
        private bool currentIsRepeat;

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
        [Header("�̵��ݺ�����")]
        public bool repeatMove;

        private void Awake()
        {
            rigid = GetComponentInParent<Rigidbody>();
            parent_object = transform.parent.gameObject;
            startPos = parent_object.transform.position;
            targetPos = startPos;
        }

        private void DataReset()
        {
            currentIsDash = false;
            currentIsCollision = false;
            currentCollisionCount = 0;
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

                if (completeCollisionCount <= currentCollisionCount && !currentIsCollision)
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
            Destroy(parent_object);
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
            DataReset();
            StopCoroutine(ActionMoveCor());
            StartCoroutine(ActionMoveCor());
        }

        private IEnumerator ActionMoveCor()
        {
            if (repeatMove)
            {
                if (!currentIsRepeat)
                {
                    targetPos = targetTransform.position;
                }
                else
                {
                    targetPos = startPos;
                }
                currentIsRepeat = !currentIsRepeat;
            }
            else
            {
                targetPos = targetTransform.position;
            }

            while (Vector3.Distance(parent_object.transform.position, targetPos) > 0.3f)
            {
                parent_object.transform.position = Vector3.MoveTowards(parent_object.transform.position, targetPos, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }

        private void OnTriggerEnter(Collider collider)
        {

            if (collider.tag == "AttackCol") BoolCheck(collider); 
        }
    }
}
