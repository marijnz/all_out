﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TenantLeaves : MonoBehaviour
{
    const string sceneName = "TenantLeaves";
    public TextMeshProUGUI speechLabel;
    public TenantLeavingSentences tenantLeavingSentences;
    public GameObject tenantImagesContainer;
    public Button fullscreenClick;
    public CanvasGroup container;


    static bool isDone;

    static Tenant tenant;
    static Tenant otherTenant;

    public static IEnumerator Show(Tenant t, Tenant otherT)
    {
        tenant = t;
        otherTenant = otherT;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        while(!isDone)
        {
            yield return null;
        }
        isDone = false;
    }

    IEnumerator CloseScene()
    {
        yield return new WaitForSeconds(.3f);
        container.DOFade(0, .3f).OnComplete(() =>
        {
            SceneManager.UnloadSceneAsync(sceneName);
            isDone = true;
        });
    }

    void Start()
    {
        container.alpha = 0;
        container.DOFade(1, .3f);

        var text = tenantLeavingSentences.texts.Random();
        text = text.Replace("OTHERANIMAL", otherTenant.generatedTenant.data.animalName.ToLower());
        speechLabel.text = text.Replace("THISANIMAL", tenant.generatedTenant.data.animalName.ToLower());

        int index = 0;
        foreach (Transform t in tenantImagesContainer.transform)
        {
            var isTenant = tenant.generatedTenant.dataId == index;
            isTenant = true; // todo remove when added more animals
            t.gameObject.SetActive(isTenant);
            index++;
        }

        fullscreenClick.onClick.AddListener(() =>
        {
            StartCoroutine(CloseScene());
            fullscreenClick.enabled = false;
        });

        fullscreenClick.enabled = false;
        StartCoroutine(EnableClickingAway());
    }

    IEnumerator EnableClickingAway()
    {
        yield return new WaitForSeconds(.5f);
        fullscreenClick.enabled = true;
    }
}