
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class IT_Valve : Interactable
{
    Animator anim;
    PickItem player;

    bool used = false;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PickItem>();
       
    }

    protected override void Update()
    {
        base.Update();
        anim.SetBool("used", used);
    }

    public override void OnUse()
    {
        base.OnUse();

        if (IsHoldingValveWheel())
        {
            used = true;

            //removing the object from here is prolly not a good idea but im too lazy(T n T)

            Destroy(player.heldItem.gameObject);
            player.heldItem = null;
            Destroy(this, 0.1f);
            
        }
    }

    bool IsHoldingValveWheel()
    {
        if (player.heldItem == null) return false;
        return player.heldItem.GetComponent<I_ValveWheel>();
    }

}
