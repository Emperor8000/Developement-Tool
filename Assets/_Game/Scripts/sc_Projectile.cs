using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Projectile : MonoBehaviour
{
    public sc_GunConfiguration _gunConfig = null;
    public Vector3 _speed;
    public int damageAmount = 2;
    public Vector3 position;
    public float missDuration;
    public float remainingDuration;

    public void SetStuff(float Duration, int Damage)
    {
        missDuration = Duration;
        damageAmount = Damage;
        position = this.transform.position;
        remainingDuration = missDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        sc_Shootable shootableObject = other.GetComponent<sc_Shootable>();
        Debug.Log("projectile hit");

        if (shootableObject != null)
        {
            shootableObject.shoot(damageAmount);
        } 
        

        if(_gunConfig != null)
        {
            _gunConfig.startImpact(this.transform.position);
            _gunConfig.ReturnProjectile(this.gameObject);
            Debug.Log("attempting to trigger impact");
        }
    }

    private void Update()
    {
        remainingDuration -= Time.deltaTime;

        if(remainingDuration <= 0)
        {
            _gunConfig.ReturnProjectile(this.gameObject);
        }

        position += _speed * Time.deltaTime;

        transform.position = position;
    }
}
