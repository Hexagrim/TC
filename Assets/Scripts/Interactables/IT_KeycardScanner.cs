
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class IT_KeycardScanner : Interactable
{
    Animator anim;
    PickItem player;
    public Rigidbody doorHinge;
    
    public bool used;
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

        if (IsHoldingKeyCard())
        {
            Destroy(player.heldItem.gameObject);
            StartCoroutine(UseKeycard());
            player.heldItem = null;
        }

    }

    bool IsHoldingKeyCard()
    {

        if (player.heldItem == null) return false;
        return player.heldItem.GetComponent<I_KeyCard>();

    }

    IEnumerator UseKeycard()
    {
        used = true;
        yield return new WaitForSeconds(1f);
        doorHinge.isKinematic = false;

        canUse = false;

    }

}
