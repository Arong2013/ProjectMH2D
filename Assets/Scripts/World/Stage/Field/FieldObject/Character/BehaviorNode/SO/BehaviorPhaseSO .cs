using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBehaviorPhase", menuName = "Behavior/Phase")]
public class BehaviorPhaseSO : ScriptableObject
{
    public List<BehaviorConditionSO> conditions;
    public BehaviorActionSO taskAction;
    public BehaviorPhase CreatBehaviorPhase()
    {
        return new BehaviorPhase(conditions, taskAction);
    }
}
public class BehaviorPhase
{
    public CharacterMarcine character => parentSequence.character;
    private BehaviorSequence parentSequence;
    private List<BehaviorCondition> conditions;
    private BehaviorAction taskAction;
    public BehaviorPhase(List<BehaviorConditionSO> conditions, BehaviorActionSO taskAction)
    {
        var list = new List<BehaviorCondition>();
        foreach(var condition in conditions)
        {
            Debug.Log(condition);
            list.Add(condition.CreateCondition());
        }
        this.taskAction = taskAction.CreateAction();
        this.conditions = list;
        list.ForEach(condition => condition.SetParent(this));
        this.taskAction.SetParent(this);
    }
    public BehaviorState Execute()
    {
        bool allConditionsMet = conditions.TrueForAll(c => c.Execute() == BehaviorState.SUCCESS);
        return allConditionsMet ? taskAction.Execute() : BehaviorState.FAILURE;
    }

    public void SetData(string key, object value) => parentSequence.SetData(key, value);

    public object GetData(string key)
    {
        if(!parentSequence.decisionContext.ContainsKey(key))
            return null;
        return parentSequence.decisionContext[key];
    }
    public void SetParentSequence(BehaviorSequence sequence) => parentSequence = sequence;
}
