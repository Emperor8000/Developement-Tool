using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Trail Configuration", menuName = "Guns/Gun", order = 0)]
public class sc_GunConfiguration : ScriptableObject
{
    public sc_GunType Type;
    public string Name;
    public GameObject PrefabModel;
    public Vector3 Spawnpoint;
    public Vector3 SpawnRotation;

    public sc_ShootConfiguration ShootConfig;
    public sc_TrailConfiguration TrailConfig;

    private MonoBehaviour ActiveMonoBehavior;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootParticle;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehavior)
    {
        this.ActiveMonoBehavior = ActiveMonoBehavior;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(PrefabModel);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = Spawnpoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

        ShootParticle = Model.GetComponentInChildren<ParticleSystem>();
    }
    
    public void Shoot()
    {
        if(Time.time > ShootConfig.FireRate + LastShootTime)
        {
            LastShootTime = Time.time; //set now as last shoot time

            if (ShootParticle != null) //check if particle system is not null, then play it if it is assigned
            {
                ShootParticle.Play();
                Vector3 shootDirection = ShootParticle.transform.forward + new Vector3(Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x), 
                    Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y), Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z));

                if(Physics.Raycast(ShootParticle.transform.position, shootDirection, out RaycastHit Hit, float.MaxValue, ShootConfig.HitMask))
                {

                }
            }else
            {
                Debug.Log("No Particle System Assigned");
            }
        }
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return trail;
    }
}
