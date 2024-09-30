using HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public void HandleBuildingDeath(Building building)
    {
        //handles the building's death and starts respawn coroutine
        StartCoroutine(RespawnBuilding(building));
    }

    private IEnumerator RespawnBuilding(Building building)
    {
        yield return new WaitForSeconds(building.respawnTime);

        building.Reactivate();
    }
}
