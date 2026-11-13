using System.Collections;
using UnityEngine;


public class IT_ElectricLocker : Interactable
{
    public Animator Anim;
    Transform player;
    bool canUseChest = true;
    public bool open = false;
    IT_FuseBox fuseBox;
    public GameObject greenLight, redLight;
    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<PlayerMovement>().transform;
        fuseBox = FindAnyObjectByType<IT_FuseBox>();
    }
    public override void OnUse()
    {
        base.OnUse();
        if (!canUseChest) return;
        open = !open;
        StartCoroutine(Cooldown());

    }
    protected override void Update()
    {
        base.Update();
        Anim.SetBool("open", open);
        
        redLight.SetActive(!canUse);
        greenLight.SetActive(canUse);

        canUse = fuseBox.cut;
    }
    IEnumerator Cooldown()
    {
        canUseChest = false;
        yield return new WaitForSeconds(0.1f);
        canUseChest = true;
    }


}
