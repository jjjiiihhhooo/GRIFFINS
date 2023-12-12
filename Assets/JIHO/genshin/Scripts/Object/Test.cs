using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace genshin
{
    public class Test : MonoBehaviour
    {
        public Transform forward_obj;
        public ObjectForward objForward;
        public float speed;

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.tag == "Player")
            {

                collider.GetComponent<Player>().Data.AirborneData.ObjectDashData.Direction = objForward.forward;
                collider.GetComponent<Player>().Data.AirborneData.ObjectDashData.SpeedModifier = speed;

                collider.GetComponent<Player>().movementStateMachine.ChangeState(collider.GetComponent<Player>().movementStateMachine.ObjectDashState);
            }
        }
    }
}
