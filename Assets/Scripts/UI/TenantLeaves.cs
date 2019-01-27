using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TenantLeaves : MonoBehaviour
{
    public TextMeshProUGUI speechLabel;
    public TenantLeavingSentences tenantLeavingSentences;
    public GameObject tenantImagesContainer;
    public Button fullscreenClick;

    static bool isDone;

    static Tenant tenant;
    static Tenant otherTenant;

    public static IEnumerator Show(Tenant t, Tenant otherT)
    {
        tenant = t;
        otherTenant = otherT;
        SceneManager.LoadScene("TenantLeaves", LoadSceneMode.Additive);
        while(!isDone)
        {
            yield return null;
        }
        isDone = false;
    }

    void Start()
    {
        var text = tenantLeavingSentences.texts.Random();
        speechLabel.text = text.Replace("OTHERANIMAL", otherTenant.generatedTenant.data.animalName.ToLower());
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
            isDone = true;
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
