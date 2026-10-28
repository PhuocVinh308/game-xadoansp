# BỘ TÀI LIỆU TOÀN DIỆN: BLENDER MCP + 3D AI + UNITY 6
## GAME GIÁO DỤC LỊCH SỬ: CHIẾN THẮNG CÁI SƠN (16/06/1960 - VĨNH LONG)

---

## PHẦN 1: QUY TRÌNH KẾT HỢP (WORKFLOW OVERVIEW)

```
[1. Blender MCP / Python Automation]
    └── Dựng địa hình sông nước Nam Bộ, ruộng lúa, đường Cái Sơn, dừa nước, nhà lá.
    └── Tối ưu topology Low-poly, hierarchy chuẩn và xuất sang FBX/GLB.

[2. 3D AI Generation (Tripo3D / Meshy / CSM / Rodin)]
    ├── Nhân vật: Tỉnh trưởng Khưu Văn Ba, Du kích áo bà ba nón lá, Lính bảo an.
    └── Phương tiện chi tiết: Xe Jeep Willys M38, Xe tải GMC quân sự.
    └── Rigging Humanoid tự động (Mixamo / AccuRIG).

[3. Unity 6 Universal Render Pipeline (URP)]
    ├── Lắp ráp Scene 3D & ánh sáng chiều tà miền Tây (16h15).
    ├── Hệ thống điều khiển đoàn xe (Convoy AI) & góc nhìn quan sát ống nhòm.
    ├── Kịch bản phục kích & cơ chế tính điểm chuẩn xác (Kill Zone Timing).
    └── Giao diện học tập: Thẻ tư liệu lịch sử (Fact Cards) & Trắc nghiệm Q&A có thưởng huy hiệu.
```

---

## PHẦN 2: BỘ PROMPT CHO BLENDER MCP (AUTOMATION SCRIPT & PROMPT)

### 1. Prompt kích hoạt qua Blender MCP Tool:
```text
Act as a Technical Artist and Blender MCP Automation Expert.
Execute the automation pipeline to construct the 3D environment for "Trận chiến Cái Sơn (16/06/1960 - Vĩnh Long)":

1. Scene Specifications:
   - Location: Cai Son bridge & dirt road, Song Phu commune, Tam Binh district, Vinh Long province, South Vietnam.
   - Era: June 1960, tropical rural Mekong Delta setting.
   - Key Elements: Red dirt road (7m width), parallel alluvial mud-water canal (8m width), paddy fields, dense clusters of nipa palm trees (cây dừa nước), and rustic thatch-roof peasant cottages (nhà tranh vách đất).

2. Convoy Hierarchy:
   - Group 'Convoy_Vehicles': 
     * 1 Lead Willys military Jeep (Governor Khuu Van Ba)
     * 2 Escort military transport trucks with canvas covers.
   - Setup proper pivot points at ground contact for all wheels.

3. Ambush Guerrilla Placement:
   - Group 'Ambush_Guerrillas': 3 low-poly southern guerrilla scouts kneeling in the roadside foliage.

4. Export Settings:
   - Target Engine: Unity 6 URP.
   - Axis: -Z Forward, Y Up.
   - Clean UVs, joined hierarchy, PBR standard materials (Albedo, Roughness).
```

### 2. Lệnh thực thi nhanh bằng Python Headless:
```bash
blender --background --python blender_scripts/cai_son_environment_generator.py -- --export-fbx --export-gltf
```
*Tệp xuất sẽ nằm tại:* `Assets/Models/CaiSon_Ambush_Environment.glb` và `.fbx`.

---

## PHẦN 3: BỘ PROMPT CHO CÁC CÔNG CỤ 3D AI (TRIPO3D / MESHY / RODIN)

Dùng các prompt sau để tạo mô hình 3D Text-to-3D hoặc Image-to-3D với phong cách Stylized/Semi-realistic tối ưu cho game Unity:

### Prompt 1: Tỉnh trưởng Khưu Văn Ba (Governor Khuu Van Ba)
```text
Full-body 3D character of a 1960 South Vietnamese military governor (Khưu Văn Ba), early Republic of Vietnam Army officer uniform in khaki olive drab, garrison cap (mũ calô) with gold insignia, rank epaulets on shoulders, stern expression, mid-40s Vietnamese man, leather utility belt with pistol holster, standing in A-pose. Stylized realism, clean quad topology, PBR textures, optimized for Unity 6 humanoid avatar, game ready, mobile friendly.
--negative: modern tactical gear, helmets, cartoonish proportions, asymmetrical hands, distorted face
```

