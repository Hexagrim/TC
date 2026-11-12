using System;
using System.Collections;
using UnityEngine;

public class SmokeArea : MonoBehaviour
{
    public GameObject smoke1, smoke2;
    Collider[] smokeCollider;
    public bool closed = false;
    bool warning = false;
    
    void Start()
    {
        smokeCollider = GetComponents<Collider>();
    }

    void Update()
    {
        
    }

    public void CloseSmoke()
    {
        if(closed) return;

        smoke1.SetActive(false);
        smoke2.SetActive(false);
        Array.ForEach(smokeCollider, col  => col.enabled = false);
        closed = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!warning) StartCoroutine(ShowWarning());
        }
    }

    IEnumerator ShowWarning()
    {
        warning = true;
        Debug.Log("The steam is too hot");
        yield return new WaitForSeconds(2);
        warning = false;
    }
}
