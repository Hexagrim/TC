
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

public class IT_MainDoor : Interactable
{
    bool used = false;
    public IT_Screw[] screws;
    public IT_Plank[] planks;
    public IT_KeycardScanner keycardScanner;

    //empty until i make the ending of the game ::
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();       
    }

    public override void OnUse()
    {
        if(used) return;
        if (!Array.TrueForAll(screws, o => o == null)) return;
        if(!Array.TrueForAll(planks, o => o == null)) return;
        if(!keycardScanner.used) return;
        base.OnUse();
        used = true;
        FindAnyObjectByType<UI_Manager>().EndGame();


    }

}
