using Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraScreenResolution : MonoBehaviour
    {
        public CinemachineBrain cbrain;

        CinemachineVirtualCamera currentCam;
        float sceneWidth = 28.45f;

        private void Start()
        {
            currentCam = cbrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            currentCam = cbrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

            float unitsPerPixel = sceneWidth / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
            currentCam.m_Lens.OrthographicSize = desiredHalfHeight;

        }
    }
}
