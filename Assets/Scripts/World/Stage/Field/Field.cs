using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    public void Awake()
    {
        CharacterDataManager.GetSingleton();
        var itemdata =  ItemDataLoader.Instance;
        var monsterData =MonsterDataLoader.GetSingleton();
        var part = ParticleResourceData.Instance;
    }
}
