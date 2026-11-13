using System;
using UnityEngine;

public class AltarReward : MonoBehaviour
{

    public Animator ChestAnim;
    Animator anim;
    bool chestOpen = false;

    public IT_Altar[] altars;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ChestAnim.SetBool ("open", chestOpen);
        anim.SetBool ("open", chestOpen);

        chestOpen = Array.TrueForAll(altars, a => a == null);

    }

}
