using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class IT_Lock : Interactable
{
    PickItem pickMechanic;
    public Interactable connectedObject;
    protected bool unlocked = false;
    protected override void Start()
    {
        base.Start();
        pickMechanic = FindAnyObjectByType<PickItem>();
    }
    protected override void Update()
    {
        base.Update();
        connectedObject.canUse = unlocked;
        
    }

    public override void OnUse()
    {
        base.OnUse();
        if (unlocked) return;
        if (pickMechanic.heldItem != null && pickMechanic.heldItem.GetComponent<I_Key>())
        {
            TryUnlock(pickMechanic.heldItem.GetComponent<I_Key>());
        }
    }

    protected virtual void TryUnlock(I_Key usedKey)
    {
        if (unlocked) return;
        if (usedKey.Lock == this)
        {
            OpenLock();
        }
        else
        {
            WrongKey();
        }
    }
    
    protected virtual void OpenLock()
    {
        if (unlocked) return;
        unlocked = true;
    }

    protected virtual void WrongKey()
    {
        if (unlocked) return;
        unlocked = false;
    }
}
