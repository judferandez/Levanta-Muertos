using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] int checkPointSpawnDelayMax = 25;
    [SerializeField] int checkPointSpawnDelayMin = 20;
    [SerializeField] int powerUpSpawnDelayMax = 30;
    [SerializeField] int powerUpSpawnDelayMin = 10;
    [SerializeField] int itemAvaibleTimeMax = 6;
    [SerializeField] int itemAvaibleTimeMin = 4;
    [SerializeField] float spawnRadius = 0.5f;
    [SerializeField] GameObject checkpointPrefab;
    [SerializeField] GameObject[] powerUpPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCheckpointRutine());
        StartCoroutine(SpawnPowerUpRutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCheckpointRutine()
    {
        while (true)
        {
            int randomTime = Random.Range(checkPointSpawnDelayMin, checkPointSpawnDelayMax);
            yield return new WaitForSeconds(randomTime);
            Vector2 randomPosition = transform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
            var checkpointCreated = Instantiate(checkpointPrefab, randomPosition, Quaternion.identity);
            StartCoroutine(DestroyItemRutine(checkpointCreated));
        }
    }

    IEnumerator SpawnPowerUpRutine()
    {
        while (true)
        {
            int randomTime = Random.Range(powerUpSpawnDelayMin, powerUpSpawnDelayMax);
            yield return new WaitForSeconds(randomTime);
            Vector2 randomPosition = transform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
            int random = Random.Range(0, powerUpPrefab.Length);
            var powerUpCreated = Instantiate(powerUpPrefab[random], randomPosition, Quaternion.identity);
            StartCoroutine(DestroyItemRutine(powerUpCreated));
        }
    }

    IEnumerator DestroyItemRutine(GameObject powerUpCreated)
    {
        while (true)
        {
            int randomTime = Random.Range(itemAvaibleTimeMin, itemAvaibleTimeMax);
            yield return new WaitForSeconds(randomTime);
            Destroy(powerUpCreated);
        }
    }
}
