
using UnityEngine;

public class GrannySpawner : MonoBehaviour
{
    [Header("Granny Prefab (Parent Object)")]
    public GameObject grannyPrefab;

    [Header("Spawn Points (5 Empty GameObjects)")]
    public Transform[] spawnPoints;

    private GameObject spawnedGranny;

    void Start()
    {
        SpawnGrannyAtRandom();
    }

    public void SpawnGrannyAtRandom()
    {
        if (grannyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("❌ Missing grannyPrefab or spawnPoints in Inspector!");
            return;
        }

        // Destroy old Granny (optional if respawning)
        if (spawnedGranny != null)
            Destroy(spawnedGranny);

        // Pick random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenPoint = spawnPoints[randomIndex];

        // Instantiate Granny
        spawnedGranny = Instantiate(
            grannyPrefab,
            chosenPoint.position,
            chosenPoint.rotation
        );

        Debug.Log($"👵 Spawned Granny at: {chosenPoint.name}");
    }
}
