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

public class TenantEvaluator : MonoBehaviour
{
    public List<GeneratedTenant> allTenants = new List<GeneratedTenant>();

   // public int tickInterval =

    void Start()
    {
        FindObjectOfType<SimulateTickRate>().OnTickUpdate.AddListener(OnTick);
    }

    void OnTick(int tick)
    {

    }
}


public class PickTenantRoot : MonoBehaviour
{
    public static List<GeneratedTenant> results = new List<GeneratedTenant>();

    const int amountOfTenantsAvailable = 4;
    const int amountOfTenantsToPick = 3;
    const string sceneName = "PickTenant";

    public TenantData tenantData;
    public TenantGenerator tenantGenerator;
    public CanvasGroup container;
    public GameObject tennantsContainer;
    public TenantElement template;

    public Action<TenantData.TenantItem> onDone;

    TenantData.TenantItem chosenItem;

    static bool done;

    bool closing;

    public static IEnumerator Show()
    {
        results.Clear();
        done = false;

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
        int count = 0;
        for (int i = 0; i < amountOfTenantsAvailable; i++)
        {
            var instance = Instantiate(template, tennantsContainer.transform, false);
            var data = tenantData.potentialTenants.Random();
            var generatedTenant = tenantGenerator.Generate(data);
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
