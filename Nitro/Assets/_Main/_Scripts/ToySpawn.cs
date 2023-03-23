using System.Collections.Generic;
using UnityEngine;

namespace Synith
{
    public class ToySpawn : MonoBehaviour
    {
        [SerializeField] Vector3 spawnAreaSize;
        [SerializeField] float spawnInterval = 5f;
        [SerializeField] GameObject[] toyPrefabs;

        void Start()
        {
            InvokeRepeating(nameof(SpawnToy), spawnInterval, spawnInterval);
        }

        void SpawnToy()
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject toyPrefab = GetRandomToyPrefab();
            Instantiate(toyPrefab, spawnPosition, Quaternion.identity);
        }

        Vector3 GetRandomSpawnPosition()
        {
            float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float y = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
            float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

            Vector3 spawnPosition = new Vector3(x, y, z) + transform.position;

            return spawnPosition;
        }

        GameObject GetRandomToyPrefab()
        {
            int randomIndex = Random.Range(0, toyPrefabs.Length);
            return toyPrefabs[randomIndex];
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, spawnAreaSize);
        }
    }
}