using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Transform parentObject;
    private float zoomLevel;
    private float minZoom = -30;
    private float maxZoom = -10;
    private float speed = 2; 
    float zoomPosition;
    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
       zoomLevel += Input.mouseScrollDelta.y;
       zoomLevel += Input.GetAxis("Vertical")/10;
       zoomLevel = Mathf.Clamp(zoomLevel, minZoom, maxZoom);
       zoomPosition = Mathf.MoveTowards(zoomPosition, zoomLevel, speed*Time.deltaTime);
       transform.position = parentObject.position + (transform.forward*zoomLevel); 
    }
}
