
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class IT_Altar : Interactable
{
    Animator anim;
    PickItem player;

    bool placed = false;

    public GameObject vaseR, vaseB, vaseY;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PickItem>();       
    }

    protected override void Update()
    {
        base.Update();       
        anim.SetBool("placed", placed);
    }

    public override void OnUse()
    {
        base.OnUse();

        if (IsHoldingVase())
        {
            SetVaseColor();
            placed = true;
            Destroy(player.heldItem.gameObject);
            player.heldItem = null;
            Destroy(this, 0.1f);
            
        }
    }

    bool IsHoldingVase()
    {

        if (player.heldItem == null) return false;
        return player.heldItem.GetComponent<I_Vase>();

    }

    void SetVaseColor()
    {
        vaseR.SetActive(player.heldItem.GetComponent<I_Vase>().color == "R");
        vaseB.SetActive(player.heldItem.GetComponent<I_Vase>().color == "B");
        vaseY.SetActive(player.heldItem.GetComponent<I_Vase>().color == "Y");
    }

}
