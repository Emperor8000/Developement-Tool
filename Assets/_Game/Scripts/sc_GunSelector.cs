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
    public sc_GunConfiguration[] allGuns;

    //[Space]
    //[Header("Runtime Filled")]
    public sc_GunConfiguration ActiveGun;

    private void Start()
    {
        string fullPath = $"{Application.dataPath}/{Guns}";
        if (!System.IO.Directory.Exists(fullPath))
        {
            return;
        }

        var folders = new string[] { $"Assets/_Game/Scriptable Objects/{Guns}" };
        var guides = AssetDatabase.FindAssets("t:sc_GunConfiguration", folders);

        var GunsList = new sc_GunConfiguration[guides.Length];

        bool mismatch;
        if (allGuns == null)
        {
            mismatch = true;
            allGuns = GunsList;
        }
        else
        {
            mismatch = GunsList.Length != GunsList.Length;
        }

        for (int i = 0; i < GunsList.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guides[i]);
            GunsList[i] = AssetDatabase.LoadAssetAtPath<sc_GunConfiguration>(path);
            mismatch |= (i < allGuns.Length && allGuns[i] != GunsList[i]);
        }

        if (mismatch)
        {
            allGuns = GunsList;
        }

        for(int i = 0; i < allGuns.Length; i++)
        {
            Guns.Add(allGuns[i]);
        }

        sc_GunConfiguration gun = Guns.Find(gun => gun.Name == ActiveGunName);

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

    public void SwitchWeapon(sc_GunConfiguration newActiveGun)
    {
        //I think I don't have time for this
    }
}
