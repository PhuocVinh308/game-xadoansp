#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using CaiSonHistoryGame.Data;

namespace CaiSonHistoryGame.Editor
{
    public static class CaiSonDataGenerator
    {
        [MenuItem("Tools/Cái Sơn Game/Khởi tạo Dữ liệu Lịch sử & Trắc nghiệm")]
        public static void GenerateDefaultAssets()
        {
            string folderPath = "Assets/GameData";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "GameData");
            }

            // 1. Tạo Fact Mở đầu (Prologue)
            HistoricalFactSO prologueFact = ScriptableObject.CreateInstance<HistoricalFactSO>();
            prologueFact.factId = "FACT_01_PROLOGUE";
            prologueFact.title = "BỐI CẢNH LỊCH SỬ NĂM 1960: ĐỒNG KHỞI BÙNG PHÁT";
            prologueFact.timestampText = "Tháng 6 năm 1960";
            prologueFact.locationText = "Xã Song Phú, huyện Tam Bình, tỉnh Vĩnh Long";
            prologueFact.description = "Năm 1960, sau phong trào Đồng Khởi ở Bến Tre, ngọn lửa cách mạng bùng cháy khắp Nam Bộ. Tại Vĩnh Long, chính quyền Ngô Đình Diệm cử tên Đại tá Khưu Văn Ba làm Tỉnh trưởng, ráo riết càn quét, gom dân lập khu trù mật và khủng bố tàn bạo đồng bào ta.\n\nTỉnh ủy Vĩnh Long chủ trương kiên quyết 'Diệt ác phá kềm', hạ quyết tâm tiêu diệt tên đầu sỏ để mở toang vùng kềm kẹp của địch.";
            prologueFact.significanceSummary = "Phá vỡ thế kìm kẹp của bộ máy ngụy quyền, giải phóng các vùng nông thôn Tam Bình.";
            AssetDatabase.CreateAsset(prologueFact, $"{folderPath}/Fact_Prologue.asset");

            // 2. Tạo Fact Chiến công (Aftermath)
            HistoricalFactSO aftermathFact = ScriptableObject.CreateInstance<HistoricalFactSO>();
            aftermathFact.factId = "FACT_02_AFTERMATH";
            aftermathFact.title = "CHIẾN THẮNG CÁI SƠN: TIÊU DIỆT TỈNH TRƯỞNG KHƯU VĂN BA";
            aftermathFact.timestampText = "16h15 ngày 16/6/1960";
            aftermathFact.locationText = "Cầu Cái Sơn, Quốc lộ 4 (nay là QL1A), xã Song Phú";
            aftermathFact.description = "Đúng 16h15 ngày 16/6/1960, đoàn xe tuần tra của Khưu Văn Ba gồm 1 xe Jeep dẫn đường và 2 xe tải chở lính bảo an lọt vào tầm ngắm của ta tại cầu Cái Sơn. Lực lượng phục kích đồng loạt nổ súng chính xác, bắn cháy xe Jeep chỉ huy, tiêu diệt tại chỗ tên Tỉnh trưởng Khưu Văn Ba cùng nhiều sĩ quan hộ tống.";
            aftermathFact.significanceSummary = "Khưu Văn Ba là Tỉnh trưởng đầu tiên của chính quyền Sài Gòn bị đền tội tại chiến trường. Chiến thắng làm rúng động ngụy quyền toàn miền Nam, tạo bước ngoặt mở rộng phong trào Đồng Khởi tại Vĩnh Long.";
            AssetDatabase.CreateAsset(aftermathFact, $"{folderPath}/Fact_Aftermath.asset");

            // 3. Tạo các câu hỏi trắc nghiệm
            // Câu 1
            QuizQuestionSO q1 = ScriptableObject.CreateInstance<QuizQuestionSO>();
            q1.questionPrompt = "Trận phục kích tại Cầu Cái Sơn diễn ra vào ngày tháng năm nào?";
            q1.options = new string[] {
                "16/06/1960",
                "19/08/1945",
                "01/01/1960",
                "30/04/1975"
            };
            q1.correctOptionIndex = 0;
            q1.historicalExplanation = "Vào đúng 16h15 ngày 16 tháng 6 năm 1960, lực lượng vũ trang cách mạng đã bí mật phục kích thành công tại cầu Cái Sơn (xã Song Phú, Tam Bình, Vĩnh Long).";
            q1.points = 100;
            AssetDatabase.CreateAsset(q1, $"{folderPath}/Question_01.asset");

