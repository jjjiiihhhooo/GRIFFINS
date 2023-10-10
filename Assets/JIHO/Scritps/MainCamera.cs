using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 scopePosition;

    [SerializeField] private LayerMask layer;
    [SerializeField] private RaycastHit hit;
    [SerializeField] private Ray ray;

    [SerializeField] private float rayDistance;
    [SerializeField] private float mouseSpeed;

    [SerializeField] private LineRenderer line;
    [SerializeField] private Camera thisCam;

    public Vector3 rayDir;
    public Vector3 mousePos;

    public Transform cameraArm;



}
