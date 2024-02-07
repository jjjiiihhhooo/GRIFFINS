using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TargetSet : MonoBehaviour
{
    public float viewArea;
    public float GviewArea;
    [Range(0, 360)]
    public float viewAngle;
    [Range(0, 360)]
    public float GviewAngle;



    public LayerMask targetMask;

    [HideInInspector]
    public List<Transform> Targets = new List<Transform>();
    public List<Transform> ObjectGrappleTargets = new List<Transform>();

    public GameObject targetGameObject;
    public GameObject grappleTargetObject;

    // Update is called once per frame
    void Update()
    {
        GetTarget();
        ObjectGrappleTarget();
    }

    public void ObjectGrappleTarget()
    {
        if (Player.Instance.skillData.isGHand) return;
        ObjectGrappleTargets.Clear();
        grappleTargetObject = null;
        Collider[] TargetCollider = Physics.OverlapSphere(transform.position, GviewArea, targetMask);

        for (int i = 0; i < TargetCollider.Length; i++)
        {
            Transform target = TargetCollider[i].transform;
            Vector3 direction = target.position - transform.position;
            if (Vector3.Dot(direction.normalized, transform.forward) > GetAngle(GviewAngle / 2).z)
            {
                if (grappleTargetObject == null)
                {
                    if (target.gameObject.tag == "useObject")
                        grappleTargetObject = target.gameObject;

                }
                else if (Vector3.Distance(transform.position, target.position) < Vector3.Distance(transform.position, grappleTargetObject.transform.position))
                {
                    if (target.gameObject.tag == "useObject")
                        grappleTargetObject = target.gameObject;
                }

                ObjectGrappleTargets.Add(target);
            }
        }
    }

    public void GetTarget()
    {
        if (Player.Instance.skillData.isHand) return;
        Targets.Clear();
        targetGameObject = null;
        Collider[] TargetCollider = Physics.OverlapSphere(transform.position, viewArea, targetMask);

        for (int i = 0; i < TargetCollider.Length; i++)
        {
            Transform target = TargetCollider[i].transform;
            Vector3 direction = target.position - transform.position;
            if (Vector3.Dot(direction.normalized, transform.forward) > GetAngle(viewAngle / 2).z)
            {
                if (targetGameObject == null)
                { 
                    if(target.gameObject.tag == "useObject")
                        targetGameObject = target.gameObject;
                    
                }
                else if (Vector3.Distance(transform.position, target.position) < Vector3.Distance(transform.position, targetGameObject.transform.position))
                {
                    if (target.gameObject.tag == "useObject")
                        targetGameObject = target.gameObject;
                }

                Targets.Add(target);
            }
        }
    }

    public Vector3 GetAngle(float AngleInDegree)
    {
        return new Vector3(Mathf.Sin(AngleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(AngleInDegree * Mathf.Deg2Rad));
    }


    //private void OnDrawGizmos()
    //{
    //    Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, GviewArea);
    //    Handles.DrawLine(transform.position, transform.position + GetAngle(-GviewAngle / 2) * GviewArea);
    //    Handles.DrawLine(transform.position, transform.position + GetAngle(GviewAngle / 2) * GviewArea);

    //    foreach (Transform Target in ObjectGrappleTargets)
    //    {
    //        Handles.DrawLine(transform.position, Target.position);
    //    }
    //}
}
