using UnityEngine;
using UnityEngine.Rendering;
public class ClimbAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;
    [SerializeField] BehaviorState BehaviorState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new ClimbState(character, BehaviorState));
     
    }
}
public class ClimbState : CharacterState
{
    private Rigidbody rigidbody;
    private float climbSpeed = 12f;         // 사다리 오르내리기 속도
    private float detectionDistance = 0.5f; // Raycast 거리

    private BehaviorState behaviorState;  // BehaviorState 유지
    Animator animator;
    public ClimbState(CharacterMarcine character, BehaviorState behaviorState) : base(character)
    {
        this.behaviorState = behaviorState;
        animator =  character.GetComponent<Animator>();
    }

    public override void Enter()
    {
        base.Enter();
        rigidbody = character.GetComponent<Rigidbody>();

        character.SetAnimatorValue(CharacterAnimeIntName.InteractionType, 0);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.useGravity = false;
        animator.applyRootMotion = false;

        if(behaviorState == BehaviorState.SUCCESS)
        {
            HandleSuccessState();
            if(character.GetAnimatorValue<CharacterAnimeFloatName, float>(CharacterAnimeFloatName.SpeedCount) < -0.9)
            {

                animator.applyRootMotion = true;
            }
        }
        if(character.GetAnimatorValue<CharacterAnimeFloatName,float>(CharacterAnimeFloatName.SpeedCount) > 2.9)
        {
            animator.applyRootMotion = true;
        }
    }

    public override void Execute()
    {
        base.Execute();
        if (behaviorState == BehaviorState.RUNNING)
        {
            HandleClimbingMovement();
        }
    }

    public override void Exit()
    {
        base.Exit();
        rigidbody.useGravity = true;
        animator.applyRootMotion = false;
        if (character.GetAnimatorValue<CharacterAnimeFloatName, float>(CharacterAnimeFloatName.SpeedCount) < -0.9)
            RotateCharacter();
    }
    private void HandleSuccessState()
    {
        if (IsLadderBelow())
        {
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, -1f);
        }
        else if (IsLadderAbove())
        {
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, 1f);
        }
    }
    private void HandleClimbingMovement()
    {
        float directionY = character.currentDir.y;
        character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, directionY);

        if (directionY > 0.1f && !IsLadderAbove())
        {
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, 3f);
        }
        else if (directionY < -0.1f && !IsLadderBelow())
        {
            character.SetAnimatorValue(CharacterAnimeFloatName.SpeedCount, -3f);
        }
        character.Climb();
    }
    private bool IsLadderAbove()
    {
        return Physics.OverlapSphere(character.transform.position + Vector3.up * 1.5f, 0.3f, LayerMask.GetMask("Ladder")).Length >= 1;
    }
    private bool IsLadderBelow()
    {
        return Physics.OverlapSphere(character.transform.position, 0.3f, LayerMask.GetMask("Ladder")).Length >= 1;
    }
    private void RotateCharacter()
    {
        character.transform.rotation = Quaternion.Euler(0, character.currentDir.x > 0 ? 90 : -90, 0);
    }
}
