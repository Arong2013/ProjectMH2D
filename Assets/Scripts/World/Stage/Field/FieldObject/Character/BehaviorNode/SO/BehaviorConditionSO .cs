using UnityEngine;

public abstract class BehaviorConditionSO : ScriptableObject
{
    public abstract BehaviorCondition CreateCondition();
}

public abstract class BehaviorActionSO : ScriptableObject
{
    public abstract BehaviorAction CreateAction();
}
public abstract class BehaviorCondition
{
    protected CharacterMarcine character => actionPhase.character;
    protected BehaviorPhase actionPhase;
    public abstract BehaviorState Execute();
    public void SetParent(BehaviorPhase behaviorPhase) { this.actionPhase = behaviorPhase; }
}
public abstract class BehaviorAction
{
    protected CharacterMarcine character => actionPhase.character;
    protected BehaviorPhase actionPhase;
    public abstract BehaviorState Execute();
    public void SetParent(BehaviorPhase behaviorPhase) { this.actionPhase = behaviorPhase; }
}