using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class sc_ShootConfiguration : ScriptableObject
{
    [Header ("Choose gun logic/behavior")]
    public LayerMask HitMask;
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float FireRate = 0.25f;
}
