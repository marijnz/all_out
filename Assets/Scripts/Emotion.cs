using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Emotion : MonoBehaviour
{
    public SpriteRenderer happy;
    public SpriteRenderer angry;

    public static void Show(Vector3 position, bool isHappy)
    {
        var emotion = Instantiate(Resources.Load<Emotion>("Emotion"));
        emotion.transform.position = position;

        emotion.StartCoroutine(emotion.DoShow(isHappy));
    }

    IEnumerator DoShow(bool isHappy)
    {
        happy.gameObject.SetActive(isHappy);
        angry.gameObject.SetActive(!isHappy);

        transform.DOMoveY(transform.position.y + 2, 4);
        yield return new WaitForSeconds(2.5f);
        if(isHappy) this.happy.DOFade(0, 1f);
        else angry.DOFade(0, 1f);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
