using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    // Start is called before the first frame update

    IEnumerator Start()
    {
		yield return PickTenants(3);
    }

    public void RequestNewTenants(List<Tenant> unhappyTenants)
    {
	   StartCoroutine(DoRequestNewTenants(unhappyTenants));
    }

    IEnumerator DoRequestNewTenants(List<Tenant> unhappyTenants)
    {
	    FindObjectOfType<SimulateTickRate>().paused = true;

	    yield return new WaitForSeconds(4f); // wait for emotions to be hidden, kinda
	    foreach (var unhappyTenant in unhappyTenants)
	    {
		    Destroy(unhappyTenant.gameObject, 5f);
	    }

	    StartCoroutine(PickTenants(unhappyTenants.Count));
    }
    IEnumerator PickTenants(int count)
    {
	    yield return PickTenantRoot.Show(count);

	    yield return SceneManager.LoadSceneAsync("TestRooms1", LoadSceneMode.Additive);
	    FindObjectOfType<SimulateTickRate>().paused = false;
	    FindObjectOfType<CharacterSpawnManager>().Spawn(PickTenantRoot.results);
    }
}
