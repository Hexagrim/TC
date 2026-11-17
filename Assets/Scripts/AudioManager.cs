using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    public AudioSource SFX;
    public AudioSource JSFX;
    public AudioSource chaseMusic;
    public AudioSource backgroundMusic;

    public AudioClip itemDrop;
    public AudioClip footStep;
    public AudioClip doorSound;
    public AudioClip jumpScare;
    public bool playChaseMusic = false;

    void Start()
    {
        StartCoroutine(FadeIn(10f,backgroundMusic,0.5f));
    }

    // Update is called once per frame
    void Update()
    {

        
    }


    public void PlaySFX(AudioClip clip)
    {
        SFX.pitch = Random.Range(0.7f, 1.3f);
        SFX.PlayOneShot(clip);
    }

    //neat ienumerator trick to make smooth fade rather than having to use a counter and a timer

    public IEnumerator FadeIn(float duration , AudioSource audioSource , float maxVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();
        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null ;
        }

        audioSource.volume = maxVolume;

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
    public void PlayJumpscare()
    {
        JSFX.PlayOneShot(jumpScare);

    }

    public void playChase()
    {
        if (playChaseMusic) return;
        StartCoroutine(FadeIn(0.5f, chaseMusic, 1f));
        playChaseMusic = true;
    }
    public void stopChase()
    {
        if (!playChaseMusic) return;
        StartCoroutine(FadeOut(1f, chaseMusic));
        playChaseMusic = false;
    }
}
