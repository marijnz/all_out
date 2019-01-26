using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public List<AudioClip> clips;

    static AudioPlayer instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audioSource.loop = true;
        audioSource.Play();
    }

    public static void Play(string clip)
    {
        var foundClip = instance.clips.Find(t => t.name == clip);
        if(foundClip == null) Debug.LogWarning("Couldn't find clip for :" + clip);
        AudioSource.PlayClipAtPoint(foundClip, Camera.main.transform.position);
    }
}