            // Câu 2
            QuizQuestionSO q2 = ScriptableObject.CreateInstance<QuizQuestionSO>();
            q2.questionPrompt = "Mục tiêu chỉ huy cấp cao nào của đối phương đã bị tiêu diệt trong trận đánh Cái Sơn?";
            q2.options = new string[] {
                "Tướng Dương Văn Minh",
                "Tỉnh trưởng Khưu Văn Ba",
                "Đại sứ Nolting",
                "Cố vấn Ngô Đình Nhu"
            };
            q2.correctOptionIndex = 1;
            q2.historicalExplanation = "Tỉnh trưởng Vĩnh Long Khưu Văn Ba – tên ác ôn đầu sỏ khét tiếng – đã bị tiêu diệt ngay trên chiếc xe Jeep dẫn đầu.";
            q2.points = 100;
            AssetDatabase.CreateAsset(q2, $"{folderPath}/Question_02.asset");

            // Câu 3
            QuizQuestionSO q3 = ScriptableObject.CreateInstance<QuizQuestionSO>();
            q3.questionPrompt = "Sự kiện tiêu diệt Khưu Văn Ba có ý nghĩa đặc biệt gì trong lịch sử kháng chiến chống Mỹ?";
            q3.options = new string[] {
                "Là trận thắng kết thúc hoàn toàn chiến dịch Hồ Chí Minh",
                "Là lần đầu tiên một Tỉnh trưởng của chính quyền Sài Gòn bị tiêu diệt tại trận",
                "Buộc Mỹ phải rút toàn bộ cố vấn quân sự về nước",
                "Giải phóng toàn bộ các đô thị lớn tại miền Nam"
            };
            q3.correctOptionIndex = 1;
            q3.historicalExplanation = "Khưu Văn Ba là tên tỉnh trưởng đầu tiên của chính quyền tay sai Sài Gòn bị đền tội ngay tại chiến trường ở miền Nam Việt Nam.";
            q3.points = 100;
            AssetDatabase.CreateAsset(q3, $"{folderPath}/Question_03.asset");

            // Câu 4
            QuizQuestionSO q4 = ScriptableObject.CreateInstance<QuizQuestionSO>();
            q4.questionPrompt = "Trận chiến Cái Sơn gắn liền với phong trào cách mạng nào đang sục sôi ở Nam Bộ thời điểm đó?";
            q4.options = new string[] {
                "Phong trào Cần Vương",
                "Phong trào Đông Du",
                "Phong trào Đồng Khởi & 'Diệt ác phá kềm'",
                "Chiến dịch Điện Biên Phủ"
            };
            q4.correctOptionIndex = 2;
            q4.historicalExplanation = "Chiến thắng Cái Sơn là biểu tượng tiêu biểu cho chủ trương 'Diệt ác phá kềm', hỗ trợ đắc lực cho phong trào Đồng Khởi giải phóng nông thôn năm 1960.";
            q4.points = 100;
            AssetDatabase.CreateAsset(q4, $"{folderPath}/Question_04.asset");

            // 4. Tạo Database tổng hợp
            QuizDatabaseSO db = ScriptableObject.CreateInstance<QuizDatabaseSO>();
            db.questions.Add(q1);
            db.questions.Add(q2);
            db.questions.Add(q3);
            db.questions.Add(q4);

            db.standardBadge = new HistoricalBadge
            {
                badgeTitle = "HUY HIỆU DU KÍCH CÁI SƠN KIÊN CƯỜNG",
                description = "Trao tặng cho người chơi đã nắm vững kiến thức căn bản về mốc son ngày 16/06/1960.",
                minimumScoreRequired = 200
            };

            db.masterBadge = new HistoricalBadge
            {
                badgeTitle = "HUY HIỆU DŨNG SĨ DIỆT ÁC PHÁ KỀM XUẤT SẮC",
                description = "Vinh danh người chơi đạt điểm tuyệt đối 400/400 trong bài kiểm tra lịch sử Chiến thắng Cái Sơn!",
                minimumScoreRequired = 400
            };

            AssetDatabase.CreateAsset(db, $"{folderPath}/QuizDatabase_CaiSon.asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Thành công", "Đã tạo đầy đủ bộ dữ liệu ScriptableObject cho Lịch sử Chiến thắng Cái Sơn tại Assets/GameData!", "OK");
        }
    }
}
#endif
