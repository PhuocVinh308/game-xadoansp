using System;
using UnityEngine;

namespace CaiSonHistoryGame.Data
{
    /// <summary>
    /// ScriptableObject lưu trữ các thông tin tư liệu lịch sử được hiển thị dưới dạng thẻ thông tin (Fact Card)
    /// </summary>
    [CreateAssetMenu(fileName = "Fact_", menuName = "CaiSonGame/Historical Fact", order = 1)]
    public class HistoricalFactSO : ScriptableObject
    {
        [Header("Thông tin cơ bản")]
        public string factId;
        public string title;
        [TextArea(3, 8)]
        public string description;
        
        [Header("Tư liệu & Thời điểm")]
        public string timestampText = "16h15 ngày 16/6/1960";
        public string locationText = "Cầu Cái Sơn, xã Song Phú, huyện Tam Bình, tỉnh Vĩnh Long";
        public Sprite historicalPhoto; // Ảnh tư liệu (nếu có)
        public AudioClip voiceNarration; // Lời bình / thuyết minh

        [Header("Ý nghĩa lịch sử cốt lõi")]
        [TextArea(2, 4)]
        public string significanceSummary;
    }
}
