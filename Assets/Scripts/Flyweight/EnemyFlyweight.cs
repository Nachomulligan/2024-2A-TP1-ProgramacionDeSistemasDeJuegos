using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyweight
{
    public Transform TownCenter { get; private set; }

    public EnemyFlyweight(Transform townCenter)
    {
        TownCenter = townCenter;
    }

    public void SetTownCenter(Transform newTownCenter)
    {
        TownCenter = newTownCenter;
    }
}
