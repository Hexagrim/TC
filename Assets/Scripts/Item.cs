using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    int objectLayer;    
    protected virtual void Start()
    {
        objectLayer = gameObject.layer;
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
    }
    
}
