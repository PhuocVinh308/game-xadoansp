using System.Collections.Generic;
using UnityEngine;

namespace CaiSonHistoryGame.Data
{
    [System.Serializable]
    public class HistoricalBadge
    {
        public string badgeTitle;
        public string description;
        public Sprite icon;
        public int minimumScoreRequired;
    }

    /// <summary>
    /// Cơ sở dữ liệu bộ câu hỏi và phần thưởng huy hiệu lịch sử.
    /// </summary>
    [CreateAssetMenu(fileName = "QuizDatabase_CaiSon", menuName = "CaiSonGame/Quiz Database", order = 3)]
    public class QuizDatabaseSO : ScriptableObject
    {
        [Header("Danh sách câu hỏi trắc nghiệm")]
        public List<QuizQuestionSO> questions = new List<QuizQuestionSO>();

        [Header("Huy hiệu vinh danh")]
        public HistoricalBadge standardBadge;
        public HistoricalBadge masterBadge;
    }
}
