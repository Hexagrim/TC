using UnityEngine;

public class PROTO_Lock : IT_Lock
{
    Rigidbody rb;
    Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();
        rb.isKinematic = !unlocked;
    }

    protected override void OpenLock()
    {
        base.OpenLock();
        anim.SetBool("open", true);
        Destroy(this, 0.1f);
    }

}