### Prompt 2: Chiến sĩ Du kích Cái Sơn (Southern Vietnamese Guerrilla Fighter)
```text
Full-body 3D character of a 1960 Vietnamese southern guerrilla fighter (du kích Nam Bộ), wearing traditional black peasant pajamas (áo bà ba đen), checkered scarf (khăn rằn) draped around neck, traditional conical straw hat (nón lá) tied on back or worn on head, rugged leather ammo pouches on waist, rolled-up trousers, barefoot or rubber sandals, holding a vintage scoped bolt-action rifle, athletic lean build, A-pose. Low-poly stylized, clean topology, PBR materials, Unity-ready humanoid.
--negative: modern combat armor, futuristic weapons, deformed hands, fantasy elements
```

### Prompt 3: Lính Bảo an hộ tống (Escort Security Soldier)
```text
Full-body 3D model of an early 1960s South Vietnamese provincial paramilitary guard (Lính bảo an), wearing olive green field fatigue shirt and pants, M1 steel helmet with chin strap, web belt with canteen and pouches, military combat boots, holding an M1 carbine rifle, neutral standing T-pose or A-pose. Game-ready, optimized polycount under 8000 triangles, clean UV maps.
--negative: modern Kevlar, plate carriers, digital camouflage, sci-fi guns
```

### Prompt 4: Xe Jeep Chỉ huy Willys M38 1950s (Command Jeep)
```text
1950s Willys M38 military utility Jeep, matte olive drab paint, open-top design, flat folding windshield with transparent glass, spare tire and jerrycan mounted on the rear, whip radio communication antenna on rear corner, white military stenciled serial numbers, weathered mud splatters on tires and lower bumper. Stylized low-poly asset, separated rotating wheels, clean pivot points, Unity 6 URP compatible PBR textures.
--negative: modern civilian jeep, metallic gloss, oversized rims, broken geometry
```

### Prompt 5: Xe tải Quân sự Chở lính (Military Escort Cargo Truck)
```text
1950s Dodge M37 or GMC military cargo transport truck, olive green metal cab, rear open cargo bed covered by arched tan canvas tarp, 6 rugged all-terrain wheels, front grille guard and headlights, mud-weathered chassis. Separated wheels for game physics, low-poly game asset, clean modular parts, Unity 6 compatible.
--negative: modern semi-truck, shiny chrome, luxury paint, deformed wheels
```

---

## PHẦN 4: HƯỚNG DẪN CẤU HÌNH TRONG UNITY 6 (URP)

### 1. Khởi tạo Game Data chỉ với 1 cú click:
Mở menu Unity:
```
Tools ➔ Cái Sơn Game ➔ Khởi tạo Dữ liệu Lịch sử & Trắc nghiệm
```
Toàn bộ Fact cards, bộ 4 câu hỏi trắc nghiệm lịch sử và 2 danh hiệu huy hiệu sẽ được tự động tạo tại thư mục `Assets/GameData/`.

### 2. Thiết lập Scene Hierarchy:
1. **Scene Root:**
   - Kéo `Assets/Models/CaiSon_Ambush_Environment.glb` vào Scene tại tọa độ `(0, 0, 0)`.
2. **Convoy Setup:**
   - Đặt GameObject `ConvoyManager` vào Scene.
   - Gắn component `ConvoyVehicle` vào xe Jeep dẫn đầu (đánh dấu `isLeadVehicle = true`).
   - Gắn `ConvoyVehicle` vào 2 xe tải phía sau và thêm vào mảng `escortVehicles`.
3. **Ambush Kill Zone:**
   - Tạo GameObject `AmbushZone` gắn script `AmbushZoneTrigger`.
   - Căn chỉnh vùng màu đỏ trong Scene View nằm tại vị trí cầu Cái Sơn (Z từ -5.0 đến 8.0).
4. **Tactical Camera:**
   - Gắn script `TacticalCameraController` vào Main Camera.
   - Đặt vị trí Camera ẩn nấp trong lùm dừa nước bên bờ kênh nhìn ra tim đường.
5. **Canvas UI:**
   - Gắn `HistoryDialogueUI`, `AmbushHUD`, `QuizUIController`, `VictoryBadgeUI`.
   - Kết nối tất cả vào `GameManager`.

### 3. Kịch bản trải nghiệm người chơi:
- **16:15 Phục kích:** Người chơi nhìn qua ống nhòm, đoàn xe Khưu Văn Ba tiến vào đường Cái Sơn.
- **Khai hỏa:** Bấm nút "NỔ SÚNG!" đúng thời điểm xe lọt vào Kill Zone.
- **Diễn biến:** Xe Jeep phát nổ bốc cháy dữ dội, xe tải hộ tống phanh gấp dạt vào bờ dừa hoảng loạn.
- **Giáo dục tư liệu:** Thẻ thông tin hiện lên nêu bật ý nghĩa: *"Tỉnh trưởng đầu tiên của chính quyền Sài Gòn bị đền tội trên chiến trường, mở đường cho phong trào Đồng Khởi"*.
- **Trắc nghiệm:** Người chơi trả lời 4 câu hỏi củng cố kiến thức, nhận *"Huy hiệu Dũng sĩ Diệt ác Phá kềm Xuất sắc"*.
