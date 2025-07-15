using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Creates trees on game run
public class ResourceInstantiator : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private int sinceDug = -1;
    [SerializeField] private int sinceWatered = -1;
    [ItemIDDesc]
    [SerializeField] private int seedID = 0;
    [SerializeField] private int growthDay = 0;


    private void OnEnable()
    {
        EventHandler.InstantiateResourcePrefabsEvent += InstantiateResourcePrefabs;
    }

    private void SetResourceGridProperties(Vector3Int resourceGridPosition)
    {
        if (seedID > 0)
        {
            GridPropertyInfo gridPropertyInfo;
            gridPropertyInfo = GridPropsManager.Instance.GetGridPropertyDetails(resourceGridPosition.x, resourceGridPosition.y);

            if (gridPropertyInfo == null)
            {
                gridPropertyInfo = new GridPropertyInfo();
            }
            gridPropertyInfo.sinceDug = sinceDug;
            gridPropertyInfo.sinceWatered = sinceWatered;
            gridPropertyInfo.seedID = seedID;
            gridPropertyInfo.growthDay = growthDay;

            GridPropsManager.Instance.SetGridPropertyDetails(resourceGridPosition.x, resourceGridPosition.y, gridPropertyInfo);
        }
    }

    private void InstantiateResourcePrefabs()
    {
        //get grid
        grid = GameObject.FindObjectOfType<Grid>();

        //get grid pos for the resource
        Vector3Int resourceGridPos = grid.WorldToCell(transform.position);

        //set resource props
        SetResourceGridProperties(resourceGridPos);

        Destroy(gameObject);

    }
}
