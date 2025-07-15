using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPropertyInfo
{
    //coordinate properties
    public int xGrid;
    public int yGrid;

    //tile property bools
    public bool isDiggable = false; //for planting trees or farming
    public bool canDrop = false; //to stop items dropping out of the player area
    public bool canPlaceFurn = false; //for player home
    public bool isPath = false; //used for butterfly npcs for hunter
    public bool isNPCObs = false; //used for butterfly npcs for hunter

    //tile info for tiles manipulated by player
    public int sinceDug = -1; //if dug
    public int sinceWatered = -1; //if watered
    public int seedID = -1; //for item growing in tile
    public int growthDay = -1; //growth stage
    public int sinceHarvest = -1; //if harvested

    public GridPropertyInfo()
    {

    }

}
