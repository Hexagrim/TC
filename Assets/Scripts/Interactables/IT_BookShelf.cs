
using System;
using System.Collections;
using UnityEngine;
public class IT_BookShelf : Interactable
{
    public Animator anim;
    
    bool open = false;
    public IT_BookSlot[] slots;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (AllBooksPlaced())
        {
            open = true;
            StartCoroutine(OpenDoor());
        }
    }

    public override void OnUse()
    {
        base.OnUse();


    }

    bool AllBooksPlaced()
    {
        //cool way to check for all instead of a gross for loop:
        return Array.TrueForAll(slots, slot => slot == null);
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("open", open);
        for (int i = 0; i < slots.Length; i++)
        {
            Destroy(slots[i]);
        }
        Destroy(this, 0.5f);
    }
    
}
