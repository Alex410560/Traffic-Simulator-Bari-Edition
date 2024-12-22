using System.Collections.Generic;
using UnityEngine;

public class PedestrianGenerator : MonoBehaviour
{
    [Header("Prefab dei Pedoni")]
    public List<GameObject> pedestrianPrefabs;

    [Header("Configurazione")]
    public int maxPedestrians = 10;
    public float spawnInterval = 2f;

    private List<Transform> spawnPoints = new List<Transform>();
    private int currentPedestrianCount = 0; 

    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        StartCoroutine(SpawnPedestrians());
    }

    private System.Collections.IEnumerator SpawnPedestrians()
    {
        while (currentPedestrianCount < maxPedestrians)
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject randomPrefab = pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Count)];

            Instantiate(randomPrefab, randomSpawnPoint.position, Quaternion.identity);

            currentPedestrianCount++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
