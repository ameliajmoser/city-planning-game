using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Thanks to https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/ 
public class Util : MonoBehaviour
{
    //Returns 'true' if we touched or hovering on Unity Layer element.
    public static bool IsPointerOverLayer( int layer )
    {
        return IsPointerOverLayer( layer, GetEventSystemRaycastResults() );
    }
 
    //Returns 'true' if we touched or hovering on Unity Layer
    private static bool IsPointerOverLayer( int layer, List<RaycastResult> eventSystemRaysastResults )
    {
        for ( int index = 0; index < eventSystemRaysastResults.Count; index++ )
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if ( curRaysastResult.gameObject.layer == layer )
                return true;
        }
        return false;
    }
 

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData( EventSystem.current );
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );
        return raysastResults;
    }

    // Following method is used to retrive the relative path
    public static string GetPath()
    {
    #if UNITY_EDITOR
        return Application.dataPath + "/Resources/";
    #else
        return Application.dataPath + '/';
    #endif
    }
}