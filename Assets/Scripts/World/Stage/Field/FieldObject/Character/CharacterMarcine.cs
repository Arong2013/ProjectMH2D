using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;



public enum CharacterStatName
{
    HP, MaxHP,
    SP, MaxSP,
    ATK,
    DEF,
    SPD,
    RollSP,
}



public enum CharacterAnimeBoolName
{
    CanCombo,
    CanCharging,
    CanDead,
}
public enum CharacterAnimeFloatName
{
    ChargingCount,
    SpeedCount,
}
public enum CharacterAnimeIntName
{
    AttackType,
    HitType,
    RoarType,
    MovementType,
    InteractionType
}
public enum MovementType
{
    Walk = 1,
    Jump = 2,
    Roll = 3,
    Falling = 4,
}

public enum InteractionType
{
    Climb = 1,
    Throw = 2
}

public abstract class CharacterMarcine : FieldObject, ICombatable
{
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] protected Rigidbody2D rigidbody2D;
    [SerializeField] float groundCheckDistance;
    
    public CharacterData characterData { get; protected set; }
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected CharacterState currentBState;
    protected CharacterAnimatorHandler CharacterAnimatorHandler;
    protected CharacterMovementHandler CharacterMovementHandler;
    protected CharacterCombatHandler CharacterCombatHandler;
    protected CharacterInteractionHandler characterInteractionHandler;
    public Vector2 currentDir { get; protected set; }
    public Rigidbody2D Rigidbody2D => rigidbody2D;  

    public SpriteRenderer SpriteRenderer => spriteRenderer; 
    public bool isGround => IsGrounded();

    public bool IsFalling() => rigidbody2D.velocity.y < -3 && !isGround;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        CharacterAnimatorHandler = new CharacterAnimatorHandler(animator);
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }
    private void Start()
    {
        Init();
    }
    public abstract void Init();
    public void Move() => CharacterMovementHandler.Move();
    public void Roll() => CharacterMovementHandler.Roll();
    public void TakeDamge(DamgeData damgeData) => CharacterCombatHandler.TakeDamage(damgeData);
    public void Climb() => characterInteractionHandler.Climb();
    public void ThrowObj() => characterInteractionHandler.ThrowOBJ();
    public void Jump()
    {
        if (!isGround) return;
        CharacterMovementHandler.Jump();
        CharacterAnimatorHandler.SetAnimatorValue(CharacterAnimeIntName.MovementType, (int)MovementType.Jump);
    }
    public void ChangePlayerState(CharacterState newState)
    {
        currentBState?.Exit();
        currentBState = newState;
        currentBState.Enter();
    }
    public Type GetCharacterStateType() => currentBState.GetType();
    public CharacterState GetState() => currentBState;
    public void SetDir(Vector2 dir) { currentDir = dir; }
    public void SetInteraction(InteractionType interactionType, params object[] objects)
    {
        characterInteractionHandler.SetInteraction(interactionType, objects);
        SetAnimatorValue(CharacterAnimeIntName.InteractionType, (int)interactionType);
    }
    public bool IsGrounded()
    {
        Vector2 checkOrigin = (Vector2)transform.position + Vector2.down * 0.1f;
        var hit = Physics2D.Raycast(checkOrigin, Vector2.down, groundCheckDistance, GroundLayer);
        return hit.collider != null;
    }
    public void SetAnimatorValue<T>(T type, object value) where T : Enum { CharacterAnimatorHandler.SetAnimatorValue(type, value);}
    public TResult GetAnimatorValue<T, TResult>(T type) where T : Enum { return CharacterAnimatorHandler.GetAnimatorValue<T,TResult>(type); }
}
