using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
public class PlayerMovementHandler : CharacterMovementHandler
{
    private PlayerMarcine character;
    Action ItemAction;
    public PlayerMovementHandler(CharacterMarcine character,Rigidbody rigidbody)
    {
        this.character = character as PlayerMarcine;
    }
    public override void Move()
    {
        float speed = character.characterData.GetStat(CharacterStatName.SPD) * character.currentDir.magnitude;
        Vector2 moveDirection = new Vector2(character.currentDir.x, 0).normalized * speed;
        character.Rigidbody2D.velocity = new Vector2(moveDirection.x, character.Rigidbody2D.velocity.y);

        if (character.currentDir.x != 0)
        {
            character.SpriteRenderer.flipX = character.currentDir.x > 0;
        }
    }
    public override void Roll()
    {
        character.Rigidbody2D.velocity = Vector3.zero;
        character.Rigidbody2D.angularVelocity = 0;

        Vector3 backwardForce = character.transform.forward * 5f;
        character.Rigidbody2D.AddForce(backwardForce, ForceMode2D.Impulse);
    }
    public override void Jump()
    {
        Debug.Log("점푸");
        character.Rigidbody2D.velocity = Vector2.zero;
        character.Rigidbody2D.angularVelocity = 0;
        character.Rigidbody2D.AddForce(Vector2.up * 7f,ForceMode2D.Impulse);
    }
}
