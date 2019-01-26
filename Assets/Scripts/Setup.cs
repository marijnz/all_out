using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return PickTenantRoot.Show();

      //  Debug.Log("Picked a : " + PickTenantRoot.results.data.animalName);

        // while no new tenant needed

        yield return SceneManager.LoadSceneAsync("TestRooms1", LoadSceneMode.Additive);
        FindObjectOfType<CharacterSpawnManager>().Spawn(PickTenantRoot.results);

        // on
    }
}
