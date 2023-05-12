using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class sc_Shootable : MonoBehaviour
{
    public UnityEvent<int> Shot;

    public void shoot(int damageAmount)
    {
        Debug.Log("shot");
        Shot?.Invoke(damageAmount);
    }
}
