using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Projectile : MonoBehaviour
{
    public sc_Impact _impactScript = null;
    public sc_GunConfiguration _gunConfig = null;
    public Vector3 _speed;
    public int damageAmount = 2;
    private Vector3 position;

    private void Start()
    {
        position = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //sc_Shootable shootableObject = other.GetComponent<sc_Shootable>();
        Debug.Log("projectile hit");

        /*if (shootableObject != null)
        {
            shootableObject.shoot(damageAmount);
        } 
        

        if (_impactScript != null)
        {
            _impactScript.onImpact(this.transform.position);
        } */

        if(_gunConfig != null)
        {
            StartCoroutine(_gunConfig.PlayImpact(this.transform.position));
            Debug.Log("attempting to play particle?");
        }

        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        position += _speed * Time.deltaTime;

        transform.position = position;
    }
}
