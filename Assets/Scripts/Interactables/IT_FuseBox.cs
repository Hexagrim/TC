using System.Collections;
using UnityEngine;


public class IT_FuseBox : Interactable
{
    private Animator anim;
    public bool open = false;
    bool cut;

    public GameObject jointWire, cutWire;
    public override void OnUse()
    {
        base.OnUse();
        if (open)
        {
            if (!cut)
            {
                cut = true;
                cutWire.SetActive(true);
                jointWire.SetActive(false);
            }
            else
            {
                open = false;
            }
        }
        else
        {
            open = true;
        }
    }
    protected override void Update()
    {
        base.Update();
        anim.SetBool("open",open);
        
    }
    
}
