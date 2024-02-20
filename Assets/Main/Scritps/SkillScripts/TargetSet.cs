using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TargetSet : MonoBehaviour
{
    public float objectViewArea;
    public float interactionViewArea;
    public float enemyViewArea;

    public LayerMask objectTargetMask;
    public LayerMask interactionTargetMask;
    public LayerMask enemyTargetMask;

    public GameObject targetObject;
    public InteractableObject targetInteraction;
    public EnemyController targetEnemy;

    public GameObject grappleTargetObject;

    void Update()
    {
        //GetTarget();
        //ObjectGrappleTarget();
        ObjectTarget();
        EnemyTarget();
        InteractionTarget();
    }

    public void ObjectTarget()
    {
        Collider[] ObjectColliders = Physics.OverlapSphere(transform.position, objectViewArea, objectTargetMask);
        
        if(ObjectColliders.Length > 0)
        {
            float closestAngle = Mathf.Infinity;
            GameObject closestObject = null;

            foreach (Collider collider in ObjectColliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(Camera.main.transform.forward, direction);

                if (angle < closestAngle)
                {
                    if(collider.tag == "useObject")
                    {
                        closestAngle = angle;
                        closestObject = collider.gameObject;
                    }
                }
            }
            targetObject = closestObject;
        }
        else
        {
            targetObject = null;
        }
    }

    public void EnemyTarget()
    {
        Collider[] EnemyColliders = Physics.OverlapSphere(transform.position, enemyViewArea, enemyTargetMask);

        if (EnemyColliders.Length > 0)
        {
            float closestAngle = Mathf.Infinity;
            EnemyController closestEnemy = null;

            foreach (Collider collider in EnemyColliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(Camera.main.transform.forward, direction);

                if (angle <= closestAngle)
                {
                    if (collider.tag == "Enemy")
                    {
                        closestAngle = angle;
                        closestEnemy = collider.GetComponent<EnemyController>();
                    }
                }
            }

            if (targetEnemy != null) targetEnemy.TargetCheck(false);
            targetEnemy = closestEnemy;
            if(targetEnemy != null) targetEnemy.TargetCheck(true);
        }
        else
        {
            targetEnemy = null;
        }
    }

    public void InteractionTarget()
    {
        Collider[] InteractionColliders = Physics.OverlapSphere(transform.position, interactionViewArea, interactionTargetMask);

        if (InteractionColliders.Length > 0)
        {
            float closestAngle = Mathf.Infinity;
            InteractableObject closestInteraction = null;

            foreach (Collider collider in InteractionColliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(Camera.main.transform.forward, direction);

                if (angle < closestAngle)
                {
                    if (collider.tag == "interaction")
                    {
                        closestAngle = angle;
                        closestInteraction = collider.GetComponent<InteractableObject>();
                    }
                }
            }
            if (targetInteraction != null) targetInteraction.transform.GetChild(0).gameObject.SetActive(false);
            targetInteraction = closestInteraction;
            if (targetInteraction != null) targetInteraction.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            if (targetInteraction != null) targetInteraction.transform.GetChild(0).gameObject.SetActive(false);
            targetInteraction = null;
        }
    }


    //public void ObjectGrappleTarget()
    //{
    //    if (Player.Instance.skillData.isGHand) return;
    //    ObjectGrappleTargets.Clear();
    //    grappleTargetObject = null;
    //    Collider[] TargetCollider = Physics.OverlapSphere(transform.position, GviewArea, objectTargetMask);

    //    for (int i = 0; i < TargetCollider.Length; i++)
    //    {
    //        Transform target = TargetCollider[i].transform;
    //        Vector3 direction = target.position - transform.position;
    //        if (Vector3.Dot(direction.normalized, transform.forward) > GetAngle(GviewAngle / 2).z)
    //        {
    //            if (grappleTargetObject == null)
    //            {
    //                if (target.gameObject.tag == "useObject")
    //                    grappleTargetObject = target.gameObject;

    //            }
    //            else if (Vector3.Distance(transform.position, target.position) < Vector3.Distance(transform.position, grappleTargetObject.transform.position))
    //            {
    //                if (target.gameObject.tag == "useObject")
    //                    grappleTargetObject = target.gameObject;
    //            }

    //            ObjectGrappleTargets.Add(target);
    //        }
    //    }
    //}

    //public void GetTarget()
    //{
    //    if (Player.Instance.skillData.isHand) return;
    //    Targets.Clear();
    //    targetGameObject = null;
    //    Collider[] TargetCollider = Physics.OverlapSphere(transform.position, viewArea, objectTargetMask);

    //    for (int i = 0; i < TargetCollider.Length; i++)
    //    {
    //        Transform target = TargetCollider[i].transform;
    //        Vector3 direction = target.position - transform.position;
    //        if (Vector3.Dot(direction.normalized, transform.forward) > GetAngle(viewAngle / 2).z)
    //        {
    //            if (targetGameObject == null)
    //            { 
    //                if(target.gameObject.tag == "useObject")
    //                    targetGameObject = target.gameObject;

    //            }
    //            else if (Vector3.Distance(transform.position, target.position) < Vector3.Distance(transform.position, targetGameObject.transform.position))
    //            {
    //                if (target.gameObject.tag == "useObject")
    //                    targetGameObject = target.gameObject;
    //            }

    //            Targets.Add(target);
    //        }
    //    }
    //}

    public Vector3 GetAngle(float AngleInDegree)
    {
        return new Vector3(Mathf.Sin(AngleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(AngleInDegree * Mathf.Deg2Rad));
    }


    private void OnDrawGizmos()
    {
        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, objectViewArea);
        if(targetObject != null)
            Handles.DrawLine(transform.position, targetObject.transform.position);


        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, enemyViewArea);
        if (targetEnemy != null)
            Handles.DrawLine(transform.position, targetEnemy.transform.position);

        Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, interactionViewArea);
        if (targetInteraction != null)
            Handles.DrawLine(transform.position, targetInteraction.transform.position);
    }
}
