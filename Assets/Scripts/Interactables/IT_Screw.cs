
using System.Collections;
using UnityEngine;

public class IT_Screw : Interactable
{
    Animator anim;
    PickItem player;
    public Rigidbody screwModel;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<PickItem>();
        screwModel = GetComponentInChildren<Rigidbody>();

        screwModel.isKinematic = true;
    }
    public override void OnUse()
    {
        base.OnUse();
        if (IsHoldingScrewDriver())
        {
            StartCoroutine(OpenScrew());
        }
    }

    bool IsHoldingScrewDriver()
    {
        return true;
        //player.heldItem.GetComponent<I_ScrewDriver>();
    }
    IEnumerator OpenScrew()
    {
        anim.SetTrigger("open");
        yield return new WaitForSeconds(0.6f);
        screwModel.isKinematic = false;
        screwModel.AddForce(screwModel.transform.right * Random.Range(200, 300));
        screwModel.transform.SetParent(null);
        Destroy(gameObject);
    }
}
