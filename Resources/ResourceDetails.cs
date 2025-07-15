using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResourceDetails
{
    [ItemIDDesc]
    public int resourceItemID;  // this is the item code for the corresponding seed
    public int[] growthDays; // days growth for each stage
    //public int totalRespawnDays; // total growth days
    public GameObject[] growthPrefab;// prefab to use when instantiating growth stages
    public Sprite[] growthSprite; // growth sprite
    public Season[] seasons; // growth seasons
    public Sprite harvestedSprite; // sprite used once harvested
    [ItemIDDesc]
    public int gatherTransformID; // if the item transforms into another item when harvested this item code will be populated
    public bool hideCropBeforeHarvestedAnimation; // if the crop should be disabled before the harvested animation
    public bool disableCropCollidersBeforeHarvestedAnimation;// if colliders on crop should be disabled to avoid the harvested animation effecting any other game objects
    public bool isHarvestedAnimation; // true if harvested animation to be played on final growth stage prefab
    public bool isHarvestActionEffect = false; // flag to determine whether there is a harvest action effect
    public bool spawnResourceProducedAtPlayerPos;
    //public HarvestActionEffect harvestActionEffect; // the harvest action effect for the crop
    [ItemIDDesc]
    public int[] GatheringToolItemID; // array of item codes for the tools that can harvest or 0 array elements if no tool required
    public int[] requiredGatheringActions; // number of harvest actions required for corressponding tool in harvest tool item code array
    [ItemIDDesc]
    public int[] resourceGatheredItemID; // array of item codes produced for the harvested crop
    public int[] resourceGatheredMinQuantity; // array of minimum quantities produced for the harvested crop
    public int[] resourceGatheredMaxQuantity; // if max quantity is > min quantity then a random number of crops between min and max are produced
    public int daysToRespawn; // days to regrow next crop or -1 if a single crop


    
    public bool CanUseToolToGatherResource(int toolItemCode)
    {
        if (RequiredGatherActionsForTool(toolItemCode) == -1)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public int RequiredGatherActionsForTool(int toolItemCode)
    {
        for (int i = 0; i < GatheringToolItemID.Length; i++)
        {
            if (GatheringToolItemID[i] == toolItemCode)
            {
                return requiredGatheringActions[i];
            }
        }
        return -1;
    }
}
