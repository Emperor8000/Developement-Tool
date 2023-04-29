using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Impact Particle Config", menuName = "Guns/Impact Particle Config", order = 6)]
public class sc_ImpactParticleConfig : ScriptableObject
{
    [Header("Create bullet impact visuals")]
    [Space]

    [Tooltip("Set the material that renders the bullet impact")]
    public Material Material;

    [Tooltip("Min number of Particles")]
    public float MinParticleNumber = 5;

    [Tooltip("Max number of Particles")]
    public float MaxParticleNumber = 10;

    [Tooltip("Min Particle Size")]
    public float MinParticleSize = 0.01f;

    [Tooltip("Max Particle Size")]
    public float MaxParticleSize = 0.1f;

    [Space]
    [Tooltip("Particle System Duration")]
    public float Duration = 0.3f;

    public Color Color;

    [Tooltip("Relative speed that the bullet particle effect plays at; 100 is default")]
    public float SimulationSpeed = 100f;
}
