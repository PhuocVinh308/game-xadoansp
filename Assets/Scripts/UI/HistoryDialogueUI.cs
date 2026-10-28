using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CaiSonHistoryGame.Data;

namespace CaiSonHistoryGame.UI
{
    /// <summary>
    /// Điều khiển bảng hội thoại lịch sử, giới thiệu bối cảnh (Prologue) và tư liệu giáo dục (Educational Fact Cards).
    /// </summary>
    public class HistoryDialogueUI : MonoBehaviour
    {
        [Header("Khung giao diện")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI metaInfoText;
        [SerializeField] private TextMeshProUGUI contentBodyText;
        [SerializeField] private Image historicalPhotoDisplay;
        [SerializeField] private Button nextOrContinueButton;
        [SerializeField] private TextMeshProUGUI continueButtonText;

        [Header("Hiệu ứng chữ chạy (Typewriter)")]
        [SerializeField] private float typingSpeed = 0.025f;
        [SerializeField] private AudioSource typewriterAudio;

        private Coroutine typingCoroutine;
        private Action onCompleteCallback;
        private bool isTyping = false;
        private string fullTextCache = "";

        private void Awake()
        {
            if (nextOrContinueButton != null)
            {
                nextOrContinueButton.onClick.AddListener(OnContinueClicked);
            }
        }

        public void DisplayHistoricalFact(HistoricalFactSO fact, Action onFinished)
        {
            if (fact == null)
            {
                onFinished?.Invoke();
                return;
            }

            onCompleteCallback = onFinished;
            dialoguePanel.SetActive(true);

            if (titleText != null) titleText.text = fact.title;
            if (metaInfoText != null) metaInfoText.text = $"<b>Thời gian:</b> {fact.timestampText}\n<b>Địa điểm:</b> {fact.locationText}";
            if (historicalPhotoDisplay != null)
            {
                if (fact.historicalPhoto != null)
                {
                    historicalPhotoDisplay.gameObject.SetActive(true);
                    historicalPhotoDisplay.sprite = fact.historicalPhoto;
                }
                else
                {
                    historicalPhotoDisplay.gameObject.SetActive(false);
                }
            }

            string combinedBody = $"{fact.description}\n\n<color=#FFD700><b>Ý nghĩa lịch sử:</b> {fact.significanceSummary}</color>";
            fullTextCache = combinedBody;

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeTextRoutine(combinedBody));

            if (continueButtonText != null) continueButtonText.text = "TIẾP TỤC ➔";
        }

        public void DisplayCustomText(string title, string meta, string content, Action onFinished, string buttonLabel = "TIẾP TỤC ➔")
        {
            onCompleteCallback = onFinished;
            dialoguePanel.SetActive(true);

            if (titleText != null) titleText.text = title;
            if (metaInfoText != null) metaInfoText.text = meta;
            if (historicalPhotoDisplay != null) historicalPhotoDisplay.gameObject.SetActive(false);

            fullTextCache = content;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeTextRoutine(content));

            if (continueButtonText != null) continueButtonText.text = buttonLabel;
        }

        private IEnumerator TypeTextRoutine(string textToType)
        {
            isTyping = true;
            contentBodyText.text = "";

            bool insideTag = false;
            for (int i = 0; i < textToType.Length; i++)
            {
                char c = textToType[i];
                if (c == '<') insideTag = true;
                if (c == '>') { insideTag = false; contentBodyText.text += c; continue; }

                contentBodyText.text += c;

                if (!insideTag)
                {
                    if (typewriterAudio != null && UnityEngine.Random.value < 0.25f)
                    {
                        typewriterAudio.Play();
                    }
                    yield return new WaitForSeconds(typingSpeed);
                }
            }

            isTyping = false;
        }

        private void OnContinueClicked()
        {
            if (isTyping)
            {
                // Nhấn khi đang gõ chữ -> Hoàn tất hiển thị ngay lập tức
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                contentBodyText.text = fullTextCache;
                isTyping = false;
                return;
            }

            dialoguePanel.SetActive(false);
            onCompleteCallback?.Invoke();
        }

        public void Close()
        {
            dialoguePanel.SetActive(false);
        }
    }
}
