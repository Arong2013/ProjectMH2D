using UnityEngine;
using UnityEngine.EventSystems;

public class AttackBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private WeaponBehavior weaponBehavior;
    public void Init(WeaponBehavior weaponBehavior)
    {
        this.weaponBehavior = weaponBehavior; 
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        weaponBehavior?.BtnDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        weaponBehavior?.BtnUp();
    }
}
