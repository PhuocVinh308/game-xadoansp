using System.Collections;
using UnityEngine;

namespace CaiSonHistoryGame.Gameplay
{
    /// <summary>
    /// Điều khiển điểm quan sát chiến thuật (Tactical Vantage Point) & Ống nhòm (Binoculars).
    /// </summary>
    public class TacticalCameraController : MonoBehaviour
    {
        [Header("Camera & FOV")]
        [SerializeField] private Camera tacticalCamera;
        [SerializeField] private float normalFov = 60f;
        [SerializeField] private float binocularsFov = 22f;
        [SerializeField] private float zoomSpeed = 8f;

        [Header("Điều khiển góc quay (Look Clamp)")]
        [SerializeField] private float mouseSensitivity = 2.0f;
        [SerializeField] private Vector2 pitchLimits = new Vector2(-20f, 25f);
        [SerializeField] private Vector2 yawLimits = new Vector2(-45f, 45f);

        [Header("Rung màn hình (Camera Shake)")]
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private float shakeMagnitude = 0.25f;

        private float targetFov;
        private float currentYaw = 0f;
        private float currentPitch = 0f;
        private Vector3 initialPosition;
        private bool isBinocularsActive = true;
        private bool canControl = true;

        private void Awake()
        {
            if (tacticalCamera == null) tacticalCamera = GetComponent<Camera>();
            if (tacticalCamera == null) tacticalCamera = Camera.main;
            initialPosition = transform.localPosition;
            targetFov = binocularsFov;
        }

        private void Update()
        {
            if (!canControl) return;

            // Xử lý Zoom FOV
            if (tacticalCamera != null)
            {
                tacticalCamera.fieldOfView = Mathf.Lerp(tacticalCamera.fieldOfView, targetFov, Time.deltaTime * zoomSpeed);
            }

            // Chuyển đổi bật/tắt ống nhòm bằng phím Space hoặc chuột phải
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.B))
            {
                ToggleBinoculars();
            }

            // Quay camera quan sát trận địa
            if (Input.GetMouseButton(0) || !Cursor.visible)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

                currentYaw += mouseX;
                currentPitch -= mouseY;

                currentYaw = Mathf.Clamp(currentYaw, yawLimits.x, yawLimits.y);
                currentPitch = Mathf.Clamp(currentPitch, pitchLimits.x, pitchLimits.y);

                transform.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            }
        }

        public void ToggleBinoculars()
        {
            isBinocularsActive = !isBinocularsActive;
            targetFov = isBinocularsActive ? binocularsFov : normalFov;
        }

        public void TriggerExplosionShake()
        {
            StartCoroutine(DoCameraShake());
        }

        private IEnumerator DoCameraShake()
        {
            float elapsed = 0.0f;
            while (elapsed < shakeDuration)
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
                float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

                transform.localPosition = new Vector3(initialPosition.x + x, initialPosition.y + y, initialPosition.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = initialPosition;
        }

        public void SetControlActive(bool active)
        {
            canControl = active;
        }
    }
}
