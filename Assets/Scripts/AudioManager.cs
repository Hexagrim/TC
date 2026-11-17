using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    public AudioSource SFX;
    public AudioSource chaseMusic;
    public AudioSource backgroundMusic;

    public AudioClip itemDrop;
    public AudioClip footStep;


    bool playChaseMusic = false;
    void Start()
    {
        StartCoroutine(FadeIn(3f,backgroundMusic));
    }

    // Update is called once per frame
    void Update()
    {

        if (playChaseMusic)
        {
            StartCoroutine(FadeIn(1f, chaseMusic));
        }

    }


    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

    //neat ienumerator trick to make smooth fade rather than having to use a counter and a timer

    public IEnumerator FadeIn(float duration , AudioSource audioSource)
    {
        audioSource.volume = 0f;
        audioSource.Play();
        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null ;
        }

        audioSource.volume = 1f;

    }

    
    public IEnumerator FadeOut(float duration, AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null ;

        }
        audioSource.volume = 0f;
        audioSource.Stop();

    }

}
