using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PrefabsData : MonoBehaviour
{
    public GameObject bloodParticles;
    public static PrefabsData Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
