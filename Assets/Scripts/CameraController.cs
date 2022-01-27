using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Settings sett = Settings.Instance;
    Vector3 touchStart = new Vector3();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.touchCount == 2)
        {
            Debug.Log("zoom");

            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevMagn = (t0Prev - t1Prev).magnitude;
            float currMagn = (t0.position - t1.position).magnitude;

            float diff = currMagn - prevMagn;

            Zoom(diff * 0.001f);
        }
        
        else if (Input.GetMouseButton(0))
        {
            transform.position += touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float clampPosX = Mathf.Clamp(transform.position.x, sett.MinCamPos.x + Camera.main.orthographicSize * Camera.main.aspect, sett.MaxCamPos.x - Camera.main.orthographicSize * Camera.main.aspect);
            float clampPosY = Mathf.Clamp(transform.position.y, sett.MinCamPos.y + Camera.main.orthographicSize, sett.MaxCamPos.y - Camera.main.orthographicSize);
            transform.position = new Vector3(clampPosX, clampPosY, -10);
        }
    }

    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, sett.MinZoom, sett.MaxZoom);
    }
}
