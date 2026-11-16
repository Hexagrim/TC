using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Animator Anim;
    public Animator tAnim;

    bool playing;
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void Play()
    {
        if (playing) return;

        playing = true;
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

}
