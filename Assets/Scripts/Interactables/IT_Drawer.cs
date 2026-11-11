using System.Collections;
using UnityEngine;


public class IT_Drawer : Interactable
{
    public Animator Anim;
    Transform player;
    bool canUseDoor = true;
    public bool open = false;

    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<PlayerMovement>().transform;
    }
    public override void OnUse()
    {
        base.OnUse();
        if (!canUseDoor) return;
        open = !open;
        StartCoroutine(Cooldown());
    }
    protected override void Update()
    {
        base.Update();
        Anim.SetBool("open", open);
    }
    IEnumerator Cooldown()
    {
        canUseDoor = false;
        yield return new WaitForSeconds(0.1f);
        canUseDoor = true;
    }
}
