using UnityEngine;

public class I_Torch : Item
{
    public GameObject LightSource;
    public override void Use()
    {
        LightSource.SetActive(!LightSource.activeSelf);
    }
    public override void Drop()
    {
        base.Drop();
        LightSource.SetActive(false);
    }
}
