using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return PickTenantRoot.Show();

        Debug.Log("Picked a : " + PickTenantRoot.result.data.animalName);
    }
}
