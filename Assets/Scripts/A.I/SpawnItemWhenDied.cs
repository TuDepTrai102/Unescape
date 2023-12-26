using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class SpawnItemWhenDied : MonoBehaviour
    {
        public GameObject[] itemPrefabs;

        public void SpawnRandomItem(Vector3 spawnPosition)
        {
            if (itemPrefabs.Length > 0)
            {
                GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
                GameObject spawnedItem = Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}