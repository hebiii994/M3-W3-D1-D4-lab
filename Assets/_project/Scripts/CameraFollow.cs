using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    // Start is called before the first frame update
    
    void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 currentCameraPos = transform.position;

            Vector3 newCamerapos = new Vector3(_target.position.x, _target.position.y, currentCameraPos.z);

            transform.position = newCamerapos;
        }

    }
}
