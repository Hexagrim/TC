using System.Collections;
using TMPro;
using UnityEditor.Rendering.Canvas.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public TMP_Text promptText;
    Animator promptAnim;

    bool canShowPrompt = true;

    bool paused;
    bool canPause;

    public GameObject pauseMenu;


    public bool gameEnded;
    public Animator endUI;
    void Start()
    {
        promptAnim = promptText.GetComponent<Animator>();
        canPause = true;
        canShowPrompt = true;
    }

    void Update()
    {
        //way to use prompts 
        //FindAnyObjectByType<UI_Manager>().ShowPrompt("I cant do that..");

        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            StartCoroutine(PauseCooldown());
            paused = !paused;

        }

        pauseMenu.SetActive(paused);
        FindAnyObjectByType<PlayerMovement>().paused = paused;
        FindAnyObjectByType<Interact>().paused = paused;
        FindAnyObjectByType<PickItem>().paused = paused;

        if (paused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ShowPrompt(string pt)
    {
        if (!canShowPrompt) return;
        StartCoroutine(PromptCooldown());
        promptText.text = pt;
        promptAnim.SetTrigger("show");
    }

    IEnumerator PromptCooldown()
    {
        canShowPrompt = false;
        yield return new WaitForSeconds(2.1f);
        canShowPrompt = true;
    }

    IEnumerator PauseCooldown()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(0.5f);
        canPause = true;
    }
    
    public void Continue()
    {
        if (canPause)
        {
            paused = false;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }
    public void Quit()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    IEnumerator EndCredit()
    {
        endUI.SetTrigger("end");
        yield return new WaitForSeconds(25);
        SceneManager.LoadSceneAsync("MainMenu");

    }
}
