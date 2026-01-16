using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    private SpawnPoint[] _spawnPoints;

    private void Awake()
    {
        Instance = this;
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;
    }
    
}
