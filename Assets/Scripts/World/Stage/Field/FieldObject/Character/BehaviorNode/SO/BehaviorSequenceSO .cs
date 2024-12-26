using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBehaviorSequence", menuName = "Behavior/Sequence")]
public class BehaviorSequenceSO : ScriptableObject
{
    [SerializeField] List<BehaviorPhaseSO> actionPhases;
    public BehaviorSequence CreatBehaviorSequence(CharacterMarcine character)
    {
        return new BehaviorSequence(character, actionPhases);
    }
}
public class BehaviorSequence
{
    private List<BehaviorPhase> actionPhases;
    public Dictionary<string, object> decisionContext = new Dictionary<string, object>();
    public CharacterMarcine character { get; protected set; }
    public BehaviorSequence(CharacterMarcine character, List<BehaviorPhaseSO> actionPhases)
    {
        this.character = character;
        var list = new List<BehaviorPhase>();
        foreach (var phase in actionPhases)
        {
            var pase = phase.CreatBehaviorPhase();
            pase.SetParentSequence(this);
            list.Add(pase);
        }
        this.actionPhases = list;
    }
    public void SetData(string key, object value)
    {
        decisionContext[key] = value;
    }
    public BehaviorState Execute()
    {
        foreach (BehaviorPhase phase in actionPhases)
        {
            var result = phase.Execute();
            if (result == BehaviorState.FAILURE) return BehaviorState.FAILURE;
            if (result == BehaviorState.RUNNING) return BehaviorState.RUNNING;
        }
        return BehaviorState.SUCCESS;
    }
}