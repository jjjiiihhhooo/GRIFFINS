
using UnityEngine;

public class EnemySkilldata : MonoBehaviour
{
    public void Test(EnemyController enemyController)
    {
        Vector3 lowestPoint = new Vector3(enemyController.enemy.fireTransform.position.x, enemyController.enemy.fireTransform.position.y, enemyController.enemy.fireTransform.position.z);



        float grapplePointRelativeYPos = Player.Instance.transform.position.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + 4f;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = 4f;
        float randX = Random.Range(-enemyController.enemy.rangeX, enemyController.enemy.rangeX);
        float randZ = Random.Range(-enemyController.enemy.rangeZ, enemyController.enemy.rangeZ);

        Vector3 velocity = Vector3.zero;
        velocity = CalculateJumpVelocity(enemyController.enemy.fireTransform.position, new Vector3(enemyController.enemy.fireTransform.position.x + randX, Player.Instance.transform.position.y, enemyController.enemy.fireTransform.position.z + randZ), highestPointOnArc); ;
        GameObject temp = Instantiate(enemyController.enemy.obj, enemyController.enemy.fireTransform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().velocity = velocity;
    }









    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight) //목표 위치까지 포물선 trajectoryHeight 높이 추가
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
          + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return (velocityXZ + velocityY);
    }

}
