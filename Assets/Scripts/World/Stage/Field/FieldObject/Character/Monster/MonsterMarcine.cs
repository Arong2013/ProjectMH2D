using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class MonsterMarcine : CharacterMarcine
{
    [SerializeField] int baseMonsterID;
    [SerializeField] List<MonsterPart> monsterParts;
    [SerializeField] List<BehaviorSequenceSO> behaviorSequencesSO;


    List<BehaviorSequence> behaviorSequences = new List<BehaviorSequence>();
    public bool IsHarvest { get; set; }
    public object LastTarget { get; set; }
    public override void Init()
    {
        currentBState = new IdleState(this,animator);
        var dataloder = MonsterDataLoader.GetSingleton();
        CharacterData monsterData = dataloder.GetMonsterBaseData(baseMonsterID);
        characterData = monsterData;
        foreach (var part in monsterParts)
        {
            if (dataloder.GetMonsterPartData(baseMonsterID, part.BasePartID) is { } partData)
                part.Init(this, partData);
        }
        behaviorSequencesSO.ForEach(sequence => behaviorSequences.Add(sequence.CreatBehaviorSequence(this)));



        SetHandler();
        
    }

    void SetHandler()
    {
        CharacterMovementHandler = new MonsterMovementHandler(this, GetComponent<Rigidbody>());
        CharacterCombatHandler = new MonsterCombatHandler(this, GetComponent<Rigidbody>());
    }
    private void Update()
    {
        for (var i = 0; i < behaviorSequences.Count; i++)
        {
            var seq = behaviorSequences[i];
            if (seq.Execute() == BehaviorState.FAILURE)
                continue;
            else
                break;
        }
        currentBState?.Execute();
    }
    public void Dead()
    {
        if(LastTarget is PlayerMarcine player)
        {
            QuestEvents.MonsterKilled(baseMonsterID);
        }
        CharacterAnimatorHandler.SetAnimatorValue(CharacterAnimeBoolName.CanDead, true);
    }
    public void SetHarvest(bool can) { IsHarvest = can; }
    public void SetLastTarget(object tag) => LastTarget = tag;
}