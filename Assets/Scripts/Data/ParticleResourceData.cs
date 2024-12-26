using System.Collections.Generic;
using UnityEngine;
public class ParticleResourceData
{
    private static ParticleResourceData _instance;
    public static ParticleResourceData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ParticleResourceData();
                _instance.LoadResources();
            }
            return _instance;
        }
    }
    private readonly Dictionary<string, GameObject> particlePrefabs = new Dictionary<string, GameObject>();
    private void LoadResources()
    {
        GameObject[] loadedParticles = Resources.LoadAll<GameObject>("Particles");

        foreach (GameObject particle in loadedParticles)
        {
            particlePrefabs[particle.name] = particle;
        }

        Debug.Log($"Loaded {particlePrefabs.Count} particle prefabs.");
    }
    public GameObject GetParticle(string name)
    {
        if (particlePrefabs.TryGetValue(name, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"Particle prefab with name {name} not found!");
            return null;
        }
    }
}
