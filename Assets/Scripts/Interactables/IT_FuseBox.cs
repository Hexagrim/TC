using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class IT_FuseBox : Interactable
{
    private Animator anim;
    public bool open = false;
    bool cut;

    public GameObject jointWire, cutWire;
    private PickItem player;

    public IT_Screw[] screws;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PickItem>();
    }

    public override void OnUse()
    {
        base.OnUse();
        if (open)
        {
            if (!cut && IsHoldingWireCutter())
            {
                cut = true;
            }
            else
            {
                open = false;
            }
        }
        else
        {
            open = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        anim.SetBool("open",open);

        //wire thingy
        cutWire.SetActive(cut);
        jointWire.SetActive(!cut);

        //check for screws:

        if (Array.TrueForAll(screws, obj => obj == null))
        {
            canUse = true;
        }
        else
        {
            canUse = false;
        }
    }
    bool IsHoldingWireCutter()
    {
        //return player.heldItem.GetComponent<I_WireCutter>(); add laterrt
        return true;
    }
    
    
}
