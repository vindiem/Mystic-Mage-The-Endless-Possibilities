using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraMouseControl : MonoBehaviour
{
    public float sensitivity = 1f;
    public float zoomSpeed = 1f;
    public float minFOV = 30f;
    public float maxFOV = 60f;

    private float currentZoom = 0f;
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            // -1 - +1

            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, -10f, 10f);
            transform.position += transform.forward * currentZoom;

            virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(virtualCamera.m_Lens.FieldOfView - 
                scroll * zoomSpeed, minFOV, maxFOV);
        }
    }

    // Mobile version
    public void ScrollBarSetValue(Scrollbar scrollbar)
    {
        float scroll = scrollbar.value;
        // 0 - +1
        scroll -= 0.5f;
        scroll *= 2f;

        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, -10f, 10f);
        transform.position += transform.forward * currentZoom;

        virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(virtualCamera.m_Lens.FieldOfView - 
            scroll * zoomSpeed, minFOV, maxFOV);
    }

}