using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "New Gun", menuName = "Guns/Gun", order = 0)]
public class sc_GunConfiguration : ScriptableObject
{
    [Header ("Organize Visuals and Logic")]
    public sc_GunType Type;
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

                if(Physics.Raycast(ShootParticle.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
                {
                    ActiveMonoBehavior.StartCoroutine(PlayTrail(ShootParticle.transform.position, hit.point, hit));
                }
                else
                {
                    ActiveMonoBehavior.StartCoroutine(PlayTrail(ShootParticle.transform.position, 
                        ShootParticle.transform.position + (shootDirection * TrailConfig.MissDistance), new RaycastHit()));
                }
            }else
            {
                Debug.Log("No Particle System Assigned");
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
            //I'll make this later
        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
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
