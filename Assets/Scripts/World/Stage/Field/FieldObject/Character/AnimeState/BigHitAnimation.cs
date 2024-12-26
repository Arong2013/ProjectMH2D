using System;
using UnityEngine;

public class BigHitAnimation : StateMachineBehaviour
{
    private CharacterMarcine character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character ??= animator.GetComponent<CharacterMarcine>();
        character.ChangePlayerState(new BigHitState(character, animator));
    }
}
public class BigHitState : CharacterState
{
    Animator animator;
    public BigHitState(CharacterMarcine character, Animator animator) : base(character) { this.animator = animator; }

    public override void Enter()
    {
        base.Enter();
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            switch (param.type)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(param.name, false);
                    break;

                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(param.name, 0f);
                    break;

                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(param.name, 0);
                    break;
            }
        }
        if (character.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 backwardForce = -character.transform.forward * 10f;
            rb.AddForce(backwardForce, ForceMode.Impulse);
        }

    }
}
