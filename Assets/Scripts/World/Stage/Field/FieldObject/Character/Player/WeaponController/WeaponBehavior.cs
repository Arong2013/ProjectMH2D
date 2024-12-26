using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    protected PlayerMarcine player;
    protected int weaponDMG;
    protected float disdmg = 100;
    protected CapsuleCollider capsuleCollider;
    public void Initialize(PlayerMarcine player)
    {
        this.player = player;
        weaponDMG = 100;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    public abstract void BtnDown();
    public abstract void BtnUp();
    public void SetDisDMG(float dmg) => disdmg = dmg;   
    public void ColliderSet(bool isStart) => capsuleCollider.enabled = isStart;
}
