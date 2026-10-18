using UnityEngine;

public class PROTO_Lock : IT_Lock
{
    Rigidbody rb;
    protected override void Start()
    {
        base.Start();
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
        Destroy(this, 0.1f);
    }
}
