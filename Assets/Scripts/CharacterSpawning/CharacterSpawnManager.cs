using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterSpawnManager : MonoBehaviour
{
    public Tenant tenantPrefab;

    public List<Transform> spawnPoints;         // An array of the spawn points this enemy can spawn from.

    public void Spawn(List<GeneratedTenant> results)
    {
        StartCoroutine(DoSpawn(results));
    }

    IEnumerator DoSpawn(List<GeneratedTenant> results)
    {
        yield return new WaitForSeconds(.5f);
        foreach(var character in results)
        {
            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);


            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            Instantiate(tenantPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            tenantPrefab.spriteRenderer.sprite = character.data.image;

            spawnPoints.RemoveAt(spawnPointIndex);
            yield return new WaitForSeconds(.5f);
        }
    }
}


