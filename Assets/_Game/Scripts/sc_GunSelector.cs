using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
//[ExecuteInEditMode]
public class sc_GunSelector : MonoBehaviour
{
    [SerializeField] private sc_GunType Gun;
    [SerializeField] private Transform GunParent;
    [SerializeField] private List<sc_GunConfiguration> Guns;

    [Space]
    [Header("Runtime Filled")]
    public sc_GunConfiguration ActiveGun;

    private void Start()
    {
        sc_GunConfiguration gun = Guns.Find(gun => gun.Type == Gun);

        if (gun == null)
        {
            Debug.Log("No gun found");
        }

        ActiveGun = gun;
        if (gun != null)
        {
            gun.Spawn(GunParent, this);
        }
    }
}
