using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickTenantRoot : MonoBehaviour
{
    public static TenantData.TenantItem result;

    const int amountOfTenantsToPick = 4;
    const string sceneName = "PickTenant";

    public TenantData tenantData;

    public GameObject tennantsContainer;
    public TenantElement template;

    public Action<TenantData.TenantItem> onDone;

    TenantData.TenantItem chosenItem;

    public static IEnumerator Show()
    {
        SceneManager.LoadScene("PickTenant", LoadSceneMode.Additive);
        while(result == null)
        {
            yield return null;
        }
    }

    static void CloseScene()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    void Start()
    {
        for (int i = 0; i < amountOfTenantsToPick; i++)
        {
            var instance = Instantiate(template, tennantsContainer.transform, false);
            var data = tenantData.potentialTenants.Random();
            instance.Init(data);
            instance.button.onClick += () =>
            {
                result = data;
                CloseScene();

            };
        }

        template.gameObject.SetActive(false);
    }
}
