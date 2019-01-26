using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterSpawnManager : MonoBehaviour
{
    public List<GameObject> characters;            // The character prefabs to be spawned.
    public List<Transform> spawnPoints;         // An array of the spawn points this enemy can spawn from.
    

    void Start()
    {
        // Call the Spawn function.
        Invoke("Spawn", 0);
    }

    void Spawn()
    {
        foreach(GameObject character in characters)
        {
            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);


            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            Instantiate(character, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

            spawnPoints.RemoveAt(spawnPointIndex);
        }      
    }
}


