using UnityEngine;
using System.Collections.Generic;

public class KeySpawner : MonoBehaviour
{
    [Header("Assign Key Prefab")]
    public GameObject keyPrefab;

    [Header("Spawn Points (7 empty GameObjects)")]
    public Transform[] spawnPoints; // assign 7 locations in Inspector

    [Header("Settings")]
    [Range(1, 7)] public int keysToSpawn = 3; // how many keys to spawn at once

    private List<int> usedIndices = new List<int>();

    void Start()
    {
        SpawnKeys();
    }

    void SpawnKeys()
    {
        if (keyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("❌ Missing keyPrefab or spawnPoints in inspector!");
            return;
        }

        // Clear any old keys
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Make sure we don't ask for more keys than spawn points
        int totalKeys = Mathf.Min(keysToSpawn, spawnPoints.Length);
        usedIndices.Clear();

        for (int i = 0; i < totalKeys; i++)
        {
            int randomIndex;

            // pick a random unused spawn point
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            }
            while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            // spawn key at selected point
            Transform spawnPoint = spawnPoints[randomIndex];
            GameObject key = Instantiate(keyPrefab, spawnPoint.position, spawnPoint.rotation, transform);

            key.name = "Key_" + (i + 1);
        }

        Debug.Log($"✅ Spawned {totalKeys} keys at random locations.");
    }
}
