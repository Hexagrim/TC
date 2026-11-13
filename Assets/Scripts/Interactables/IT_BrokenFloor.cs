
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class IT_BrokenFloor : Interactable
{
    PickItem player;
    bool placed = false;

    public GameObject placedPlank;
    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<PickItem>();
        
    }

    protected override void Update()
    {
        base.Update();

    }

    public override void OnUse()
    {
        base.OnUse();

        if (IsHoldingPlank())
        {

            placed = true;
            placedPlank.SetActive(true);
            Destroy(player.heldItem.gameObject);
            player.heldItem = null;
            Destroy(this, 0.1f);
            
        }
    }

    bool IsHoldingPlank()
    {
        if (player.heldItem == null) return false;
        return player.heldItem.GetComponent<I_Plank>();
    }

}
