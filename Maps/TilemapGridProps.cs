using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapGridProps : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private SO_GridProps gridProps = null;
    [SerializeField] private GridBoolProperties gridBoolProperties = GridBoolProperties.isDiggable; //changeable in editor

    private void OnEnable()
    {
        // populates in the editor
        if (!Application.IsPlaying(gameObject))
        {
            tilemap = GetComponent<Tilemap>(); //populates the tilemap field with the tilemap component
            if (gridProps != null) //checks if the scriptable object is added
            {
                gridProps.gridPropertyList.Clear();
            }
        }
    }

    private void OnDisable()
    {        
        if (!Application.IsPlaying(gameObject))
        {
            UpdateGridProperties();

            if (gridProps != null)
            {
               
                EditorUtility.SetDirty(gridProps);
            }
        }
    }

    private void UpdateGridProperties()
    {
       
        tilemap.CompressBounds(); //reduces the tilemap size to the area given 
        //as tilemaps still expand if tiles were placed outside of the given area 

    
        if (!Application.IsPlaying(gameObject)) //for when the game is not running
        {
            if (gridProps != null) //checks for scriptable object
            {
                Vector3Int firstCell = tilemap.cellBounds.min; //bounds of the tilemap
                Vector3Int lastCell = tilemap.cellBounds.max; //bounds of the tilemap

                for (int x = firstCell.x; x < lastCell.x; x++) //looping through the tiles to get the properties
                {
                    for (int y = firstCell.y; y < lastCell.y; y++)
                    {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0)); //for every x/y pos 0 is for z which isnt used as game is 2D

                        if (tile != null) //checks if hte tile is null
                        {
                            gridProps.gridPropertyList.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperties, true));
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {        //updates while the game is not playing
        if (!Application.IsPlaying(gameObject))
        {
            Debug.Log("disable tilemap props");
        }
    }
}