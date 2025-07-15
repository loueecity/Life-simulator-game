using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : SingletonMonoBehaviour<Player>
{
   // Movement params for anim controller -- can dumb this down // it's not all needed
   private float xInput;
    private float yInput;
    private bool isCarrying = false;
    private bool isIdle;
    private bool isLiftingToolDown;
    private bool isLiftingToolLeft;
    private bool isLiftingToolRight;
    private bool isLiftingToolUp;
    private bool isRunning;
    private bool isUsingToolDown;
    private bool isUsingToolLeft;
    private bool isUsingToolRight;
    private bool isUsingToolUp;
    private bool isSwingingToolDown;
    private bool isSwingingToolLeft;
    private bool isSwingingToolRight;
    private bool isSwingingToolUp;
    private bool isWalking;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isPickingLeft;
    private bool isPickingRight;
    private ToolEffect toolEffect = ToolEffect.none;
    private Rigidbody2D rigidBody2D;
    private Direction playerDirection;
    private float movementSpeed;
    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value;}
    private GameCursor gameCursor;
    private Camera mainCam;
    //to toggle tool use so players cannot use tools while already using
    private bool toolUseDisabled = false;
    //used for yield in coroutines
    private WaitForSeconds toolAfterUseAnimPause;
    private WaitForSeconds toolUseAnimPause;
    private Character characterStats;
    private List<CharacterAttribute> characterAttributeCustomisationList;
    // Player attributes that can be swapped
    private CharacterAttribute armsCharacterAttribute;
    private AnimationOverrides animationOverrides;
    private CharacterAttribute toolCharacterAttribute;
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;
    protected override void Awake()
    {
        base.Awake();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animationOverrides = GetComponentInChildren<AnimationOverrides>();

        // Initialise swappable character attributes
        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.arms, PartVariantColour.none, PartVariantType.none);
        toolCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.tool, PartVariantColour.none, PartVariantType.axe);

        // Initialise character attribute list
        characterAttributeCustomisationList = new List<CharacterAttribute>();
        mainCam = Camera.main;
    }
    private void Start()
    {
        gameCursor = FindObjectOfType<GameCursor>();
        toolAfterUseAnimPause = new WaitForSeconds(Settings.toolAfterUseAnimPause);
        toolUseAnimPause = new WaitForSeconds(Settings.toolUseAnimPause);
    }

    private void Update()
    {

      if(!PlayerInputIsDisabled){
      ResetAnimationTriggers();
      PlayerMovementInput();
      PlayerClickInput(); //here
      PlayerWalkInput();
      PlayerTestInput();
     

      EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
      isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
      isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
      isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
      isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
      false, false, false, false);
      }
      

    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

     private void PlayerMovement()
    {
        Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime);

        rigidBody2D.MovePosition(rigidBody2D.position + move);
    }
    private void ResetAnimationTriggers()
    {
          isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
        toolEffect = ToolEffect.none;
    }

        private void PlayerMovementInput()
    {
        yInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");

        //diag movement
        if (yInput != 0 && xInput != 0)
        {
           //pythag theorem for diag movement
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }

        if (xInput != 0 || yInput != 0)
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;

            // Capture player direction for save game
            if (xInput < 0)
            {
                playerDirection = Direction.left;
            }
            else if (xInput > 0)
            {
                playerDirection = Direction.right;
            }
            else if (yInput < 0)
            {
                playerDirection = Direction.down;
            }
            else
            {
                playerDirection = Direction.up;
            }
        }
        else if (xInput == 0 && yInput == 0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
    }

     private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
        else
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }
    }

    public void DisableAndReset(){
        DisableInput();
        ResetInput();

        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
      isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
      isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
      isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
      isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
      false, false, false, false);

    }

    private void PlayerClickInput()
    {
        if (!toolUseDisabled)
        {
            if (Input.GetMouseButton(0))
            {
                if (gameCursor.CursorIsEnabled)
                {
                    //Get Cursor Grid Position
                    Vector3Int cursorGridPosition = gameCursor.GetGridPosForCursor();

                     //Get Player Grid Position
                    Vector3Int playerGridPosition = gameCursor.GetGridPosForPlayer();



                    ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
                }
            }
        }
    }

    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        ResetInput();

        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);

        // Get Grid property details at cursor position (the GridCursor validation routine ensures that grid property details are not null)
        GridPropertyInfo gridPropertyInfo = GridPropsManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        // Get Selected item details
        ItemDetails itemDetails = InvManage.Instance.GetSelectedInvItemDetails(InvLoc.player);

        if (itemDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputSeed(gridPropertyInfo,itemDetails);
                    }
                    break;

                case ItemType.Goods:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputGoods(itemDetails);
                    }
                    break;

                case ItemType.HoeTool:
                    ProcessPlayerClickInputTool(gridPropertyInfo, itemDetails, playerDirection);
                    break;
                case ItemType.AxeTool:
                    ProcessPlayerClickInputTool(gridPropertyInfo, itemDetails, playerDirection);
                    break;
                case ItemType.PickaxeTool:
                    ProcessPlayerClickInputTool(gridPropertyInfo, itemDetails, playerDirection);
                    break;
                case ItemType.none:
                    break;

                case ItemType.count:
                    break;

                default:
                    break;
            }
        }
    }

    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        if (cursorGridPosition.x > playerGridPosition.x)
        {
            return Vector3Int.right;
        }
        else if (cursorGridPosition.x < playerGridPosition.x)
        {
            return Vector3Int.left;
        }
        else if (cursorGridPosition.y > playerGridPosition.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }
    }

    private void ProcessPlayerClickInputTool(GridPropertyInfo gridPropertyInfo, ItemDetails itemDetails, Vector3Int playerDirection)
    {
        // Switch on tool
        switch (itemDetails.itemType)
        {
            case ItemType.HoeTool:
                if (gameCursor.CursorPosIsValid)
                {
                    HoeGroundAtCursor(gridPropertyInfo, playerDirection);
                }
                break;
            case ItemType.AxeTool:
                if (gameCursor.CursorPosIsValid)
                {
                    //use energy here
                    if(Character.instance.energy.currentValue >= 5){
                        Character.instance.wcAction(5);
                        ChopInPlayerDirection(gridPropertyInfo, itemDetails, playerDirection); }
                }
                break;
            case ItemType.PickaxeTool:
                if (gameCursor.CursorPosIsValid)
                {
                    if (Character.instance.energy.currentValue >= 5)
                    {
                        Character.instance.useEnergy(5);
                        //BreakInPlayerDirection(gridPropertyInfo, itemDetails, playerDirection);
                        PickaxeHit(gridPropertyInfo, itemDetails);
                        

                    }
                    
                }
                break;

            default:
                break;
        }
    }

    private void PickaxeHit(GridPropertyInfo gridPropertyDetails, ItemDetails equippedItemDetails)
    {
        StartCoroutine(PickaxeHitCoroutine(gridPropertyDetails,equippedItemDetails));
    }

    private IEnumerator PickaxeHitCoroutine(GridPropertyInfo gridPropertyDetails, ItemDetails equippedItemDetails)
    {
        PlayerInputIsDisabled = true;


        ProcessPickaxeHit(gridPropertyDetails,equippedItemDetails);
        yield return toolUseAnimPause;
        PlayerInputIsDisabled = false;
    }

    private void ProcessPickaxeHit(GridPropertyInfo gridPropertyDetails, ItemDetails equippedItemDetails)
    {

        Resource resource = GridPropsManager.Instance.GetResourceObjectAtGridLocation(gridPropertyDetails);
        // Execute Process Tool Action For crop
        if (resource != null)
        {
            if (resource)
                resource.ProcessToolAction(equippedItemDetails, isUsingToolRight, isUsingToolLeft, isUsingToolDown, isUsingToolUp);


        }

    }

    private void ChopInPlayerDirection(GridPropertyInfo gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        // Trigger animation
        StartCoroutine(ChopInPlayerDirectionRoutine(gridPropertyDetails, equippedItemDetails, playerDirection));
    }

    private IEnumerator ChopInPlayerDirectionRoutine(GridPropertyInfo gridPropertyInfo, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        PlayerInputIsDisabled = true;
        //playerToolUseDisabled = true;

        // Set tool animation to axe in override animation
        toolCharacterAttribute.partVariantType = PartVariantType.axe;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        ProcessCropWithEquippedItemInPlayerDirection(playerDirection, equippedItemDetails, gridPropertyInfo);

        yield return toolUseAnimPause;

        // After animation pause
        //yield return afterUseToolAnimationPause;

        PlayerInputIsDisabled = false;
        //playerToolUseDisabled = false;
    }

    private void ProcessCropWithEquippedItemInPlayerDirection(Vector3Int playerDirection, ItemDetails equippedItemDetails, GridPropertyInfo gridPropertyDetails)
    {
        switch (equippedItemDetails.itemType)
        {

            case ItemType.AxeTool:

                if (playerDirection == Vector3Int.right)
                {
                    isUsingToolRight = true;
                }
                else if (playerDirection == Vector3Int.left)
                {
                    isUsingToolLeft = true;
                }
                else if (playerDirection == Vector3Int.up)
                {
                    isUsingToolUp = true;
                }
                else if (playerDirection == Vector3Int.down)
                {
                    isUsingToolDown = true;
                }
                break;

            case ItemType.none:
                break;
        }

        // Get crop at cursor grid location
        Resource resource = GridPropsManager.Instance.GetResourceObjectAtGridLocation(gridPropertyDetails);

        // Execute Process Tool Action For crop
        if (resource != null)
        {
            switch (equippedItemDetails.itemType)
            {
                case ItemType.AxeTool:
                    resource.ProcessToolAction(equippedItemDetails, isUsingToolRight, isUsingToolLeft, isUsingToolDown, isUsingToolUp);
                    break;

            }
        }
    }

    private void ProcessPlayerClickInputGoods(ItemDetails itemDetails)
    {
        if (itemDetails.canDrop && gameCursor.CursorPosIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }

    private void ProcessPlayerClickInputSeed(GridPropertyInfo gridPropertyInfo, ItemDetails itemDetails)
    {
        if (itemDetails.canDrop && gameCursor.CursorPosIsValid && gridPropertyInfo.sinceDug > -1 && gridPropertyInfo.seedID == -1)
        {
            PlantSeedAtCursor(gridPropertyInfo, itemDetails);
        }
        else if (itemDetails.canDrop && gameCursor.CursorPosIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }
    private void PlantSeedAtCursor(GridPropertyInfo gridPropertyInfo, ItemDetails itemDetails)
    {
        // Process if we have cropdetails for the seed
        if (GridPropsManager.Instance.GetResourceDetails(itemDetails.itemID) != null)
        {
            // Update grid properties with seed details
            gridPropertyInfo.seedID = itemDetails.itemID;
            gridPropertyInfo.growthDay = 0;

            // Display planted crop at grid property details
            GridPropsManager.Instance.DisplayPlantedCrop(gridPropertyInfo);

            // Remove item from inventory
            //EventHandler.CallRemoveSelectedItemFromInventoryEvent();

        }

    }
    private void HoeGroundAtCursor(GridPropertyInfo gridPropertyInfo, Vector3Int playerDirection)
    {
        // Trigger animation
        StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyInfo));
    }

    private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyInfo gridPropertyInfo)
    {
        PlayerInputIsDisabled = true;
        toolUseDisabled = true;

        // Set tool animation to hoe in override animation
        //toolCharacterAttribute.partVariantType = PartVariantType.hoe;
        //characterAttributeCustomisationList.Clear();
        //characterAttributeCustomisationList.Add(toolCharacterAttribute);
        //animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        if (playerDirection == Vector3Int.right)
        {
            isUsingToolRight = true;
        }
        else if (playerDirection == Vector3Int.left)
        {
            isUsingToolLeft = true;
        }
        else if (playerDirection == Vector3Int.up)
        {
            isUsingToolUp = true;
        }
        else if (playerDirection == Vector3Int.down)
        {
            isUsingToolDown = true;
        }

        yield return toolUseAnimPause;

        // Set Grid property details for dug ground
        if (gridPropertyInfo.sinceDug == -1)
        {
            gridPropertyInfo.sinceDug = 0;
        }

        // Set grid property to dug
        GridPropsManager.Instance.SetGridPropertyDetails(gridPropertyInfo.xGrid, gridPropertyInfo.yGrid, gridPropertyInfo);

        // After animation pause
        yield return toolAfterUseAnimPause;

        PlayerInputIsDisabled = false;
        toolUseDisabled= false;
    }

    
    public void ResetInput(){
        xInput = 0f;
        yInput = 0f;
        isRunning = false;
        isWalking = false;
        isIdle = true;
    }

    public void DisableInput()
    {
        PlayerInputIsDisabled = true;
    }

    public void EnableInput()
    {
        PlayerInputIsDisabled = false;
    }


//gets where the player is on the camera
    public Vector3 GetPlayerViewportPos(){
        return mainCam.WorldToViewportPoint(transform.position);
    }

    private void PlayerTestInput(){
        if(Input.GetKeyDown(KeyCode.L)){
            SceneManagerController.Instance.FadeAndLoadScene(SceneManagement.Scene1_Farm.ToString(), transform.position);
            
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            TimeManage.Instance.TestAdvanceGameDay();
        }
    }

    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        // Apply base character arms customisation
        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        isCarrying = false;
    }

    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InvManage.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            equippedItemSpriteRenderer.sprite = itemDetails.itemIcon;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            // Apply 'carry' character arms customisation
            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributeCustomisationList.Clear();
            characterAttributeCustomisationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

            isCarrying = true;
        }
    }

}
