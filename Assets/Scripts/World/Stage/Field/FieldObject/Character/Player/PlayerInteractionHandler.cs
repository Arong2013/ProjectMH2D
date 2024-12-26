using UnityEngine.TextCore.Text;
using UnityEngine;
using System;

public class PlayerInteractionHandler : CharacterInteractionHandler
{
    public PlayerInteractionHandler(PlayerMarcine playerMarcine, Rigidbody rigidbody) : base(playerMarcine, rigidbody) { }
    public override void SetInteraction(InteractionType interactionType,params object[] objects)
    {
        AddData(interactionType, objects);
    }
    public override void ThrowOBJ()
    {
        if (InteractionData.TryGetValue(InteractionType.Throw,out var objects))
        {
            var pos = characterMarcine.transform.position + new Vector3(0, 0.5f, 0) + characterMarcine.transform.forward * 0.5f;
            MonoBehaviour.Instantiate(objects[0] as GameObject, pos, characterMarcine.transform.rotation);
        }
    }
    public override void Climb()
    {
        var dirY = characterMarcine.currentDir.y;
        Vector3 dir;
        if (dirY > 0)
            dir = Vector3.up;
        else if (dirY == 0)
            dir = Vector3.zero;
        else
            dir = Vector3.down;
        Vector3 climbMovement = characterMarcine.transform.position + dir * 5f * Time.deltaTime;
        rigidbody.MovePosition(climbMovement);
    }
    void AddData(InteractionType interactionType, params object[] objects)
    {
        if(objects.Length > 0)
        {
            if(!InteractionData.ContainsKey(interactionType))
                InteractionData.Add(interactionType, objects);
            else
                InteractionData[interactionType] = objects; 
        }
    }
}