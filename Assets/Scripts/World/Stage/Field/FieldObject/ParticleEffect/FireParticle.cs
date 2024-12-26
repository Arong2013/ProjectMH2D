using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    public float damage = 10f;
    List<ICombatable> combatables = new List<ICombatable>();    
    private void OnDisable()
    {
        combatables.Clear();
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<ICombatable>(out ICombatable target) && !combatables.Contains(target))
        {
            combatables.Add(target);
            var data = new DamgeData(damage, 1, this);
            target.TakeDamge(data);
        }
    }
}
