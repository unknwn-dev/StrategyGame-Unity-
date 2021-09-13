using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Update()
    {

        Settings sett = MainScript.Instance.Settings;

        Vector3 mouseAxis = new Vector3();
        if (Input.GetMouseButton(1))
        {
            mouseAxis = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * sett.CameraSpeed*(Camera.main.orthographicSize/10);
        }

        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, sett.MinZoom, sett.MaxZoom);

        transform.position -= mouseAxis;
        float clampPosX = Mathf.Clamp(transform.position.x, sett.MinCamPos.x + Camera.main.orthographicSize * Camera.main.aspect, sett.MaxCamPos.x - Camera.main.orthographicSize * Camera.main.aspect);
        float clampPosY = Mathf.Clamp(transform.position.y, sett.MinCamPos.y + Camera.main.orthographicSize, sett.MaxCamPos.y - Camera.main.orthographicSize);
        transform.position = new Vector3(clampPosX, clampPosY, -10);
    }
}
