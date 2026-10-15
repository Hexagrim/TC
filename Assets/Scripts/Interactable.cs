using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isUsing = false;
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Use()
    {

    }

    public virtual void StartUsing()
    {
        isUsing = true;
    }
    
    public virtual void StopUsing()
    {
        isUsing = false;
    }
}
