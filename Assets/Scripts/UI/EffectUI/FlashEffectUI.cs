using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlashEffectUI : MonoBehaviour
{
    public void OpenEffect()
    {
        gameObject.SetActive(true);
    }
    void DisableEft()
    {
        gameObject.SetActive(false);    
    }
}
