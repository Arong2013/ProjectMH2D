using System.Collections;
using UnityEngine;

public abstract class AttackState : CharacterState
{
    protected AttackState(CharacterMarcine character) : base(character) { }
    public abstract void BtnUp();

    public override void Exit()
    {
        base.Exit();
        if(character is PlayerMarcine player)
            player.WeaponAttackEnd();   
    }
}