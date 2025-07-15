using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : MonoBehaviour
{
    //variables
    private Canvas canvas;
    private Grid grid;
    private Camera mainCamera;
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorIcon = null;
    [SerializeField] private Sprite redCursorIcon = null;

    private bool _cursorPosIsValid = false;
    public bool CursorPosIsValid { get => _cursorPosIsValid; set => _cursorPosIsValid = value; }

    private int _itemUseGridRadius = 0;
    public int ItemUseGridRadius { get => _itemUseGridRadius; set => _itemUseGridRadius = value; }

    private ItemType _selectedItemType;
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private bool _cursorIsEnabled = false;
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (CursorIsEnabled)
        {
            DisplayCursor();
        }
    }

    private Vector3Int DisplayCursor()
    {
        if (grid != null)
        {
            // Get grid position for cursor
            Vector3Int gridPosition = GetGridPosForCursor();

            // Get grid position for player
            Vector3Int playerGridPosition = GetGridPosForPlayer();

            // Set cursor sprite
            SetCursorValidity(gridPosition, playerGridPosition);

            // Get rect transform position for cursor
            cursorRectTransform.position = GetRectTransformPosForCursor(gridPosition);

            return gridPosition;
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    private void SceneLoaded()
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }

    private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        //makes cursor valid/green
        cursorImage.sprite = greenCursorIcon;
        CursorPosIsValid = true;

        // Check item use radius is valid
        if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUseGridRadius
            || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUseGridRadius)
        {
            //invalidates cursor turns it red
            cursorImage.sprite = redCursorIcon;
            CursorPosIsValid = false;

            return;
        }

        // Get selected item details
        ItemDetails itemDetails = InvManage.Instance.GetSelectedInvItemDetails(InvLoc.player);

        if (itemDetails == null)
        {
            //invalidates cursor turns it red
            cursorImage.sprite = redCursorIcon;
            CursorPosIsValid = false;
            return;
        }

        // Get grid property details at cursor position
        GridPropertyInfo gridPropertyInfo = GridPropsManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        if (gridPropertyInfo != null)
        {
            // Determine cursor validity based on inventory item selected and grid property details
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (!cursorValidForSeeds(gridPropertyInfo))
                    {
                        //invalidates cursor turns it red
                        cursorImage.sprite = redCursorIcon;
                        CursorPosIsValid = false;
                        return;
                    }
                    break;
                    //case for the game cursor
                case ItemType.Goods:

                    if (!cursorValidForGoods(gridPropertyInfo)){
                        //invalidates cursor turns it red
                        cursorImage.sprite = redCursorIcon;
                        CursorPosIsValid = false;
                        return;
                    }
                    break;

                case ItemType.HoeTool:
                    if (!cursorValidForTool(gridPropertyInfo, itemDetails))
                    {
                        //invalidates cursor turns it red
                        cursorImage.sprite = redCursorIcon;
                        CursorPosIsValid = false;
                        return;
                    }
                    break;

                case ItemType.none:
                    break;

                case ItemType.count:
                    break;

                default:
                    break;
            }
        }
        else
        {
            //invalidates cursor turns it red
            cursorImage.sprite = redCursorIcon;
            CursorPosIsValid = false;
            return;
        }
    }


    /// <summary>
    /// Test cursor validity for a commodity for the target gridPropertyDetails. Returns true if valid, false if invalid
    /// </summary>
    private bool cursorValidForGoods(GridPropertyInfo gridPropertyInfo)
    {
        return gridPropertyInfo.canDrop;

    }

    /// <summary>
    /// Set cursor validity for a seed for the target gridPropertyDetails. Returns true if valid, false if invalid
    /// </summary>
    private bool cursorValidForSeeds(GridPropertyInfo gridPropertyInfo)
    {
        return gridPropertyInfo.canDrop;

    }

    private bool cursorValidForTool(GridPropertyInfo gridPropertyInfo,ItemDetails itemDetails)
    {
        // switch cases depending on the different tool types not all may be implemented
        switch (itemDetails.itemType)
        {
            //for farming the ground needs to be ploughed by a hoe
            case ItemType.HoeTool:
                //checks if the player can dig at the ground tile and if it has already been dug
                if (gridPropertyInfo.isDiggable == true && gridPropertyInfo.sinceDug == -1)
                {
                    
                    // to retrieve the world position of the cursor
                    Vector3 cursorWorldPosition = new Vector3(GetWorldPosForCursor().x + 0.5f, GetWorldPosForCursor().y + 0.5f, 0f);

                    // Get list of items at cursor location
                    List<Item> itemList = new List<Item>();

                    Helper.GetComponentsAtBoxLocation<Item>(out itemList, cursorWorldPosition, Settings.cursorSize, 0f);
                    

                    // Loop through items found to see if any are reapable type - we are not going to let the player dig where there are reapable scenary items
                    bool foundReapable = false;

                    foreach (Item item in itemList)
                    {
                        if (InvManage.Instance.GetItemDetails(item.ItemID).itemType == ItemType.Reapable)
                        {
                            foundReapable = true;
                            break;
                        }
                    }

                    if (foundReapable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            case ItemType.AxeTool:
                
            case ItemType.PickaxeTool:
            
            default:
                return false;
        }
    }

    //disables the cursor from the view
    public void DisableCursor()
    {
        cursorImage.color = Color.clear;

        CursorIsEnabled = false;
    }

    //enables the cursor to be seen
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 1f);
        CursorIsEnabled = true;
    }

    
    //
    //
    //
    //
    //This section is for getting the grid positions for cursor and player
    public Vector3Int GetGridPosForCursor()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));  // z is how far the objects are in front of the camera - camera is at -10 so objects are (-)-10 in front = 10
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 GetWorldPosForCursor()
    {
        return grid.CellToWorld(GetGridPosForCursor());
    }
    public Vector3Int GetGridPosForPlayer()
    {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    public Vector2 GetRectTransformPosForCursor(Vector3Int gridPos)
    {
        Vector3 gridWorldPos = grid.CellToWorld(gridPos);
        Vector2 gridScreenPos = mainCamera.WorldToScreenPoint(gridWorldPos);
        return RectTransformUtility.PixelAdjustPoint(gridScreenPos, cursorRectTransform, canvas);
    }
}
