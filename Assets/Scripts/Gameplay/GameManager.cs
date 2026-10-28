using System.Collections;
using UnityEngine;
using CaiSonHistoryGame.Data;
using CaiSonHistoryGame.UI;

namespace CaiSonHistoryGame.Gameplay
{
    /// <summary>
    /// Game Manager điều phối toàn bộ vòng đời kịch bản trò chơi Lịch sử Chiến thắng Cái Sơn (1960).
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Trạng thái trò chơi")]
        [SerializeField] private GameState currentState = GameState.Prologue;

        [Header("Dữ liệu Lịch sử & Câu hỏi")]
        [SerializeField] private HistoricalFactSO prologueFactSO;
        [SerializeField] private HistoricalFactSO aftermathFactSO;
        [SerializeField] private QuizDatabaseSO quizDatabase;

        [Header("Hệ thống Gameplay")]
        [SerializeField] private ConvoyManager convoyManager;
        [SerializeField] private AmbushZoneTrigger ambushZoneTrigger;
        [SerializeField] private TacticalCameraController cameraController;

        [Header("Hệ thống Giao diện (UI)")]
        [SerializeField] private HistoryDialogueUI dialogueUI;
        [SerializeField] private AmbushHUD ambushHUD;
        [SerializeField] private QuizUIController quizUI;
        [SerializeField] private VictoryBadgeUI victoryUI;

