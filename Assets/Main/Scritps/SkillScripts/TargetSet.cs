using UnityEditor;
using UnityEngine;


public class TargetSet : MonoBehaviour
{
    public float objectViewArea;
    public float interactionViewArea;
    public float enemyViewArea;
    public float enemyViewArea_2;

    public LayerMask objectTargetMask;
    public LayerMask interactionTargetMask;
    public LayerMask enemyTargetMask;

    public GameObject targetObject;
    public InteractableObject targetInteraction;
    public EnemyController targetEnemy;
    public EnemyController attackEnemy;

    public GameObject grappleTargetObject;

    public Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        //GetTarget();
        //ObjectGrappleTarget();
        ObjectTarget();
        EnemyTarget(player.currentCharacter.targetArea);
        AttackTarget(player.currentCharacter.attackArea);
        InteractionTarget();
    }

    public void ObjectTarget()
    {
        Collider[] ObjectColliders = Physics.OverlapSphere(transform.position, objectViewArea, objectTargetMask);

        if (ObjectColliders.Length > 0)
        {
            float closestAngle = Mathf.Infinity;
            GameObject closestObject = null;

            foreach (Collider collider in ObjectColliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(Camera.main.transform.forward, direction);

                if (angle < closestAngle)
                {
                    if (collider.tag == "useObject")
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

    public void EnemyTarget(float targetArea)
    {
        Collider[] EnemyColliders = Physics.OverlapSphere(transform.position, targetArea, enemyTargetMask);

        if (EnemyColliders.Length > 0)
        {
            float closestAngle = Mathf.Infinity;
            EnemyController closestEnemy = null;

            foreach (Collider collider in EnemyColliders)
            {
                //Vector3 direction = collider.transform.position - transform.position;
                //float angle = Vector3.Angle(Camera.main.transform.forward, direction);

                //if (angle <= closestAngle)
                //{
                //    if (collider.tag == "Enemy")
                //    {
                //        closestAngle = angle;
                //        closestEnemy = collider.GetComponent<EnemyController>();
                //    }
                //}

                if (closestEnemy == null) closestEnemy = collider.GetComponent<EnemyController>();
                else
                {
                    if (Vector3.Distance(collider.transform.position, player.transform.position) < Vector3.Distance(closestEnemy.transform.position, player.transform.position))
                    {
                        closestEnemy = collider.GetComponent<EnemyController>();
                    }
                }
            }

            if (player.currentCharacter.target == null)
            {
                targetEnemy = closestEnemy;
            }

        }
        else
        {
            if (player.currentCharacter.target != null)
            {
                player.currentCharacter.target = null;

            }

            if (targetEnemy != null)
            {
                targetEnemy = null;
            }
        }
    }

    public void AttackTarget(float attackArea)
    {
        Collider[] EnemyColliders = Physics.OverlapSphere(transform.position, attackArea, enemyTargetMask);

        if (EnemyColliders.Length > 0)
        {
            float closestAngle = Mathf.Infinity;
            EnemyController closestEnemy = null;

            foreach (Collider collider in EnemyColliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(Camera.main.transform.forward, direction);

                if (angle < closestAngle)
                {
                    if (collider.tag == "Enemy")
                    {
                        closestAngle = angle;
                        closestEnemy = collider.GetComponent<EnemyController>();
                    }
                }
            }

            //if (attackEnemy != null) attackEnemy.TargetCheck(false);
            attackEnemy = closestEnemy;
            //if (attackEnemy != null) attackEnemy.TargetCheck(true);
        }
        else
        {
            attackEnemy = null;
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
            targetInteraction = closestInteraction;
            if (targetInteraction != null)
            {
                if (targetInteraction.GetReady())
                    Player.Instance.SetActiveInteraction(true, targetInteraction.InteractorName);
            }
        }
        else
        {
            if (targetInteraction != null) Player.Instance.SetActiveInteraction(false);
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


    
}
