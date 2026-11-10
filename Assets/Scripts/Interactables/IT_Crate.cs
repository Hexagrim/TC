
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class IT_Crate : Interactable
{
    //comment-ed out stuff cause there will probably be no animations for this, but i might change my mind later

    //Animator anim;
    PickItem player;

    bool used = false;


    public GameObject open, close;

    protected override void Start()
    {
        base.Start();
        //anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PickItem>();
       
    }

    protected override void Update()
    {
        base.Update();
        //anim.SetBool("used", used);

        
    }

    public override void OnUse()
    {
        base.OnUse();

        if (IsHoldingCrowbar())
        {
            used = true;

            //Destroy(player.heldItem.gameObject);
            //player.heldItem = null;
            open.SetActive(true);
            close.SetActive(false);
            GetComponent<Collider>().enabled = false;
            Destroy(this, 0.1f);
            
        }
    }

    bool IsHoldingCrowbar()
    {
        if (player.heldItem == null) return false;
        return player.heldItem.GetComponent<I_CrowBar>();
    }

}
