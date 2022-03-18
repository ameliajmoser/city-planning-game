using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    public Action<Vector3> OnMouseDown, OnMouseHold;
    public Action OnMouseUp;
    public LayerMask groundMask;

    [SerializeField]
    Camera mainCamera;

    private void Update(){
        CheckClickDownEvent();
        CheckClickUpEvent();
        CheckClickHoldEvent();
    }

    private Vector3? RaycastGround(){
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask)){
            return hit.point;
        }
        return null;
    }
    private void CheckClickHoldEvent()
    {
        if(Input.GetMouseButton(0)  && !EventSystem.current.IsPointerOverGameObject()){
            var pos = RaycastGround();
            if (pos != null){
                OnMouseHold?.Invoke(pos.Value);
                //Debug.Log("Hold: " + pos.Value);
            }
        }
    }

    private void CheckClickUpEvent()
    {
        if(Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()){
            OnMouseUp?.Invoke();
        }

    }

    private void CheckClickDownEvent()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){
            var pos = RaycastGround();
            if (pos != null){
                OnMouseDown?.Invoke(pos.Value);
                //Debug.Log("Down: " + pos.Value);
            }
        }
    }
}
