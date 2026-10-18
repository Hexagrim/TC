using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isUsing = false;
    public bool canUse = true;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Use()
    {
        if (!canUse) return;
        OnUse();
    }

    public virtual void StartUsing()
    {
        if (!canUse) return;
        isUsing = true;
    }
    
    public virtual void StopUsing()
    {
        if (!canUse) return;
        isUsing = false;
    }
    public virtual void OnUse()
    {

    }
}
