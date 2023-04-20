using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Configuration", menuName = "Guns/Trail Configuration", order = 4)]
public class sc_TrailConfiguration : ScriptableObject
{
    public Material Material;
    public AnimationCurve WidthCurve;
    public float Duration = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient Color;

    public float MissDistance = 100f;
    public float SimulationSpeed = 100f;
}
