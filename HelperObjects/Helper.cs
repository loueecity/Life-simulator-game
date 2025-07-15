using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    //helper objects for methods that can be called in many different classes
   
    //gets the comps of an area parsed into the method 
    //generic component types
    public static bool GetComponentsAtBoxLocation<T>(out List<T> listComponentsAtBoxPosition, Vector2 point, Vector2 size, float angle)
    {
        bool found = false; //sets found to false
        List<T> componentList = new List<T>(); //adds to list of generic types e.g Type Item

        //checks the colliders present at the location
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(point, size, angle);

        // loops through the collider arrays for components for the generic type given in the method
        for (int i = 0; i < collider2DArray.Length; i++)
        {
            T tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>();
            if (tComponent != null)
            {
                found = true;
                componentList.Add(tComponent); //adds to list of the components
            }
            else
            { //if none are in the parent component
                tComponent = collider2DArray[i].gameObject.GetComponentInChildren<T>();
                if (tComponent != null)
                {
                    found = true;
                    componentList.Add(tComponent);
                }
            }
        }

        //gives the list of the components
        listComponentsAtBoxPosition = componentList;

        //returns found 
        return found;
    }
}
