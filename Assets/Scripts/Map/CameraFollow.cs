using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script must be attached to the main camera. This will make the camera follow the given object. In this case, the player. */
public class CameraFollow : MonoBehaviour
{
    public Transform objectTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(objectTransform.position.x, objectTransform.position.y, transform.position.z);
    }

}
