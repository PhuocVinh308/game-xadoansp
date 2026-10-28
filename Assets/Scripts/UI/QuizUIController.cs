using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CaiSonHistoryGame.Data;

namespace CaiSonHistoryGame.UI
{
    /// <summary>
    /// Điều khiển hệ thống trắc nghiệm tương tác kiểm tra kiến thức lịch sử trận Cái Sơn.
    /// </summary>
    public class QuizUIController : MonoBehaviour
    {
        [Header("Khung câu hỏi")]
        [SerializeField] private GameObject quizPanel;
        [SerializeField] private TextMeshProUGUI questionNumberText;
        [SerializeField] private TextMeshProUGUI questionPromptText;
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Các nút đáp án")]
        [SerializeField] private Button[] optionButtons = new Button[4];
        [SerializeField] private TextMeshProUGUI[] optionTexts = new TextMeshProUGUI[4];
        [SerializeField] private Image[] optionBackgrounds = new Image[4];

        [Header("Màu sắc trạng thái")]
        [SerializeField] private Color normalColor = new Color(0.2f, 0.2f, 0.25f, 1f);
        [SerializeField] private Color correctColor = new Color(0.1f, 0.65f, 0.25f, 1f);
        [SerializeField] private Color wrongColor = new Color(0.75f, 0.15f, 0.15f, 1f);

        [Header("Bảng giải thích & Nút chuyển tiếp")]
        [SerializeField] private GameObject explanationBox;
        [SerializeField] private TextMeshProUGUI explanationText;
        [SerializeField] private Button nextQuestionButton;

        [Header("Âm thanh phản hồi")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip correctSfx;
        [SerializeField] private AudioClip wrongSfx;

        private List<QuizQuestionSO> questionsList = new List<QuizQuestionSO>();
        private int currentQuestionIndex = 0;
        private int totalScore = 0;
        private bool hasAnsweredCurrent = false;
        private Action<int> onQuizFinishedCallback;

        private void Awake()
        {
            for (int i = 0; i < optionButtons.Length; i++)
            {
                int index = i;
                if (optionButtons[i] != null)
                {
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
                }
            }

            if (nextQuestionButton != null)
            {
                nextQuestionButton.onClick.AddListener(OnNextButtonClicked);
            }
        }

        public void StartQuiz(QuizDatabaseSO database, Action<int> onFinished)
        {
            if (database == null || database.questions == null || database.questions.Count == 0)
            {
                onFinished?.Invoke(0);
                return;
            }

            questionsList = new List<QuizQuestionSO>(database.questions);
            currentQuestionIndex = 0;
            totalScore = 0;
            onQuizFinishedCallback = onFinished;

            quizPanel.SetActive(true);
            UpdateScoreDisplay();
            ShowQuestion(currentQuestionIndex);
        }

        private void ShowQuestion(int index)
        {
            if (index < 0 || index >= questionsList.Count)
            {
                EndQuiz();
                return;
            }

            hasAnsweredCurrent = false;
            QuizQuestionSO q = questionsList[index];

            if (questionNumberText != null)
                questionNumberText.text = $"CÂU HỎI {index + 1} / {questionsList.Count}";

            if (questionPromptText != null)
                questionPromptText.text = q.questionPrompt;

            if (explanationBox != null) explanationBox.SetActive(false);
            if (nextQuestionButton != null) nextQuestionButton.gameObject.SetActive(false);

            char[] letters = new char[] { 'A', 'B', 'C', 'D' };
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < q.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].interactable = true;
                    if (optionTexts[i] != null) optionTexts[i].text = $"<b>{letters[i]}.</b> {q.options[i]}";
                    if (optionBackgrounds[i] != null) optionBackgrounds[i].color = normalColor;
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnOptionSelected(int selectedIndex)
        {
            if (hasAnsweredCurrent) return;
            hasAnsweredCurrent = true;

            QuizQuestionSO currentQ = questionsList[currentQuestionIndex];
            bool isCorrect = (selectedIndex == currentQ.correctOptionIndex);

            // Khóa tất cả các nút
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (optionButtons[i] != null) optionButtons[i].interactable = false;
            }

            // Đổi màu đáp án
            if (optionBackgrounds[selectedIndex] != null)
            {
                optionBackgrounds[selectedIndex].color = isCorrect ? correctColor : wrongColor;
            }

            // Luôn highlight đáp án đúng màu xanh
            if (optionBackgrounds[currentQ.correctOptionIndex] != null)
            {
                optionBackgrounds[currentQ.correctOptionIndex].color = correctColor;
            }

            if (isCorrect)
            {
                totalScore += currentQ.points;
                UpdateScoreDisplay();
                if (sfxSource != null && correctSfx != null) sfxSource.PlayOneShot(correctSfx);
            }
            else
            {
                if (sfxSource != null && wrongSfx != null) sfxSource.PlayOneShot(wrongSfx);
            }

            // Hiển thị phần giải thích lịch sử
            if (explanationBox != null && explanationText != null)
            {
                explanationBox.SetActive(true);
                string prefix = isCorrect ? "<color=#00FF66><b>CHÍNH XÁC!</b></color> " : "<color=#FF4444><b>CHƯA ĐÚNG!</b></color> ";
                explanationText.text = prefix + currentQ.historicalExplanation;
            }

            if (nextQuestionButton != null)
            {
                nextQuestionButton.gameObject.SetActive(true);
            }
        }

        private void OnNextButtonClicked()
        {
            currentQuestionIndex++;
            if (currentQuestionIndex < questionsList.Count)
            {
                ShowQuestion(currentQuestionIndex);
            }
            else
            {
                EndQuiz();
            }
        }

        private void UpdateScoreDisplay()
        {
            if (scoreText != null)
            {
                scoreText.text = $"ĐIỂM: <color=#FFD700>{totalScore}</color>";
            }
        }

        private void EndQuiz()
        {
            quizPanel.SetActive(false);
            onQuizFinishedCallback?.Invoke(totalScore);
        }
    }
}
