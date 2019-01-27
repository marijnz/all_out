using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
	    DoBlur(true);
	    foreach (var unhappyTenant in unhappyTenants)
	    {
		    Destroy(unhappyTenant.gameObject, 5f);
	    }

	    StartCoroutine(PickTenants(unhappyTenants.Count));
    }
    IEnumerator PickTenants(int count)
    {
	    yield return PickTenantRoot.Show(count);

	    if(!SceneManager.GetSceneByName("TestRooms1").isLoaded)
	    {
		    yield return SceneManager.LoadSceneAsync("TestRooms1", LoadSceneMode.Additive);
	    }

	    FindObjectOfType<SimulateTickRate>().paused = false;
	    DoBlur(false);
	    FindObjectOfType<CharacterSpawnManager>().Spawn(PickTenantRoot.results);
    }

    void DoBlur(bool doBlur, bool instant = false)
    {
	    var blur = FindObjectOfType<PostprocessingBlur>();
	    if(blur != null)
	    {
		    float duration = instant ? 0 : 0.4f;
		    if(doBlur)
		    {
			    blur.postprocessMaterial.DOFloat(0.07f, "_BlurSize", duration);
		    }
		    else
		    {
			    blur.postprocessMaterial.DOFloat(0f, "_BlurSize", duration);
		    }
	    }
    }
}
