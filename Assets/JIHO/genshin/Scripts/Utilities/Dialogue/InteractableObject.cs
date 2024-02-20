using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;



public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string interactorName;
    [SerializeField] private UnityEvent actionOnInteract;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private bool oneTime;

    public string InteractorName { get { return interactorName; } }

    private void Awake()
    {
        nameText.text = interactorName;
    }

    public void OnInteract()
    {
        if (actionOnInteract == null) return;
        actionOnInteract?.Invoke();
        if (oneTime) actionOnInteract = null;
    }
}

