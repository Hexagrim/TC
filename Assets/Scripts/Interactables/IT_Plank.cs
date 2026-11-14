
using System.Collections;
using UnityEngine;

public class IT_Plank : Interactable
{
    Animator anim;
    PickItem player;
    Rigidbody plankModel;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PickItem>();
        plankModel = GetComponent<Rigidbody>();

        plankModel.isKinematic = true;
    }
    public override void OnUse()
    {
        base.OnUse();
        if (IsHoldingCrowBar())
        {
            plankModel.isKinematic = false;
            plankModel.AddForce(plankModel.transform.right * Random.Range(200, 300));
            Destroy(this, 0.05f);
        }
    }

    bool IsHoldingCrowBar()
    {

        if (player.heldItem == null) return false;
        return player.heldItem.GetComponent<I_CrowBar>();

    }

        

}
