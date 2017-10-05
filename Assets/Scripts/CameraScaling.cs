using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaling : MonoBehaviour
{

    public float orthographicSize = 5; //Size of camera in orthographic mode, as noted in the inspector.
    public float aspect = 1.33333f; //4:3, 16:9 etc as a decimal (4/3, 16/9)
    private Camera cam; //Reference to this camera.
    void Start()
    {
        cam = GetComponent<Camera>();

        /*Resizes the camera projection matrix to ensure it is always the same scale, no matter the current size.
        This could result in stretching, but will ensure objects are always in the same viewport on all screens.*/
        cam.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                cam.nearClipPlane, cam.farClipPlane);
    }
}