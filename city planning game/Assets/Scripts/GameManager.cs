using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MouseInput mouseInput;
    public GameObject testCube;

    // Start is called before the first frame update
    private void Start()
    {
        mouseInput.OnMouseDown += HandleMouseClick;
    }

    private void HandleMouseClick(Vector3 pos)
    {
        Debug.Log(pos);
        Instantiate(testCube, pos, Quaternion.identity);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
