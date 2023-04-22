using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trail Config", menuName = "Guns/Trail Configuration", order = 4)]
public class sc_TrailConfiguration : ScriptableObject
{
    [Header("Create bullet trail visuals")]
    [Space]

    [Tooltip("Set the material that renders the bullet trails")]
    public Material Material;

    [Tooltip("WidthCurve of the bullet trails")]
    public AnimationCurve WidthCurve;

    [Space]
    [Tooltip("Bullet trail duration")]
    public float Duration = 0.5f;

    public float MinVertexDistance = 0.1f;
    public Gradient Color;

    public float MissDistance = 100f;

    [Tooltip("Relative speed that the bullet trail plays at; 100 is default")]
    public float SimulationSpeed = 100f;
}
