using System;
using UnityEngine;

namespace CaiSonHistoryGame.Data
{
    /// <summary>
    /// ScriptableObject đại diện cho một câu hỏi trắc nghiệm lịch sử trận đánh Cái Sơn.
    /// </summary>
    [CreateAssetMenu(fileName = "Question_", menuName = "CaiSonGame/Quiz Question", order = 2)]
    public class QuizQuestionSO : ScriptableObject
    {
        [Header("Nội dung câu hỏi")]
        [TextArea(2, 4)]
        public string questionPrompt;

        [Header("Các đáp án lựa chọn (A, B, C, D)")]
        public string[] options = new string[4];

        [Header("Chỉ số đáp án đúng (0 = A, 1 = B, 2 = C, 3 = D)")]
        [Range(0, 3)]
        public int correctOptionIndex;

        [Header("Giải thích lịch sử sau khi trả lời")]
        [TextArea(2, 5)]
        public string historicalExplanation;

        [Header("Điểm thưởng")]
        public int points = 100;
    }
}
