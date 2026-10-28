using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CaiSonHistoryGame.Data;

namespace CaiSonHistoryGame.UI
{
    /// <summary>
    /// Hiển thị màn hình vinh danh, trao huy hiệu lịch sử sau khi hoàn thành chiến dịch và bài trắc nghiệm.
    /// </summary>
    public class VictoryBadgeUI : MonoBehaviour
    {
        [Header("Khung giao diện")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI badgeNameText;
        [SerializeField] private TextMeshProUGUI badgeDescriptionText;
        [SerializeField] private Image badgeIcon;
        [SerializeField] private TextMeshProUGUI totalScoreSummaryText;

        [Header("Nút điều hướng")]
        [SerializeField] private Button replayButton;

        private void Awake()
        {
            if (replayButton != null)
            {
                replayButton.onClick.AddListener(OnReplayClicked);
            }
        }

        public void DisplayVictory(HistoricalBadge badge, int finalScore)
        {
            victoryPanel.SetActive(true);

            if (titleText != null)
                titleText.text = "CHIẾN CÔNG CÁI SƠN HOÀN TẤT!";

            if (badge != null)
            {
                if (badgeNameText != null) badgeNameText.text = badge.badgeTitle;
                if (badgeDescriptionText != null) badgeDescriptionText.text = badge.description;
                if (badgeIcon != null && badge.icon != null) badgeIcon.sprite = badge.icon;
            }

            if (totalScoreSummaryText != null)
            {
                totalScoreSummaryText.text = $"Tổng điểm kiến thức lịch sử: <color=#FFD700><b>{finalScore}</b></color>";
            }
        }

        private void OnReplayClicked()
        {
            // Tải lại màn chơi hiện tại
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
