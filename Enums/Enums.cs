//used if tool has an effect
public enum ToolEffect
{
    none,
    watering
}

//types of items
public enum ItemType
{
    Seed,
    Goods,
    WateringTool,
    HoeTool,
    AxeTool,
    PickaxeTool,
    ScytheTool,
    NetTool,
    CollectorTool,
    Reapable,
    Furnishing,
    none,
    count,

}

//for movement
public enum Direction
{
    up,
    down,
    left,
    right,
    none
}

public enum GridBoolProperties
{
    isDiggable,
    canDrop,
    canPlaceFurn,
    isPath,
    isNPCObs,

}

//refers to inventory location for different storages e.g player sotrage, chest storage 
public enum InvLoc
{
    player,
    chest,
    count
}

//enum for scenes to be changed, add more scenes etc
public enum SceneManagement{
    Scene1_Farm,
    Scene2_Mine,
    Scene3_Home
}

//used if seasons are used
public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter,
    none,
    count
}

public enum AnimationName
{
    idleDown,
    idleUp,
    idleRight,
    idleLeft,
    walkUp,
    walkDown,
    walkRight,
    walkLeft,
    runUp,
    runDown,
    runRight,
    runLeft,
    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,
    swingToolUp,
    swingToolDown,
    swingToolRight,
    swingToolLeft,
    liftToolUp,
    liftToolDown,
    liftToolRight,
    liftToolLeft,
    holdToolUp,
    holdToolDown,
    holdToolRight,
    holdToolLeft,
    pickDown,
    pickUp,
    pickRight,
    pickLeft,
    count
}

public enum PartVariantColour
{
    none,
    count
}

public enum PartVariantType
{
    none,
    carry,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}

public enum CharacterPartAnimator
{
    body,
    arms,
    hair,
    tool,
    hat,
    count
}