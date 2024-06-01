
using TMPro;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public DestinationEvent Point;

    public Vector3 playerPos;
    public Vector3 targetPos;
    public Vector3 forward;

    private void Start()
    {
        playerPos = OnlySingleton.Instance.destinationTransform.position;
    }

    private void Update()
    {
        PositionCheck();
    }

    private void PositionCheck()
    {
        if (Point == null) return;

        targetPos = Point.transform.position;

        if (Point.isCheck)
        {
            GameManager.Instance.uiManager.leftVectorImage.gameObject.SetActive(false);
            GameManager.Instance.uiManager.rightVectorImage.gameObject.SetActive(false);
            return;
        }

        OnlySingleton.Instance.destinationTransform.forward = targetPos - Player.Instance.transform.position;
        OnlySingleton.Instance.destinationTransform.eulerAngles = new Vector3(0, OnlySingleton.Instance.destinationTransform.eulerAngles.y, 0);

        float camY = OnlySingleton.Instance.mainCam.eulerAngles.y;
        float destY = OnlySingleton.Instance.destinationTransform.eulerAngles.y;


        Vector3 directionToB = targetPos - OnlySingleton.Instance.mainCam.position; // A���� B�� ���ϴ� ����
        Vector3 forward = OnlySingleton.Instance.mainCam.forward; // A�� �� ���� ����
        Vector3 right = OnlySingleton.Instance.mainCam.right; // A�� ������ ���� ����

        // ������ ���Ϳ� B�� ���ϴ� ������ ������ ���մϴ�.
        float dotProduct = Vector3.Dot(right, directionToB);

        if (dotProduct > 0)
        {
            GameManager.Instance.uiManager.leftVectorImage.gameObject.SetActive(false);
            GameManager.Instance.uiManager.rightVectorImage.gameObject.SetActive(true);
        }
        else if (dotProduct < 0)
        {
            GameManager.Instance.uiManager.leftVectorImage.gameObject.SetActive(true);
            GameManager.Instance.uiManager.rightVectorImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("B�� A�� �����̳� ���ʿ� �ֽ��ϴ�.");
        }
    }


}
