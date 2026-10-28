using UnityEngine;
using System;

namespace CaiSonHistoryGame.Gameplay
{
    /// <summary>
    /// Vùng phục kích (Ambush Kill Zone) tại cầu Cái Sơn.
    /// Tính toán và đánh giá độ chuẩn xác về thời điểm ra lệnh "Nổ Súng".
    /// </summary>
    public class AmbushZoneTrigger : MonoBehaviour
    {
        [Header("Khoảng cách tọa độ Z của vùng phục kích")]
        public float sweetSpotZMin = -5.0f;
        public float sweetSpotZMax = 8.0f;
        public float earlyWarningZ = -18.0f;
        public float lateWarningZ = 16.0f;

        [Header("Âm thanh súng nổ")]
        [SerializeField] private AudioSource gunshotAudio;
        [SerializeField] private AudioClip ambushSignalClip;

        public event Action<AmbushRating> OnAmbushExecuted;

        /// <summary>
        /// Người chơi nhấn nút ra lệnh "Nổ súng!"
        /// </summary>
        public AmbushRating ExecuteAmbush(Transform leadVehicleTransform)
        {
            if (leadVehicleTransform == null)
            {
                Debug.LogWarning("[AmbushZoneTrigger] Lead vehicle not found!");
                return AmbushRating.Miss;
            }

            float currentZ = leadVehicleTransform.position.z;
            AmbushRating rating;

            if (currentZ < sweetSpotZMin)
            {
                rating = AmbushRating.TooEarly;
            }
            else if (currentZ >= sweetSpotZMin && currentZ <= sweetSpotZMax)
            {
                rating = AmbushRating.Perfect;
            }
            else if (currentZ > sweetSpotZMax && currentZ <= lateWarningZ)
            {
                rating = AmbushRating.TooLate;
            }
            else
            {
                rating = AmbushRating.Miss;
            }

            // Phát âm thanh súng lệnh / mìn định hướng gài sẵn
            if (gunshotAudio != null && ambushSignalClip != null)
            {
                gunshotAudio.PlayOneShot(ambushSignalClip);
            }

            OnAmbushExecuted?.Invoke(rating);
            return rating;
        }

        private void OnDrawGizmos()
        {
            // Vẽ hộp trực quan hóa Kill Zone trong Unity Scene Editor
            Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
            Vector3 center = new Vector3(0, 0.5f, (sweetSpotZMin + sweetSpotZMax) / 2f);
            Vector3 size = new Vector3(8f, 2f, Mathf.Abs(sweetSpotZMax - sweetSpotZMin));
            Gizmos.DrawCube(center, size);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
