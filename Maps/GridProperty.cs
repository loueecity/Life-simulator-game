[System.Serializable]
public class GridProperty
{ 
    
    public bool gridBoolVal = false; //values
    public GridBoolProperties gridBoolProperties; //property
    public GridCoordinate gridCoord; //coordinate
    

    public GridProperty(GridCoordinate gridCoord, GridBoolProperties gridBoolProperties, bool gridBoolVal)
    {
        this.gridBoolVal = gridBoolVal;
        this.gridBoolProperties = gridBoolProperties;
        this.gridCoord = gridCoord;
    }
}