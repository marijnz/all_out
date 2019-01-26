using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratedTenant
{
    public List<TenantTrait> traits = new List<TenantTrait>();
    public TenantData.TenantItem data;
}


public class PickTenantRoot : MonoBehaviour
{
    public static GeneratedTenant result;

    const int amountOfTenantsToPick = 4;
    const string sceneName = "PickTenant";

    public TenantData tenantData;
    public TenantGenerator tenantGenerator;
    public CanvasGroup container;
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

    void CloseScene()
    {
        container.DOFade(0, .3f).OnComplete(() =>
        {
            SceneManager.UnloadSceneAsync(sceneName);
        });
    }

    void Awake()
    {
        container.alpha = 0;
        container.DOFade(1, .3f);
    }

    void Start()
    {
        for (int i = 0; i < amountOfTenantsToPick; i++)
        {
            var instance = Instantiate(template, tennantsContainer.transform, false);
            var data = tenantData.potentialTenants.Random();
            var generatedTenant = tenantGenerator.Generate(data);
            instance.Init(generatedTenant);
            instance.button.onClick += () =>
            {
                result = generatedTenant;
                CloseScene();
            };
        }

        template.gameObject.SetActive(false);
    }
}