        [Header("Âm nhạc nền")]
        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] private AudioClip prologueBgm;
        [SerializeField] private AudioClip suspenseBgm;
        [SerializeField] private AudioClip victoryBgm;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // Bắt đầu màn chơi từ Giai đoạn 1: Mở đầu (Prologue)
            TransitionToState(GameState.Prologue);
        }

        public void TransitionToState(GameState newState)
        {
            currentState = newState;
            Debug.Log($"[GameManager] Chuyển trạng thái: {newState}");

            switch (newState)
            {
                case GameState.Prologue:
                    HandlePrologueState();
                    break;
                case GameState.AmbushReady:
                    HandleAmbushReadyState();
                    break;
                case GameState.AmbushTriggered:
                    // Được kích hoạt từ nút bấm của AmbushHUD
                    break;
                case GameState.Aftermath:
                    HandleAftermathState();
                    break;
                case GameState.QuizMode:
                    HandleQuizModeState();
                    break;
                case GameState.VictorySummary:
                    // Xử lý sau khi kết thúc Quiz
                    break;
            }
        }

        // =====================================================================
        // GIAI ĐOẠN 1: MỞ ĐẦU (PROLOGUE)
        // =====================================================================
        private void HandlePrologueState()
        {
            PlayBGM(prologueBgm);

            if (cameraController != null) cameraController.SetControlActive(false);
            if (ambushHUD != null) ambushHUD.gameObject.SetActive(false);

            if (dialogueUI != null)
            {
                if (prologueFactSO != null)
                {
                    dialogueUI.DisplayHistoricalFact(prologueFactSO, () =>
                    {
                        TransitionToState(GameState.AmbushReady);
                    });
                }
                else
                {
                    dialogueUI.DisplayCustomText(
                        "BỐI CẢNH LỊCH SỬ - NĂM 1960",
                        "Xã Song Phú, huyện Tam Bình, tỉnh Vĩnh Long",
                        "Tháng 6/1960, cao trào Đồng Khởi đang bùng cháy khắp Nam Bộ. Tại Vĩnh Long, Tỉnh trưởng Khưu Văn Ba khét tiếng tàn bạo, chỉ huy đàn áp phong trào cách mạng.\n\nNhận được nguồn tin tình báo mật: Đúng 16h15 chiều ngày 16/6/1960, Khưu Văn Ba sẽ trực tiếp dẫn đoàn tuần tra qua khu vực cầu Cái Sơn.\n\nTổ du kích cùng bộ đội địa phương nhận lệnh khẩn: Thiết lập ổ phục kích bất ngờ, thực hiện nhiệm vụ 'Diệt ác phá kềm'!",
                        () =>
                        {
                            TransitionToState(GameState.AmbushReady);
                        },
                        "VÀO TRẬN ĐỊA PHỤC KÍCH ➔"
                    );
                }
            }
        }

        // =====================================================================
        // GIAI ĐOẠN 2: TRẬN ĐỊA PHỤC KÍCH (THE AMBUSH)
        // =====================================================================
        private void HandleAmbushReadyState()
        {
            PlayBGM(suspenseBgm);

            if (cameraController != null)
            {
                cameraController.SetControlActive(true);
            }

            if (ambushHUD != null)
            {
                ambushHUD.gameObject.SetActive(true);
                ambushHUD.InitializeHUD("16:15:00 - 16/06/1960", "Cầu Cái Sơn, Tam Bình, Vĩnh Long", "Mục tiêu: Xe Jeep chở Tỉnh trưởng Khưu Văn Ba");
                ambushHUD.SetFireButtonInteractable(true);
                ambushHUD.OnFireButtonPressed += OnPlayerFiredSignal;
            }

            if (convoyManager != null)
            {
                convoyManager.StartConvoyAdvance();
            }
        }

        private void OnPlayerFiredSignal()
        {
            if (currentState != GameState.AmbushReady) return;

            currentState = GameState.AmbushTriggered;
            if (ambushHUD != null)
            {
                ambushHUD.SetFireButtonInteractable(false);
                ambushHUD.OnFireButtonPressed -= OnPlayerFiredSignal;
            }

            // Đánh giá thời cơ phục kích
            Transform leadTransform = (convoyManager != null && convoyManager.leadVehicle != null) 
                ? convoyManager.leadVehicle.transform 
                : null;

            AmbushRating rating = (ambushZoneTrigger != null) 
                ? ambushZoneTrigger.ExecuteAmbush(leadTransform) 
                : AmbushRating.Perfect;

            // Rung màn hình và nổ xe Jeep
            if (cameraController != null) cameraController.TriggerExplosionShake();
            if (convoyManager != null) convoyManager.OnLeadVehicleAttacked();

            if (ambushHUD != null)
            {
                ambushHUD.ShowAmbushResult(rating);
            }

            // Chuyển sang Giai đoạn 3 sau 3 giây hiển thị hiệu ứng
            StartCoroutine(DelayTransitionAftermath(3.5f));
        }

        private IEnumerator DelayTransitionAftermath(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (ambushHUD != null) ambushHUD.gameObject.SetActive(false);
            TransitionToState(GameState.Aftermath);
        }

        // =====================================================================
        // GIAI ĐOẠN 3: XỬ LÝ TÌNH HUỐNG & Ý NGHĨA LỊCH SỬ (AFTERMATH)
        // =====================================================================
        private void HandleAftermathState()
        {
            PlayBGM(victoryBgm);

            if (dialogueUI != null)
            {
                if (aftermathFactSO != null)
                {
                    dialogueUI.DisplayHistoricalFact(aftermathFactSO, () =>
                    {
                        TransitionToState(GameState.QuizMode);
                    });
                }
                else
                {
                    dialogueUI.DisplayCustomText(
                        "Ý NGHĨA CHIẾN THẮNG CÁI SƠN (16/06/1960)",
                        "Trận đánh vang dội khắp miền Nam",
                        "Trận phục kích tại Cầu Cái Sơn ngày 16/6/1960 đã tiêu diệt hoàn toàn xe chỉ huy và tiêu diệt Tỉnh trưởng Khưu Văn Ba.\n\nĐây là tên TỈNH TRƯỞNG ĐẦU TIÊN của chính quyền tay sai Sài Gòn bị đền tội ngay tại chiến trường Nam Bộ.\n\nChiến thắng này đã giáng một đòn chí mạng làm rung chuyển bộ máy tề ngụy toàn tỉnh Vĩnh Long và Đồng bằng sông Cửu Long, cổ vũ mạnh mẽ phong trào Đồng Khởi giải phóng nông thôn.",
                        () =>
                        {
                            TransitionToState(GameState.QuizMode);
                        },
                        "LÀM BÀI TRẮC NGHIỆM LỊCH SỬ ➔"
                    );
                }
            }
        }

        // =====================================================================
        // GIAI ĐOẠN 4: BÀI TRẮC NGHIỆM LỊCH SỬ (QUIZ)
        // =====================================================================
        private void HandleQuizModeState()
        {
            if (quizUI != null && quizDatabase != null)
            {
                quizUI.StartQuiz(quizDatabase, OnQuizCompleted);
            }
            else
            {
                Debug.LogWarning("[GameManager] Không tìm thấy QuizUI hoặc QuizDatabase!");
                OnQuizCompleted(300);
            }
        }

        private void OnQuizCompleted(int finalScore)
        {
            TransitionToState(GameState.VictorySummary);

            HistoricalBadge badgeToAward = null;
            if (quizDatabase != null)
            {
                if (finalScore >= (quizDatabase.masterBadge?.minimumScoreRequired ?? 300))
                {
                    badgeToAward = quizDatabase.masterBadge;
                }
                else
                {
                    badgeToAward = quizDatabase.standardBadge;
                }
            }

            if (victoryUI != null)
            {
                victoryUI.DisplayVictory(badgeToAward, finalScore);
            }
        }

        private void PlayBGM(AudioClip clip)
        {
            if (bgmAudioSource == null || clip == null) return;
            bgmAudioSource.Stop();
            bgmAudioSource.clip = clip;
            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
        }
    }
}
