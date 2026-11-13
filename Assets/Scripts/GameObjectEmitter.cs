using System.Collections.Generic;
using UnityEngine;

public class GameObjectEmitter : MonoBehaviour
{
    [field: SerializeField] public GameObject ObjectPrefab { get; private set; }
    
    private ParticleSystem _ps;
    private List<ParticleSystem.Particle> ExitParticles = new();
    
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, ExitParticles);
        Debug.Log(ExitParticles.Count);
        foreach (ParticleSystem.Particle p in ExitParticles)
        {
            Debug.Log(p);
            GameObject spawnedObject = Instantiate(ObjectPrefab);
            spawnedObject.transform.position = p.position;
        }
    }
}
