using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class UI_Interaction : MonoBehaviour
{
    [SerializeField] private GameObject interactionObject;
    [SerializeField] private TextMeshProUGUI textmesh;

    private Transform targetObject;

    private void Start()
    {
        interactionObject.SetActive(false);
    }

    public void SetInteractionUI(InteractableObject interactable)
    {
        interactionObject.SetActive(true);
        //targetObject = interactable.NamePosition;
        textmesh.text = interactable.InteractorName;
    }

    public void Disable()
    {
        interactionObject.SetActive(false);
    }

    private void Update()
    {
        if (interactionObject.activeInHierarchy)
        {
            interactionObject.transform.position = Camera.main.WorldToScreenPoint(targetObject.transform.position);
        }
    }
}


