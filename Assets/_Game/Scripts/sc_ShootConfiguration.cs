using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class sc_ShootConfiguration : ScriptableObject
{
    [Header("Choose gun logic/behavior")]
    [Space]

    [Tooltip("Whether the weapon shoots raycasts")]
    public bool Raycast = true;

    [Tooltip("Whether the weapon shoots projectile objects")]
    public bool Projectile = false;

    [Tooltip("If the weapon shoots projectiles, give the projectile prefab here (it must contain a collider and a rigidBody)")]
    public GameObject ProjectilePrefab = null;

    [Tooltip("If the weapon shoots projectiles, set the speed of projectiles here")]
    public float ProjectileSpeed = 20;

    [Tooltip("How many seconds will the projectile exist if it doesn't hit anything?")]
    public float ProjectileMissDuration = 20;

    [Space]
    [Tooltip("What layer you want this weapon to hit (only affects raycasts)")]
    public LayerMask HitMask;

    [Tooltip("Whether or not you can hold down the mouse to continue firing")]
    public bool IsAutomatic = false;

    [Tooltip("Maximum spread of the weapon")]
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);

    [Tooltip("Set minimum time in seconds between each shot, this one probably didn't need a tooltip")]
    public float FireRate = 0.25f;
}
