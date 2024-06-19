using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public Vector3 position;
    public GameObject wave;
}

public class EnemyManager : MonoBehaviour
{
    public EnemyWave[] enemyWaves;

    public GameObject curWaveObject;

    public void SpawnWave(int index)
    {
        curWaveObject = Instantiate(enemyWaves[index].wave, enemyWaves[index].position, Quaternion.identity);
    }
}
