using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OutVoker : MonoBehaviour
{
    public LayerMask CastLayer;
    private Vector3 targetPosition = new Vector3();
    
    private void Start()
    {
        
    }

    private void Update()
    {
        Ray mouseWorldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        #region get mouse position
        
        if (Physics.Raycast(mouseWorldPosition, out RaycastHit raycastHit, CastLayer))
        {
            // transform.position = raycastHit.point;
            targetPosition = raycastHit.point;
        }

        #endregion
        
        // movement
        
        if (Input.GetMouseButton(1))
        {
            transform.position = targetPosition;
            
            
        }
        
        // look to the mouse position
        else if (Input.GetKey(KeyCode.F) == true)
        {
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        }

    }
}
