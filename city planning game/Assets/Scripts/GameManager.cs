using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MouseInput mouseInput;
    public GameObject testCube;

    public Building heldBuilding;

    // Start is called before the first frame update
    private void Start()
    {
        mouseInput.OnMouseDown += HandleMouseClick;
    }

    private void HandleMouseClick(Vector3 pos)
    {
        Debug.Log(pos);
        // PlaceGameObject(testCube, pos);

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // private void PlaceGameObject(GameObject obj, Vector3 pos){
    //     Instantiate(obj, pos, Quaternion.identity);
    // }
    // private void PlaceBuilding(Building building, Vector3 pos){
        
    // }
}
