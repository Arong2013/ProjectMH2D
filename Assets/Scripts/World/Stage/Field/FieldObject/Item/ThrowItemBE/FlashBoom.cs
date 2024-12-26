using UnityEngine;
using System.Collections;
public class FlashBoom : MonoBehaviour
{
    [SerializeField] ThrowItemType throwItemType;
    [SerializeField] float boomTime;
    [SerializeField] GameObject FlashBoomEffect;

    private void Update()
    {
       boomTime -= Time.deltaTime;  
        if (boomTime < 0 )
        {
            PlayBoom();
            Destroy(gameObject);
        }
    }
    public void PlayBoom()
    {
        Utils.GetUI<FlashEffectUI>().OpenEffect();
    }
}