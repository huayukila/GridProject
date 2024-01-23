using UnityEngine;

public class WaveSpawn : MonoBehaviour
{
    public int WaveSize;
    public GameObject EnemyPrefab;
    public float EnemyInterval;
    public Transform spawnPoint;
    public float startTime;
    public Transform[] WayPoints;
    private int enemyCount;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", startTime, EnemyInterval);
    }

    private void Update()
    {
        if (enemyCount == WaveSize) CancelInvoke("SpawnEnemy");
    }

    private void SpawnEnemy()
    {
        enemyCount++;
        var enemy = Instantiate(EnemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().waypoints = WayPoints;
    }
}