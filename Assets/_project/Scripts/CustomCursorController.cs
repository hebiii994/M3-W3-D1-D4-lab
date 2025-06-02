using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursorController : MonoBehaviour
{
    [SerializeField] private RectTransform _cursorImageRectTransform;

    
    void Start()
    {
        Cursor.visible = false;

        if (_cursorImageRectTransform == null)
        {
            Debug.LogError("RectTransform del mirino non assegnato a CustomCursorController!");
            this.enabled = false; 
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_cursorImageRectTransform != null)
        {
            
            _cursorImageRectTransform.position = Input.mousePosition;
        }
    }

    private void OnDestroy()
    {
        
        Cursor.visible = true;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.visible = false; 
        }
        else
        {
            Cursor.visible = true; 
        }
    }
}
