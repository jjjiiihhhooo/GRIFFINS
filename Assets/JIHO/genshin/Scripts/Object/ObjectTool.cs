using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace genshin
{
    public class ObjectTool : MonoBehaviour
    {
        [Header("---------필요 정보---------")]
        [Header("목표 횟수")]
        public int completeCollisionCount;
        [Header("조건 갯수")]
        public int boolIndex;
        [Header("대쉬 방향")]
        public ObjectForward forward;
        [Header("대쉬 속도")]
        public float speed;
        [Header("목표 타겟")]
        public Transform targetTransform;

        private int currentCollisionCount;
        private int currentBoolIndex;

        private bool currentIsDash;
        private bool currentIsCollision;

        private Rigidbody rigid;

        [Header("---------조건 타입---------")]
        [Header("대쉬 상태")]
        public bool isDash;
        [Header("맞은 횟수")]
        public bool isCollsion;

        [Header("---------기믹 타입---------")]
        [Header("파괴 기믹")]
        public bool actionDestroy;
        [Header("대쉬 기믹")]
        public bool actionDash;
        [Header("중력 기믹")]
        public bool actionGravity;
        [Header("이동 기믹")]
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
