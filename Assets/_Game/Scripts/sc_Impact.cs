using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class sc_Impact : MonoBehaviour
{
    public UnityEvent<Vector3> Impact;

    public void onImpact(Vector3 impactPos)
    {
        Debug.Log("projectileImpact");
        Impact?.Invoke(impactPos);
    }
}
