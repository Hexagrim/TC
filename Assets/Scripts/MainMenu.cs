using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Animator Anim;
    public Animator tAnim;

    public AudioSource bgMusic;
    bool playing;
    void Start()
    {
        Anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        
    }

    public void Play()
    {
        if (playing) return;

        playing = true;

        StartCoroutine(FadeOut(1f, bgMusic));
        StartCoroutine(StartPlay());
    }

    IEnumerator StartPlay()
    {

        Anim.SetTrigger("play");
        yield return new WaitForSeconds(0.5f);
        tAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadSceneAsync("Main");

    }

    public void Quit()
    {
        Application.Quit();
    }

    //same as audio manager
    public IEnumerator FadeOut(float duration, AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;

        }
        audioSource.volume = 0f;
        audioSource.Stop();

    }

}
