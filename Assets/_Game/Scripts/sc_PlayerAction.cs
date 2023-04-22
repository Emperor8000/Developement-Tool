using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class sc_PlayerAction : MonoBehaviour
{
    [SerializeField] private sc_GunSelector GunSelector;

    private void Update()
    {
        if(Mouse.current.leftButton.isPressed && GunSelector.ActiveGun != null && GunSelector.ActiveGun.ShootConfig.IsAutomatic)
        {
            GunSelector.ActiveGun.Shoot();
        } else if(Mouse.current.leftButton.wasPressedThisFrame && GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.Shoot();
        }
    }
}
