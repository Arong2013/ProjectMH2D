using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;

public class JumpAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new JumpState(character));
    }
}
public class JumpState : CharacterState
{
    float reTime;
    public JumpState(CharacterMarcine character) : base(character) { }

    public override void Enter()
    {
        base.Enter();
        character.Jump();
    }
    public override void Execute()
    {
        character.Move();
        reTime += 1;
        character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, character.currentDir.magnitude);
        if (reTime > 60f && character.isGround)
            character.SetAnimatorValue(CharacterAnimeIntName.MovementType, 0);
        if (character.IsFalling())
        {

        }
    }

    public override void Exit()
    {
        base.Exit();
        character.SetAnimatorValue(CharacterAnimeIntName.MovementType, 0);
    }
}
