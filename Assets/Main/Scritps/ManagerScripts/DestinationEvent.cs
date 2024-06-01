using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestinationEvent : MonoBehaviour
{
    public bool isCheck;
    [SerializeField] private Renderer Srenderer;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        Srenderer = GetComponentInChildren<Renderer>();

        GameManager.Instance.destinationManager.Point = this;
    }

    private void Update()
    {
        Check();
        transform.LookAt(Camera.main.transform);
    }

    private void Check()
    {
        float temp;


        float tempA = Vector3.Distance(Player.Instance.transform.position, transform.position);
        text.text = tempA.ToString("F1") + "m";

        if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 10)
        {
            temp = Vector3.Distance(transform.position, Player.Instance.transform.position) / 10;
        }
        else
        {
            temp = 1f;
        }
       
        transform.localScale = new Vector3(temp, temp, temp);

        if (IsVisibleCamera(Camera.main, Srenderer)) isCheck = true;
        else isCheck = false;
    }

    private bool IsVisibleCamera(Camera camera, Renderer _renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }
}
