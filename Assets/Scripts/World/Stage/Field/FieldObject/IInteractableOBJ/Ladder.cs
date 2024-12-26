using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    public void InteractEnter(PlayerMarcine player)
    {
        player.ToggleClimb();  
    }
}
