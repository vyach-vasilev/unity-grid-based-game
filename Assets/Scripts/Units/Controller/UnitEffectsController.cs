using System.Collections.Generic;
using UnityEngine;

public class UnitEffectsController: MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _effects;
    
    public void PlayEffect(string effectName)
    {
        var effect = _effects.Find(e => e.name.Equals(effectName));
        effect.Play();
    }
}