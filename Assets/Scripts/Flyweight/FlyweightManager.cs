using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class FlyweightManager : MonoBehaviour
{
    public Transform TownCenter { get; private set; }

    private void Awake()
    {
        var townCenterObject = GameObject.FindGameObjectWithTag("TownCenter");
        if (townCenterObject == null)
        {
            Debug.LogError("No se encontró el Town Center.");
            return;
        }
        TownCenter = townCenterObject.transform;
    }
}