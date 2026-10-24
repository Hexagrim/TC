using System.Collections;
using UnityEngine;

public class IT_Door : Interactable
{
    public Animator Anim;
    Transform player;
    bool canUseDoor = true;
    public bool open;
    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<PlayerMovement>().transform;
    }
    public override void OnUse()
    {
        base.OnUse();
        if (!canUseDoor) return;
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Close"))
        {
            Vector3 direction = player.position - transform.position;
            float dot = Vector3.Dot(transform.forward , direction);

            if (dot > 0)
            {
                Anim.SetTrigger("openF");
                open = true;
            }
            else  
            {
                Anim.SetTrigger("openB");
                open = true;
            }

        }

        else
        {
            Anim.SetTrigger("close");
            open = false;
        }
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canUseDoor = false;
        yield return new WaitForSeconds(0.2f);
        canUseDoor = true;
    }
    public void OpenEnemy(Transform enemy)
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Close"))
        {
            Vector3 direction = enemy.position - transform.position;
            float dot = Vector3.Dot(transform.forward, direction);

            if (dot > 0)
            {
                Anim.SetTrigger("openF");
                open = true;
            }
            else
            {
                Anim.SetTrigger("openB");
                open = true;
            }

        }
    }
}
