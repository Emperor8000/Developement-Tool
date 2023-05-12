using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "New Gun", menuName = "Guns/Gun", order = 0)]
public class sc_GunConfiguration : ScriptableObject
{
    [Header ("Organize Visuals and Logic")]

    [Tooltip("Give the gun a name: important, because it will be used to select weapons")]
    public string Name;

    [Tooltip("Set visuals, note that the model must contain a particle system where you want it to fire from or it won't work")]
    public GameObject PrefabModel = null;

    [Space]
    [Tooltip("Where should the weapon spawn")]
    public Vector3 Spawnpoint = Vector3.zero;

    [Tooltip("What rotation should the weapon spawn at")]
    public Vector3 SpawnRotation = Vector3.zero;

    [Space]
    [Tooltip("Use bullet trails? Doesn't affect projectiles, but a trail renderer can be added to the projectile prefab for this effect")]
    public bool UseTrail = true;

    [Tooltip("Play particle effect on shoot? Note: If false, particle system still necessary in order to know where to shoot from")]
    public bool UseParticle = true;

    [Space]
    [Tooltip("Set shoot logic")]
    public sc_ShootConfiguration ShootConfig = null;

    [Tooltip("Choose bullet trails configuration, not necessary if not using bullet trails")]
    public sc_TrailConfiguration TrailConfig = null;

    [Tooltip("Enter a prefab for an impact particle to be spawned upon impact (affects both raycasts and projectiles)")]
    public GameObject ImpactConfig = null;

    private MonoBehaviour ActiveMonoBehavior;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootParticle;
    private ObjectPool<TrailRenderer> TrailPool;
    private ObjectPool<ParticleSystem> ImpactParticlePool;
    private ObjectPool<GameObject> ProjectilePool;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehavior) //function to spawn visuals and set initial runtime values
    {
        this.ActiveMonoBehavior = ActiveMonoBehavior;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        ImpactParticlePool = new ObjectPool<ParticleSystem>(CreateParticle);
        ProjectilePool = new ObjectPool<GameObject>(CreateProjectile);

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

                if (UseParticle) //play particle effect if turned on
                {
                ShootParticle.Play();
                }
                
                Vector3 shootDirection = ShootParticle.transform.forward + new Vector3(Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x), 
                    Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y), Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z));

                if(ShootConfig.Raycast) //function for shooting raycasts
                {
                    if (Physics.Raycast(ShootParticle.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
                    {
                        sc_Shootable shootableObject = hit.transform.gameObject.GetComponent<sc_Shootable>();

                        if(shootableObject != null)
                        {
                            shootableObject.shoot(ShootConfig.Damage);
                        }

                        if (UseTrail)
                        {
                            ActiveMonoBehavior.StartCoroutine(PlayTrail(ShootParticle.transform.position, hit.point, hit));
                        }
                    }
                    else
                    {
                        if (UseTrail)
                        {
                            ActiveMonoBehavior.StartCoroutine(PlayTrail(ShootParticle.transform.position,
                            ShootParticle.transform.position + (shootDirection * TrailConfig.MissDistance), new RaycastHit()));
                        }
                    }
                }

                if (ShootConfig.Projectile) //function for shooting projectiles
                {   
                    if (ShootConfig.ProjectilePrefab != null && ShootParticle != null)
                    {
                        GameObject instance = ProjectilePool.Get(); //get a projectile and set it active
                        instance.transform.position = ShootParticle.transform.position;
                        instance.gameObject.SetActive(true);

                        sc_Projectile projectileLogic = instance.GetComponent<sc_Projectile>(); //get the projectile script from the projectile
                        projectileLogic._gunConfig = this; //give it this script for impact purposes
                        projectileLogic.SetStuff(ShootConfig.ProjectileMissDuration, ShootConfig.Damage);

                        projectileLogic._speed = ShootParticle.gameObject.transform.forward * ShootConfig.ProjectileSpeed + new Vector3(Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
                    Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y), Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z));
                        //projectileLogic.damageAmount = _damageAmount; an idea for damage effects
                    }
                }
                
            }else
            {
                Debug.Log("No particle system found");
            }
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null;

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint); //find distance between startpoint and end point
        float remainingDistance = distance;

        while(remainingDistance > 0) //while remaining distance is greater than 0, move the trail based on simulation speed until it reaches endpoint
        {
            instance.transform.position = Vector3.Lerp(StartPoint, EndPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if(Hit.collider!= null)
        {
            ActiveMonoBehavior.StartCoroutine(PlayImpact(Hit.transform.position));
        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private IEnumerator PlayImpact(Vector3 ImpactPoint)
    {
        ParticleSystem instance = ImpactParticlePool.Get();
        instance.transform.position = ImpactPoint;
        instance.gameObject.SetActive(true);
        yield return null;

        while (instance.particleCount > 0)
        {
            yield return null;
        }
       
        instance.gameObject.SetActive(false);
        Debug.Log("Instance Deactivated");
        ImpactParticlePool.Release(instance);
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

    public void startImpact(Vector3 StartPoint)
    {
        ActiveMonoBehavior.StartCoroutine(PlayImpact(StartPoint));
    }

    private ParticleSystem CreateParticle()
    {
        GameObject instance = Instantiate(ImpactConfig);
        ParticleSystem particle = instance.GetComponent<ParticleSystem>();

        return particle;
    }
    private GameObject CreateProjectile()
    {
        GameObject instance = Instantiate(ShootConfig.ProjectilePrefab);
        return instance;
    }

    public void ReturnProjectile(GameObject instance)
    {
        instance.gameObject.SetActive(false);
        ProjectilePool.Release(instance);
    }

}
