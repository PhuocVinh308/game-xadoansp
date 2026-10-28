using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CaiSonHistoryGame.Gameplay;

namespace CaiSonHistoryGame.UI
{
    /// <summary>
    /// Giao diện kính ngắm chiến thuật, đồng hồ thời gian lịch sử và nút lệnh khai hỏa.
    /// </summary>
    public class AmbushHUD : MonoBehaviour
    {
        [Header("Khung hiển thị thời gian & địa điểm")]
        [SerializeField] private TextMeshProUGUI clockText;
        [SerializeField] private TextMeshProUGUI locationText;
        [SerializeField] private TextMeshProUGUI targetIdentifierText;

        [Header("Ống nhòm & Tâm ngắm")]
        [SerializeField] private GameObject binocularsReticle;
        [SerializeField] private GameObject targetLockBox;

        [Header("Nút khai hỏa")]
        [SerializeField] private Button fireCommandButton;
        [SerializeField] private TextMeshProUGUI fireButtonText;

        [Header("Bảng thông báo kết quả đánh giá")]
        [SerializeField] private GameObject resultBannerPanel;
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private TextMeshProUGUI resultDetailText;

        public event Action OnFireButtonPressed;

        private void Awake()
        {
            if (fireCommandButton != null)
            {
                fireCommandButton.onClick.AddListener(() => OnFireButtonPressed?.Invoke());
            }

            if (resultBannerPanel != null)
            {
                resultBannerPanel.SetActive(false);
            }
        }

        public void InitializeHUD(string timeStr, string locationStr, string targetStr)
        {
            if (clockText != null) clockText.text = timeStr;
            if (locationText != null) locationText.text = locationStr;
            if (targetIdentifierText != null) targetIdentifierText.text = targetStr;
        }

        public void SetBinocularsActive(bool active)
        {
            if (binocularsReticle != null) binocularsReticle.SetActive(active);
        }

        public void SetFireButtonInteractable(bool interactable)
        {
            if (fireCommandButton != null) fireCommandButton.interactable = interactable;
        }

        public void ShowAmbushResult(AmbushRating rating)
        {
            if (resultBannerPanel == null) return;
            resultBannerPanel.SetActive(true);

            switch (rating)
            {
                case AmbushRating.Perfect:
                    resultTitleText.text = "<color=#00FF66>★ BÁCH PHÁT BÁCH TRÚNG! ★</color>";
                    resultDetailText.text = "Phát súng lệnh đanh gọn! Xe Jeep của Tỉnh trưởng Khưu Văn Ba bị tiêu diệt tại chỗ. Đoàn xe hộ tống rối loạn!";
                    break;
                case AmbushRating.TooEarly:
                    resultTitleText.text = "<color=#FFCC00>KHAI HỎA HƠI SỚM</color>";
                    resultDetailText.text = "Đoàn xe địch chưa lọt sâu vào cung đường tử thần, nhưng xe đầu vẫn bị trúng đạn và mất lái!";
                    break;
                case AmbushRating.TooLate:
                    resultTitleText.text = "<color=#FF9900>KHAI HỎA HƠI TRỄ</color>";
                    resultDetailText.text = "Đoàn xe gần vượt qua vị trí ém quân chính, du kích kịp bắn đuổi chặn đầu xe Jeep!";
                    break;
                case AmbushRating.Miss:
                    resultTitleText.text = "<color=#FF3333>BỎ LỠ THỜI CƠ</color>";
                    resultDetailText.text = "Thời cơ phục kích trôi qua, hỏa lực không kịp tập trung vào mục tiêu trọng yếu!";
                    break;
            }
        }

        public void HideHUD()
        {
            gameObject.SetActive(false);
        }
    }
}
