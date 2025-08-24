using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class WandStats : ScriptableObject
{
    public GameObject model;
    public List<GameObject> bulletTypes;
    [Range(1, 10)] public int shootDamage;   
    [Range(0.1f, 3)] public float shootRate;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootVol;

}
