using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class WandStats : ScriptableObject
{
    public GameObject model;
   
    [Range(1, 10)] public float shootDamageMod;   
    [Range(0.1f, 3)] public float shootRate;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootVol;
}
