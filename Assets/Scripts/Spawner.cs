using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject Obstacle;
    [SerializeField] private float minSpawnIntervalInSeconds;
    [SerializeField] private float maxSpawnIntervalInSeconds;

    private Jumper jumper;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        jumper = GetComponentInChildren<Jumper>();
        jumper.OnReset += DestroyAllSpawnedObjects;
        
        StartCoroutine(nameof(Spawn));
    }
    
    private IEnumerator Spawn()
    {
        var spawned = Instantiate(Obstacle, transform.position, transform.rotation, transform);
        spawnedObjects.Add(spawned);
        
        yield return new WaitForSeconds(Random.Range(minSpawnIntervalInSeconds, maxSpawnIntervalInSeconds));
        StartCoroutine(nameof(Spawn));
    }
    private void DestroyAllSpawnedObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(spawnedObjects[i]);
            spawnedObjects.RemoveAt(i);
        }
    }
}
