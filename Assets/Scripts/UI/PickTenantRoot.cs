using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneratedTenant
{
    public List<TenantTrait> traits = new List<TenantTrait>();
    public TenantData.TenantItem data;
    public int dataId;
}

public class PickTenantRoot : MonoBehaviour
{
    public static List<GeneratedTenant> results = new List<GeneratedTenant>();

    const int amountOfTenantsAvailable = 4;
    const string sceneName = "PickTenant";

    public TenantData tenantData;
    public TenantGenerator tenantGenerator;
    public CanvasGroup container;
    public GameObject tennantsContainer;
    public TenantElement template;
    public TextMeshProUGUI tenantsToPickLabel;

    public Action<TenantData.TenantItem> onDone;

    TenantData.TenantItem chosenItem;

    static bool done;
    bool closing;

    static int amountOfTenantsToPick;

    public static IEnumerator Show(int tenantsToPickVal)
    {
        results.Clear();
        done = false;
        amountOfTenantsToPick = tenantsToPickVal;

        SceneManager.LoadScene("PickTenant", LoadSceneMode.Additive);
        while(!done)
        {
            yield return null;
        }
    }

    IEnumerator CloseScene()
    {
        yield return new WaitForSeconds(.3f);
        container.DOFade(0, .3f).OnComplete(() =>
        {
            SceneManager.UnloadSceneAsync(sceneName);
            done = true;
        });
    }

    void Awake()
    {
        container.alpha = 0;
        container.DOFade(1, .3f);
    }

    void Start()
    {
        if(amountOfTenantsToPick == 1) tenantsToPickLabel.text = "Pick a new tenant!";
        else if(amountOfTenantsToPick == 2) tenantsToPickLabel.text = "Pick two new tenants!";
        else if(amountOfTenantsToPick == 3)  tenantsToPickLabel.text = "Pick three new tenants!";

        int count = 0;
        for (int i = 0; i < amountOfTenantsAvailable; i++)
        {
            var instance = Instantiate(template, tennantsContainer.transform, false);
            var data = tenantData.potentialTenants.Random();
            var generatedTenant = tenantGenerator.Generate(data);
            generatedTenant.dataId = tenantData.potentialTenants.IndexOf(data);
            instance.Init(generatedTenant);
            instance.button.onClick += () =>
            {
                if(!closing)
                {
                    if(!instance.selected)
                    {
                        instance.transform.DOScale(Vector3.one * 1.1f, .3f).SetEase(Ease.OutBack);
                        instance.selected = true;
                        results.Add(generatedTenant);
                        count++;
                        AudioSource.PlayClipAtPoint(data.audioClip, Vector3.zero);
                    }
                    else
                    {
                        instance.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutSine);
                        instance.selected = false;
                        results.Remove(generatedTenant);
                        count--;
                    }
                    if(count == amountOfTenantsToPick)
                    {
                        closing = true;
                        StartCoroutine(CloseScene());
                    }
                }
            };
        }

        template.gameObject.SetActive(false);
    }
}
