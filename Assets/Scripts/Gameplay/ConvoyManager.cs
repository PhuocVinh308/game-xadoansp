using System.Collections.Generic;
using UnityEngine;

namespace CaiSonHistoryGame.Gameplay
{
    /// <summary>
    /// Quản lý toàn bộ đoàn xe quân sự tuần tra của Tỉnh trưởng Khưu Văn Ba.
    /// </summary>
    public class ConvoyManager : MonoBehaviour
    {
        public static ConvoyManager Instance { get; private set; }

        [Header("Xe trong đoàn")]
        public ConvoyVehicle leadVehicle; // Xe Jeep Khưu Văn Ba
        public List<ConvoyVehicle> escortVehicles = new List<ConvoyVehicle>();

        [Header("Điểm tập kết xuất phát")]
        public Transform convoySpawnPoint;
        public float vehicleSpacing = 14.0f; // Khoảng cách giãn đội hình 14m

        [Header("Trạng thái di chuyển")]
        [SerializeField] private bool isConvoyMoving = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartConvoyAdvance()
        {
            isConvoyMoving = true;
            if (leadVehicle != null)
            {
                leadVehicle.enabled = true;
            }

            foreach (var truck in escortVehicles)
            {
                if (truck != null) truck.enabled = true;
            }
        }

        /// <summary>
        /// Kêu gọi khi phát lệnh khai hỏa trúng xe đầu
        /// </summary>
        public void OnLeadVehicleAttacked()
        {
            if (leadVehicle != null)
            {
                leadVehicle.ApplyDirectHit();
            }

            // Toàn bộ các xe hộ tống phía sau rơi vào hoảng loạn, phanh gấp và mất phương hướng
            foreach (var truck in escortVehicles)
            {
                if (truck != null)
                {
                    truck.TriggerChaos();
                }
            }
        }

        public void StopAllVehicles()
        {
            isConvoyMoving = false;
            if (leadVehicle != null) leadVehicle.currentSpeed = 0f;
            foreach (var truck in escortVehicles)
            {
                if (truck != null) truck.currentSpeed = 0f;
            }
        }
    }
}
