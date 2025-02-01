using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraScreenResolution : MonoBehaviour
    {
        public CinemachineBrain cbrain;

        CinemachineCamera currentCam;
        float sceneWidth = 28.45f;

        private void Update()
        {
            currentCam = cbrain.ActiveVirtualCamera as CinemachineCamera;
            if (!currentCam) return;

            float unitsPerPixel = sceneWidth / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
            currentCam.Lens.OrthographicSize = desiredHalfHeight;

        }
    }
}
