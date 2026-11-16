using System.Collections;
using TMPro;
using UnityEditor.Rendering.Canvas.ShaderGraph;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public TMP_Text promptText;
    Animator promptAnim;

    bool canShowPrompt = true;

    void Start()
    {
        promptAnim = promptText.GetComponent<Animator>();
        canShowPrompt = true;
    }

    void Update()
    {
        //way to use prompts 
        //FindAnyObjectByType<UI_Manager>().ShowPrompt("I cant do that..");
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
}
