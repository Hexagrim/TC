using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    int objectLayer;

    bool canPlayDropSFX = false;
    AudioManager AM;
    protected virtual void Start()
    {
        objectLayer = gameObject.layer;
        AM = FindAnyObjectByType<AudioManager>();
    }

    protected virtual void Update()
    {

    }

    public virtual void Use()
    {

    }

    public virtual void Pick()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Hand");
    }

    public virtual void Drop()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
        gameObject.layer = objectLayer;
        GetComponent<Rigidbody>().AddTorque(
            new Vector3(Random.Range(-0.05f, 0.05f), 0f, Random.Range(-0.05f, 0.05f)),
            ForceMode.Impulse
        );
        Debug.Log("DONEEEEEEEE");
        canPlayDropSFX = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canPlayDropSFX)
        {
            canPlayDropSFX = false;
            if (AM != null) AM.PlaySFX(AM.itemDrop);
        }
    }

}
