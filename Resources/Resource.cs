using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int harvestActionCount = 0;

    [Tooltip("This should be populated from child gameobject")]
    [SerializeField] private SpriteRenderer cropHarvestedSpriteRenderer = null;
    //this class is for resources eg rocks/trees/NPCs for hunter
    [HideInInspector] 
    public Vector2Int resourceGridPos;

    public void ProcessToolAction(ItemDetails equippedItemDetails, bool isToolRight, bool isToolLeft, bool isToolDown, bool isToolUp)
    {
        // Get grid property details
        GridPropertyInfo gridPropertyInfo = GridPropsManager.Instance.GetGridPropertyDetails(resourceGridPos.x, resourceGridPos.y);

        if (gridPropertyInfo == null)
            return;

        // Get seed item details
        ItemDetails seedItemDetails = InvManage.Instance.GetItemDetails(gridPropertyInfo.seedID);
        if (seedItemDetails == null)
            return;

        // Get crop details
        ResourceDetails resourceDetails = GridPropsManager.Instance.GetResourceDetails(seedItemDetails.itemID);
        if (resourceDetails == null)
            return;

        // Get animator for crop if present
        Animator animator = GetComponentInChildren<Animator>();

        // Trigger tool animation
        if (animator != null)
        {
            if (isToolRight || isToolUp)
            {
                animator.SetTrigger("isUsingToolRight");
            }
            else if (isToolLeft || isToolDown)
            {
                animator.SetTrigger("isUsingToolLeft");
            }
        }


        // Get required harvest actions for tool
        int requiredHarvestActions = resourceDetails.RequiredGatherActionsForTool(equippedItemDetails.itemID);
        if (requiredHarvestActions == -1)
            return; // this tool can't be used to harvest this crop


        // Increment harvest action count
        harvestActionCount += 1;

        // Check if required harvest actions made
        if (harvestActionCount >= requiredHarvestActions)
            HarvestCrop(isToolRight, isToolUp, resourceDetails, gridPropertyInfo, animator);
    }

    public void NewProcessToolAction(ItemDetails equippedItemDetails)
    {
        // Get grid property details
        GridPropertyInfo gridPropertyInfo = GridPropsManager.Instance.GetGridPropertyDetails(resourceGridPos.x, resourceGridPos.y);

        if (gridPropertyInfo == null)
            return;

        // Get seed item details
        ItemDetails seedItemDetails = InvManage.Instance.GetItemDetails(gridPropertyInfo.seedID);
        if (seedItemDetails == null)
            return;

        // Get crop details
        ResourceDetails resourceDetails = GridPropsManager.Instance.GetResourceDetails(seedItemDetails.itemID);
        if (resourceDetails == null)
            return;

        // Get animator for crop if present
        Animator animator = GetComponentInChildren<Animator>();

        // Get required harvest actions for tool
        int requiredHarvestActions = resourceDetails.RequiredGatherActionsForTool(equippedItemDetails.itemID);
        if (requiredHarvestActions == -1)
            return; // this tool can't be used to harvest this crop


        // Increment harvest action count
        harvestActionCount += 1;

      
    }

    private void HarvestCrop(bool isUsingToolRight, bool isUsingToolUp, ResourceDetails resourceDetails, GridPropertyInfo gridPropertyInfo, Animator animator)
    {

        // Is there a harvested animation
        if (resourceDetails.isHarvestedAnimation && animator != null)
        {
            // If harvest sprite then add to sprite renderer
            if (resourceDetails.harvestedSprite != null)
            {
                if (cropHarvestedSpriteRenderer != null)
                {
                    cropHarvestedSpriteRenderer.sprite = resourceDetails.harvestedSprite;
                }
            }

            if (isUsingToolRight || isUsingToolUp)
            {
                animator.SetTrigger("harvestright");
            }
            else
            {
                animator.SetTrigger("harvestleft");
            }
        }

        // Delete crop from grid properties
        gridPropertyInfo.seedID = -1;
        gridPropertyInfo.growthDay = -1;
        gridPropertyInfo.sinceHarvest = -1;
        //gridPropertyInfo.daysSinceWatered = -1;

        // Should the crop be hidden before the harvested animation
        if (resourceDetails.hideCropBeforeHarvestedAnimation)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        GridPropsManager.Instance.SetGridPropertyDetails(gridPropertyInfo.xGrid, gridPropertyInfo.yGrid, gridPropertyInfo);

        // Is there a harvested animation - Destroy this crop game object after animation completed
        if (resourceDetails.isHarvestedAnimation && animator != null)
        {
            StartCoroutine(ProcessHarvestActionsAfterAnimation(resourceDetails, gridPropertyInfo, animator));
        }
        else
        {

            HarvestActions(resourceDetails, gridPropertyInfo);
        }
    }

    private IEnumerator ProcessHarvestActionsAfterAnimation(ResourceDetails resourceDetails, GridPropertyInfo gridPropertyInfo, Animator animator)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Harvested"))
        {
            yield return null;
        }

        HarvestActions(resourceDetails, gridPropertyInfo);
    }

    private void HarvestActions(ResourceDetails resourceDetails, GridPropertyInfo gridPropertyInfo)
    {
        SpawnHarvestedItems(resourceDetails);

        Destroy(gameObject);
    }

    private void SpawnHarvestedItems(ResourceDetails resourceDetails)
    {
        // Spawn the item(s) to be produced
        for (int i = 0; i < resourceDetails.resourceGatheredItemID.Length; i++)
        {
            int resourcesToProduce;

            // Calculate how many crops to produce
            if (resourceDetails.resourceGatheredMaxQuantity[i] == resourceDetails.resourceGatheredMaxQuantity[i] ||
                resourceDetails.resourceGatheredMinQuantity[i] < resourceDetails.resourceGatheredMinQuantity[i])
            {
                resourcesToProduce = resourceDetails.resourceGatheredMinQuantity[i];
            }
            else
            {
                resourcesToProduce = Random.Range(resourceDetails.resourceGatheredMinQuantity[i], resourceDetails.resourceGatheredMaxQuantity[i] + 1);
            }

            for (int j = 0; j < resourcesToProduce; j++)
            {
                Vector3 spawnPosition;
                if (resourceDetails.spawnResourceProducedAtPlayerPos)
                {
                    //  Add item to the players inventory
                    InvManage.Instance.AddItem(InvLoc.player, resourceDetails.resourceGatheredItemID[i]);
                }
                else
                {
                    // Random position
                    spawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0f);
                    SceneItemManager.Instance.InstantiateSceneItem(resourceDetails.resourceGatheredItemID[i], spawnPosition);
                }
            }
        }
    }


   
}
