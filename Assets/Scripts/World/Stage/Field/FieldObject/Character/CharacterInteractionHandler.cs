using UnityEngine;
using System;
using System.Collections.Generic;
public abstract class CharacterInteractionHandler
{
    protected CharacterMarcine characterMarcine;
    protected Rigidbody rigidbody;
    protected Dictionary<InteractionType, object[]> InteractionData = new Dictionary<InteractionType, object[]>();
    public CharacterInteractionHandler(CharacterMarcine characterMarcine, Rigidbody rigidbody)
    {
        this.characterMarcine = characterMarcine;
        this.rigidbody = rigidbody;
    }
    public abstract void SetInteraction(InteractionType interactionType, params object[] objects);
    public abstract void ThrowOBJ();
    public abstract void Climb();
}