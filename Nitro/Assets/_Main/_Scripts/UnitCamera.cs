using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Synith
{
    public class UnitCamera : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera unitCamera;
        [SerializeField] float zoomSpeed = 3f;
        [SerializeField] float minCameraDistance = 1f, maxCameraDistance = 15f;
        [SerializeField] bool invertZoom;

        CinemachineFramingTransposer cameraBody;

        Unit unit;

        private void Awake()
        {
            cameraBody = unitCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            unit = GetComponent<Unit>();
        }

        public void HandleAllCamera()
        {
            HandleZoom();
        }

        void HandleZoom()
        {
            float zoomValue = unit.UnitInput.Zoom;
            float zoomAmount = zoomValue * zoomSpeed * Time.deltaTime;

            zoomAmount = invertZoom ? -zoomAmount : zoomAmount;

            cameraBody.m_CameraDistance = Mathf.Clamp(cameraBody.m_CameraDistance + zoomAmount, minCameraDistance, maxCameraDistance);
        }
    }
}
