using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public LayerMask interactableLayermask;

    public void PhysicsUpdate()
    {
        Vector3 counterCamera = Vector3.ProjectOnPlane(transform.position - Camera.main.transform.position, Vector3.up).normalized;
        Ray interactionRay = new Ray(transform.position + Vector3.up, counterCamera);
        Debug.DrawRay(interactionRay.origin, interactionRay.origin + counterCamera * interactionRange);
        var rHits = Physics.RaycastAll(interactionRay, interactionRange, interactableLayermask);

        if (rHits.Length != 0)
        {
            UI_Interaction interactionUI = FindObjectOfType<UI_Interaction>();
            var targetInteraction = rHits[0].collider.GetComponent<InteractableObject>();

            if (interactionUI != null && targetInteraction != null)
            {
                interactionUI.SetInteractionUI(targetInteraction);

                //if (Input.GetKeyDown(KeyCode.F)) targetInteraction.OnInteract();
            }
        }
        else
        {
            UI_Interaction interactionUI = FindObjectOfType<UI_Interaction>();
            if (interactionUI != null)
                interactionUI.Disable();
        }
    }


}

