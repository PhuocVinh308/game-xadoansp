# GAME LỊCH SỬ: CHIẾN THẮNG CÁI SƠN (16/06/1960 - VĨNH LONG)
### Tích hợp Blender MCP + 3D AI Generative Pipeline + Unity 6 (URP)

Dự án game giáo dục lịch sử tái hiện trận phục kích tiêu diệt Tỉnh trưởng Khưu Văn Ba tại cầu Cái Sơn (xã Song Phú, Tam Bình, Vĩnh Long) vào lúc 16h15 ngày 16/6/1960.

---

## CẤU TRÚC THƯ MỤC DỰ ÁN

```
game-xadoansp/
├── blender_scripts/
│   └── cai_son_environment_generator.py    # Script Blender MCP tự động hóa tạo cảnh quan & convoy
├── Assets/
│   ├── Models/
│   │   ├── CaiSon_Ambush_Environment.glb  # Model 3D đã sinh sẵn từ Blender (URP ready)
│   │   └── CaiSon_Ambush_Environment.fbx  # File FBX chuẩn trục Unity (-Z Forward, Y Up)
│   ├── Scripts/
│   │   ├── Data/
│   │   │   ├── HistoricalFactSO.cs        # ScriptableObject chứa thẻ tư liệu lịch sử
│   │   │   ├── QuizQuestionSO.cs          # ScriptableObject câu hỏi trắc nghiệm
│   │   │   └── QuizDatabaseSO.cs          # Database câu hỏi & Huy hiệu vinh danh
│   │   ├── Gameplay/
│   │   │   ├── GameEnums.cs               # Trạng thái game & đánh giá thời điểm phục kích
│   │   │   ├── GameManager.cs             # State Machine điều phối 4 màn chơi lịch sử
│   │   │   ├── ConvoyVehicle.cs           # Mô phỏng xe rung lắc, trúng đạn nổ, dạt lề
│   │   │   ├── ConvoyManager.cs           # Điều khiển đội hình đoàn xe và sự hoảng loạn
│   │   │   ├── TacticalCameraController.cs# Góc nhìn du kích, ống nhòm zoom, rung màn hình
│   │   │   └── AmbushZoneTrigger.cs       # Đánh giá độ chuẩn xác cú nổ súng (Kill Zone)
│   │   ├── UI/
│   │   │   ├── AmbushHUD.cs               # Kính ngắm chiến thuật, đồng hồ 16:15, nút Nổ Súng
│   │   │   ├── HistoryDialogueUI.cs       # Cắt cảnh bối cảnh lịch sử, hiệu ứng gõ chữ
│   │   │   ├── QuizUIController.cs        # Mini-game trắc nghiệm 4 đáp án & âm thanh
│   │   │   └── VictoryBadgeUI.cs          # Màn hình vinh danh & trao huy hiệu
│   │   └── Editor/
│   │       └── CaiSonDataGenerator.cs     # Menu 1-click tạo trọn bộ dữ liệu câu hỏi trong Unity
├── PROMPT_3D_AI_AND_BLENDER_MCP.md        # Toàn bộ Prompt chi tiết cho Blender MCP & 3D AI
└── README.md
```

---

## 4 GIAI ĐOẠN MÀN CHƠI TRONG GAME (GAME DESIGN FLOW)

| Màn chơi (Scene) | Nội dung lịch sử tích hợp | Cơ chế tương tác của người chơi |
| :--- | :--- | :--- |
| **1. Mở đầu (Prologue)** | Giới thiệu bối cảnh năm 1960 tại Song Phú, Tam Bình, Vĩnh Long: cao trào Đồng Khởi và nhiệm vụ "Diệt ác phá kềm". | Đọc tư liệu lịch sử với hiệu ứng máy đánh chữ và lời thuyết minh. |
| **2. Trận địa phục kích (The Ambush)** | Đúng 16h15 ngày 16/6/1960, đoàn xe của Tỉnh trưởng Khưu Văn Ba lọt vào ổ phục kích. | Quan sát qua ống nhòm, căn tọa độ Kill Zone và nhấn nút **"NỔ SÚNG!"**. |
| **3. Xử lý tình huống & Chiến công** | Tiêu diệt xe Jeep đi đầu, Khưu Văn Ba đền tội, đoàn xe hộ tống hoảng loạn dạt vào mép dừa nước. | Xem diễn biến nổ xe, nhận thông tin giải thích ý nghĩa lịch sử to lớn của trận đánh. |
| **4. Kiểm tra kiến thức (Quiz)** | Củng cố mốc thời gian, ý nghĩa phong trào Đồng Khởi / Diệt ác phá kềm. | Trả lời 4 câu hỏi trắc nghiệm tương tác để nhận **Huy hiệu Dũng sĩ Diệt ác Phá kềm**. |

---

## HƯỚNG DẪN CHẠY & TÁI TẠO MÔ HÌNH 3D

Nếu muốn chạy lại script sinh cảnh 3D từ Blender:
```bash
blender --background --python blender_scripts/cai_son_environment_generator.py -- --export-fbx --export-gltf
```
Tài nguyên xuất sẽ tự động ghi đè và cập nhật vào `Assets/Models/`.
