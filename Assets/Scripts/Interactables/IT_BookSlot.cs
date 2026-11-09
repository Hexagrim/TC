
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class IT_BookSlot : Interactable
{

    Animator anim;
    PickItem player;

    bool placed = false;
    public string bookColor;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PickItem>();
    }

    protected override void Update()
    {
        base.Update();
        anim.SetBool("used", placed);
    }

    public override void OnUse()
    {
        base.OnUse();

        if (IsHoldingValveWheel())
        {
            placed = true;

            //yeah its still a bad idea T_T

            Destroy(player.heldItem.gameObject);
            player.heldItem = null;
            Destroy(this, 0.1f);
            
        }

    }

    bool IsHoldingValveWheel()
    {

        if (player.heldItem == null) return false;
        if (!player.heldItem.GetComponent<I_Book>()) return false;
        else return player.heldItem.GetComponent<I_Book>().color == bookColor;

    }

}
