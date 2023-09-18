using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosChecker : MonoBehaviour
{
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool drawGizmo;
    [SerializeField] private LayerMask layer;

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + transform.forward * maxDistance, boxSize);
    }

    public bool IsWall()
    {
        return Physics.BoxCast(transform.position, boxSize, transform.forward, transform.rotation, maxDistance, layer);
    }
}
