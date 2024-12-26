using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public void InteractEnter(PlayerMarcine player)
    {
        var uis = Utils.GetUI<QuestCollectionUI>();
        uis.OpenCollectionUI();
    }
}
