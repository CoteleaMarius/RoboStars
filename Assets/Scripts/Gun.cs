using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use();

    public GameObject bulletPrefab;
    public Animator gunAnimator;
    public GameObject shootFX;
    public Transform muzzle;

}
