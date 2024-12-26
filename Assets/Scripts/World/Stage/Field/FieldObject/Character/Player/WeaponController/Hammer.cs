using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Hammer : WeaponBehavior
{
    float chargingcount;
    List<MonsterMarcine> monsterMarcines = new List<MonsterMarcine>();
    public override void BtnDown()
    {
        player.SetAnimatorValue(CharacterAnimeIntName.AttackType,1);
    }
    public override void BtnUp()
    {
        if (player.GetState()  is AttackState attackState)
        {
            attackState.BtnUp();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonsterPart>(out MonsterPart combatable) && !other.GetComponent<PlayerMarcine>() && !monsterMarcines.Contains(combatable.monsterMarcine))
        {
            var data = new DamgeData(weaponDMG * disdmg * 0.01f, 0, player);
            combatable.TakeDamge(data);
        }
    }
}
