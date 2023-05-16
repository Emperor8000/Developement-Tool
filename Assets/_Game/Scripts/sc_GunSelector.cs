using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class sc_GunSelector : MonoBehaviour
{
    [SerializeField] private string ActiveGunName;
    [SerializeField] public Transform GunParent;
    [SerializeField] public List<sc_GunConfiguration> Guns;

    //[Space]
    //[Header("Runtime Filled")]
    public sc_GunConfiguration ActiveGun;

    private void Start()
    { 
        sc_GunConfiguration gun = Guns.Find(gun => gun.Name == ActiveGunName);

        if (gun == null)
        {
            Debug.Log("No gun found");
        }

        ActiveGun = gun;

        if (gun != null)
        {
            GameObject previousGun = GameObject.Find(gun.name + "(Clone)"); //search for a previous gun of this type

            if (!previousGun)
            {
                gun.Spawn(GunParent, this);
            }
            else
            {
                gun.Setup(GunParent, this, previousGun);
            }
        }
    }

    public void SwitchWeapon(sc_GunConfiguration newActiveGun)
    {
        //I think I don't have time for this
    }
}
