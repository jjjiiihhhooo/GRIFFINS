using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TargetSet : MonoBehaviour
{
    public float viewArea;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;

    [HideInInspector]
    public List<Transform> Targets = new List<Transform>();

    public GameObject targetGameObject;

    // Update is called once per frame
    void Update()
    {
        GetTarget();
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
    //    Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, viewArea);
    //    Handles.DrawLine(transform.position, transform.position + GetAngle(-viewAngle / 2) * viewArea);
    //    Handles.DrawLine(transform.position, transform.position + GetAngle(viewAngle / 2) * viewArea);

    //    foreach (Transform Target in Targets)
    //    {
    //        Handles.DrawLine(transform.position, Target.position);
    //    }
    //}
}
