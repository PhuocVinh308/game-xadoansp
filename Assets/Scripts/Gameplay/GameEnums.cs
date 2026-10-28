namespace CaiSonHistoryGame.Gameplay
{
    public enum GameState
    {
        Prologue,        // Cắt cảnh/thuyết minh bối cảnh lịch sử năm 1960
        AmbushReady,     // Chờ đoàn xe địch tiến vào (16:15), người chơi quan sát qua ống nhòm
        AmbushTriggered, // Người chơi bấm nút "Nổ súng" khai hỏa
        Aftermath,       // Diễn biến xe Jeep nổ, đoàn hộ tống tan tác, bảng ý nghĩa lịch sử hiện ra
        QuizMode,        // Bộ câu hỏi trắc nghiệm kiểm tra kiến thức
        VictorySummary   // Kết thúc màn chơi, tổng kết điểm và trao huy hiệu lịch sử
    }

    public enum AmbushRating
    {
        TooEarly,   // Khai hỏa quá sớm (địch chưa vào hẳn cung đường tử thần)
        Perfect,    // Khai hỏa chuẩn xác tuyệt đối (Bách phát bách trúng)
        TooLate,    // Khai hỏa trễ (đoàn xe sắp thoát tầm ngắm)
        Miss        // Bỏ lỡ thời cơ
    }
}
