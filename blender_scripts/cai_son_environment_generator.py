"""
=============================================================================
BLENDER AUTOMATION SCRIPT: TRẬN CHIẾN CÁI SƠN (16/06/1960 - VĨNH LONG)
=============================================================================
Sử dụng với Blender MCP hoặc chạy trực tiếp bằng dòng lệnh:
    blender --background --python cai_son_environment_generator.py -- --export-fbx --export-gltf

Script này tự động tạo dựng môi trường 3D Low-Poly Nam Bộ năm 1960:
1. Địa hình: Con đường đất Cái Sơn, kênh rạch nước lợ, bờ ruộng lúa Tam Bình.
2. Thực vật & Kiến trúc: Cây dừa nước, bụi rậm phục kích, nhà tranh mái lá Nam Bộ.
3. Đoàn xe tuần tra:
   - 01 Xe Jeep quân sự dẫn đầu (chở Tỉnh trưởng Khưu Văn Ba)
   - 02 Xe vận tải hộ tống
4. Vị trí ẩn nấp của Tổ du kích Cái Sơn (áo bà ba, nón lá, súng trường).
5. Tối ưu phân cấp Hierarchy và xuất file FBX/glTF chuẩn Unity 6 (URP).
=============================================================================
"""

import bpy
import bmesh
import math
import os
import sys

def clear_scene():
    """Xóa sạch scene mặc định của Blender."""
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.object.delete(use_global=False)
    for col in bpy.data.collections:
        bpy.data.collections.remove(col)
    for m in bpy.data.materials:
        bpy.data.materials.remove(m)
    for mesh in bpy.data.meshes:
        bpy.data.meshes.remove(mesh)

def create_material(name, diffuse_color, roughness=0.8):
    """Tạo URP-friendly PBR Material với màu sắc và độ nhám."""
    mat = bpy.data.materials.get(name)
    if mat is None:
        mat = bpy.data.materials.new(name=name)
        mat.use_nodes = True
        nodes = mat.node_tree.nodes
        bsdf = nodes.get("Principled BSDF")
        if bsdf:
            # Blender 4.x / 3.x compatible Base Color
            bsdf.inputs["Base Color"].default_value = diffuse_color
            bsdf.inputs["Roughness"].default_value = roughness
    return mat

def get_or_create_collection(col_name):
    """Quản lý các Collection phân nhóm rõ ràng."""
    if col_name in bpy.data.collections:
        return bpy.data.collections[col_name]
    new_col = bpy.data.collections.new(col_name)
    bpy.context.scene.collection.children.link(new_col)
    return new_col

# =============================================================================
# 1. TẠO MÔI TRƯỜNG & ĐỊA HÌNH NAM BỘ (TERRAIN & WATER)
# =============================================================================

def build_environment():
    col = get_or_create_collection("Environment_CaiSon")
    
    # 1.1 Đất đồng bằng & Ruộng lúa
    bpy.ops.mesh.primitive_plane_add(size=120, location=(0, 0, -0.2))
    ground = bpy.context.active_object
    ground.name = "Ground_PaddyField"
    col.objects.link(ground)
    bpy.context.scene.collection.objects.unlink(ground)
    
    paddy_mat = create_material("Mat_PaddyGrass", (0.28, 0.45, 0.15, 1.0), roughness=0.9)
    ground.data.materials.append(paddy_mat)

    # 1.2 Con đường đất liên xã Song Phú - Cái Sơn
    bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 0.05))
    road = bpy.context.active_object
    road.name = "Road_Dirt_CaiSon"
    road.scale = (7.0, 100.0, 0.1) # Rộng 7m, dài 100m
    col.objects.link(road)
    bpy.context.scene.collection.objects.unlink(road)
    
    dirt_mat = create_material("Mat_RedDirtRoad", (0.58, 0.32, 0.18, 1.0), roughness=0.95)
    road.data.materials.append(dirt_mat)

    # 1.3 Kênh rạch nước đục phù sa miền Tây (song song với đường)
    bpy.ops.mesh.primitive_cube_add(size=1, location=(-9.0, 0, -0.3))
    canal = bpy.context.active_object
    canal.name = "Canal_MekongRiver"
    canal.scale = (8.0, 100.0, 0.4)
    col.objects.link(canal)
    bpy.context.scene.collection.objects.unlink(canal)
    
    water_mat = create_material("Mat_RiverWater", (0.35, 0.42, 0.30, 1.0), roughness=0.2)
    canal.data.materials.append(water_mat)

# =============================================================================
# 2. CÂY DỪA NƯỚC & BỤI CÂY PHỤC KÍCH (NIPA PALM & BUSHES)
# =============================================================================

def build_nipa_palm(name, location):
    """Tạo cụm Dừa Nước đặc trưng của sông ngòi Vĩnh Long."""
    col = get_or_create_collection("Props_Vegetation")
    palm_group = bpy.data.objects.new(name, None)
    col.objects.link(palm_group)
    palm_group.location = location

    leaf_mat = create_material("Mat_PalmLeaf", (0.15, 0.40, 0.10, 1.0), roughness=0.7)
    stem_mat = create_material("Mat_PalmStem", (0.35, 0.28, 0.15, 1.0), roughness=0.85)

    num_fronds = 7
    for i in range(num_fronds):
        angle = (2 * math.pi / num_fronds) * i
        # Cuống dừa
        bpy.ops.mesh.primitive_cylinder_add(radius=0.12, depth=3.5, location=(0, 0, 1.5))
        stem = bpy.context.active_object
        stem.name = f"{name}_Stem_{i}"
        stem.rotation_euler = (0.35, 0.2, angle)
        stem.parent = palm_group
        stem.data.materials.append(stem_mat)
        col.objects.link(stem)
        bpy.context.scene.collection.objects.unlink(stem)

        # Tán lá dừa dẹp
        bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 2.2))
        leaf = bpy.context.active_object
        leaf.name = f"{name}_Frond_{i}"
        leaf.scale = (0.5, 2.5, 0.04)
        leaf.rotation_euler = (0.45, 0.2, angle)
        leaf.parent = palm_group
        leaf.data.materials.append(leaf_mat)
        col.objects.link(leaf)
        bpy.context.scene.collection.objects.unlink(leaf)

    return palm_group

def build_foliage_ambush_line():
    """Tạo dãy dừa nước và bờ bụi nơi các chiến sĩ du kích ém quân phục kích."""
    # Hàng dừa dọc bờ kênh bên trái (tọa độ x = -6.5 đến -7.5)
    for y_pos in range(-40, 45, 8):
        build_nipa_palm(f"NipaPalm_Left_{y_pos}", (-6.5, y_pos + (math.sin(y_pos)*1.5), 0))

    # Bụi rậm rải rác bên lề đường
    bush_col = get_or_create_collection("Props_Vegetation")
    bush_mat = create_material("Mat_BushGreen", (0.12, 0.35, 0.08, 1.0), roughness=0.9)
    for y_pos in [-25, -15, -5, 5, 18, 30]:
        bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=1, radius=1.3, location=(-4.8, y_pos, 0.5))
        bush = bpy.context.active_object
        bush.name = f"AmbushBush_{y_pos}"
        bush.scale = (1.2, 1.8, 0.9)
        bush.data.materials.append(bush_mat)
        bush_col.objects.link(bush)
        bpy.context.scene.collection.objects.unlink(bush)

# =============================================================================
# 3. NHÀ TRANH MÁI LÁ NAM BỘ (RUSTIC THATCH HOUSE)
# =============================================================================

def build_thatched_house(name, location, rot_z=0):
    col = get_or_create_collection("Props_Structures")
    house_group = bpy.data.objects.new(name, None)
    col.objects.link(house_group)
    house_group.location = location
    house_group.rotation_euler = (0, 0, rot_z)

    wood_mat = create_material("Mat_BambooWood", (0.50, 0.38, 0.22, 1.0), roughness=0.9)
    thatch_mat = create_material("Mat_ThatchRoof", (0.65, 0.52, 0.28, 1.0), roughness=0.95)

    # Thân nhà
    bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 1.25))
    walls = bpy.context.active_object
    walls.name = f"{name}_Walls"
    walls.scale = (5.0, 7.0, 2.5)
    walls.parent = house_group
    walls.data.materials.append(wood_mat)
    col.objects.link(walls)
    bpy.context.scene.collection.objects.unlink(walls)

    # Mái lá dốc
    bpy.ops.mesh.primitive_cone_add(vertices=4, radius1=4.5, depth=2.0, location=(0, 0, 3.2))
    roof = bpy.context.active_object
    roof.name = f"{name}_Roof"
    roof.scale = (1.0, 1.3, 1.0)
    roof.rotation_euler = (0, 0, math.pi / 4)
    roof.parent = house_group
    roof.data.materials.append(thatch_mat)
    col.objects.link(roof)
    bpy.context.scene.collection.objects.unlink(roof)

# =============================================================================
# 4. ĐOÀN XE TUẦN TRA (MILITARY CONVOY - 1960)
# =============================================================================

def build_jeep(name, location):
    """Xe Jeep chỉ huy chở Tỉnh trưởng Khưu Văn Ba."""
    col = get_or_create_collection("Convoy_Vehicles")
    jeep_group = bpy.data.objects.new(name, None)
    col.objects.link(jeep_group)
    jeep_group.location = location

    jeep_paint = create_material("Mat_MilitaryJeep", (0.22, 0.27, 0.18, 1.0), roughness=0.6)
    metal_dark = create_material("Mat_JeepMetal", (0.1, 0.1, 0.1, 1.0), roughness=0.5)
    tire_mat = create_material("Mat_RubberTire", (0.05, 0.05, 0.05, 1.0), roughness=0.9)
    glass_mat = create_material("Mat_Windshield", (0.6, 0.75, 0.85, 1.0), roughness=0.1)

    # Thân xe Jeep
    bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 0.8))
    body = bpy.context.active_object
    body.name = f"{name}_Chassis"
    body.scale = (1.8, 3.6, 0.8)
    body.parent = jeep_group
    body.data.materials.append(jeep_paint)
    col.objects.link(body)
    bpy.context.scene.collection.objects.unlink(body)

    # Kính chắn gió
    bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0.3, 1.5))
    glass = bpy.context.active_object
    glass.name = f"{name}_Windshield"
    glass.scale = (1.6, 0.1, 0.7)
    glass.parent = jeep_group
    glass.data.materials.append(glass_mat)
    col.objects.link(glass)
    bpy.context.scene.collection.objects.unlink(glass)

    # 4 Bánh xe
    wheel_offsets = [(-1.0, 1.1), (1.0, 1.1), (-1.0, -1.1), (1.0, -1.1)]
    for idx, (ox, oy) in enumerate(wheel_offsets):
        bpy.ops.mesh.primitive_cylinder_add(radius=0.45, depth=0.3, location=(ox, oy, 0.45))
        wheel = bpy.context.active_object
        wheel.name = f"{name}_Wheel_{idx}"
        wheel.rotation_euler = (0, math.pi / 2, 0)
        wheel.parent = jeep_group
        wheel.data.materials.append(tire_mat)
        col.objects.link(wheel)
        bpy.context.scene.collection.objects.unlink(wheel)

    # Bảng cắm cờ hoặc ký hiệu chỉ huy
    bpy.ops.mesh.primitive_cylinder_add(radius=0.03, depth=1.2, location=(0.8, -1.6, 1.3))
    flag_pole = bpy.context.active_object
    flag_pole.name = f"{name}_Antenna"
    flag_pole.parent = jeep_group
    flag_pole.data.materials.append(metal_dark)
    col.objects.link(flag_pole)
    bpy.context.scene.collection.objects.unlink(flag_pole)

    return jeep_group

def build_military_truck(name, location):
    """Xe tải quân sự GMC/Dodge chở binh lính hộ tống."""
    col = get_or_create_collection("Convoy_Vehicles")
    truck_group = bpy.data.objects.new(name, None)
    col.objects.link(truck_group)
    truck_group.location = location

    truck_paint = create_material("Mat_TruckGreen", (0.18, 0.23, 0.15, 1.0), roughness=0.7)
    canvas_mat = create_material("Mat_TruckCanvas", (0.45, 0.42, 0.32, 1.0), roughness=0.95)
    tire_mat = create_material("Mat_RubberTire", (0.05, 0.05, 0.05, 1.0), roughness=0.9)

    # Đầu cabin
    bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 1.6, 1.2))
    cabin = bpy.context.active_object
    cabin.name = f"{name}_Cabin"
    cabin.scale = (2.2, 1.8, 1.6)
    cabin.parent = truck_group
    cabin.data.materials.append(truck_paint)
    col.objects.link(cabin)
    bpy.context.scene.collection.objects.unlink(cabin)

    # Thùng xe bọc bạt
    bpy.ops.mesh.primitive_cube_add(size=1, location=(0, -1.2, 1.5))
    cargo = bpy.context.active_object
    cargo.name = f"{name}_CargoBed"
    cargo.scale = (2.3, 3.8, 2.0)
    cargo.parent = truck_group
    cargo.data.materials.append(canvas_mat)
    col.objects.link(cargo)
    bpy.context.scene.collection.objects.unlink(cargo)

    # 6 Bánh xe tải
    wheel_offsets = [
        (-1.2, 1.7, 0.55), (1.2, 1.7, 0.55),
        (-1.2, -0.6, 0.55), (1.2, -0.6, 0.55),
        (-1.2, -2.1, 0.55), (1.2, -2.1, 0.55)
    ]
    for idx, (ox, oy, oz) in enumerate(wheel_offsets):
        bpy.ops.mesh.primitive_cylinder_add(radius=0.55, depth=0.4, location=(ox, oy, oz))
        wheel = bpy.context.active_object
        wheel.name = f"{name}_Wheel_{idx}"
        wheel.rotation_euler = (0, math.pi / 2, 0)
        wheel.parent = truck_group
        wheel.data.materials.append(tire_mat)
        col.objects.link(wheel)
        bpy.context.scene.collection.objects.unlink(wheel)

    return truck_group

# =============================================================================
# 5. CHIẾN SĨ DU KÍCH CÁI SƠN (REVOLUTIONARY GUERRILLAS)
# =============================================================================

def build_guerrilla_soldier(name, location, aim_direction=(1, 0, 0)):
    """Mô hình chiến sĩ du kích mặc áo bà ba đen, đội nón lá, cầm súng trường."""
    col = get_or_create_collection("Ambush_Guerrillas")
    soldier_group = bpy.data.objects.new(name, None)
    col.objects.link(soldier_group)
    soldier_group.location = location

    black_cloth = create_material("Mat_BaBaDen", (0.08, 0.08, 0.09, 1.0), roughness=0.9)
    skin_mat = create_material("Mat_SkinTone", (0.82, 0.62, 0.48, 1.0), roughness=0.6)
    hat_mat = create_material("Mat_NonLaStraw", (0.80, 0.72, 0.45, 1.0), roughness=0.8)
    rifle_mat = create_material("Mat_RifleWoodMetal", (0.25, 0.18, 0.12, 1.0), roughness=0.7)

    # Thân người (tư thế quỳ nấp)
    bpy.ops.mesh.primitive_cylinder_add(radius=0.28, depth=0.9, location=(0, 0, 0.45))
    torso = bpy.context.active_object
    torso.name = f"{name}_Torso"
    torso.parent = soldier_group
    torso.data.materials.append(black_cloth)
    col.objects.link(torso)
    bpy.context.scene.collection.objects.unlink(torso)

    # Đầu
    bpy.ops.mesh.primitive_uv_sphere_add(radius=0.18, location=(0, 0, 1.05))
    head = bpy.context.active_object
    head.name = f"{name}_Head"
    head.parent = soldier_group
    head.data.materials.append(skin_mat)
    col.objects.link(head)
    bpy.context.scene.collection.objects.unlink(head)

    # Chiếc nón lá truyền thống
    bpy.ops.mesh.primitive_cone_add(vertices=16, radius1=0.45, depth=0.25, location=(0, 0, 1.25))
    hat = bpy.context.active_object
    hat.name = f"{name}_NonLa"
    hat.parent = soldier_group
    hat.data.materials.append(hat_mat)
    col.objects.link(hat)
    bpy.context.scene.collection.objects.unlink(hat)

    # Khẩu súng trường ngắm bắn
    bpy.ops.mesh.primitive_cylinder_add(radius=0.03, depth=1.2, location=(0.3, 0.2, 0.75))
    rifle = bpy.context.active_object
    rifle.name = f"{name}_Rifle"
    rifle.rotation_euler = (0, math.pi / 2.3, math.pi / 2)
    rifle.parent = soldier_group
    rifle.data.materials.append(rifle_mat)
    col.objects.link(rifle)
    bpy.context.scene.collection.objects.unlink(rifle)

    return soldier_group

# =============================================================================
# 6. TỔNG HỢP & XUẤT ASSET (EXPORT PIPELINE CHO UNITY 6)
# =============================================================================

def build_full_cai_son_scene():
    clear_scene()

    # Môi trường & Địa hình
    build_environment()
    build_foliage_ambush_line()

    # Nhà dân 2 bên
    build_thatched_house("House_SongPhu_1", (10.0, 15.0, 0), rot_z=-0.2)
    build_thatched_house("House_SongPhu_2", (12.0, -18.0, 0), rot_z=0.3)

    # Đoàn xe lọt vào trận địa (khoảng cách 12m - 15m giữa các xe)
    # Xe Jeep Tỉnh trưởng Khưu Văn Ba đi đầu
    build_jeep("Convoy_Jeep_KhuuVanBa", (0, 10.0, 0))
    # 2 xe vận tải bám theo
    build_military_truck("Convoy_Truck_Escort_1", (0, -4.0, 0))
    build_military_truck("Convoy_Truck_Escort_2", (0, -18.0, 0))

    # Đội du kích mai phục sát mép dừa và bụi rậm
    build_guerrilla_soldier("Guerrilla_Commander", (-4.6, 9.5, 0))
    build_guerrilla_soldier("Guerrilla_Marksman_1", (-4.5, 12.0, 0))
    build_guerrilla_soldier("Guerrilla_Support_2", (-4.8, -2.0, 0))

    print(">>> Đã xây dựng hoàn tất Scene 3D Trận chiến Cái Sơn (1960)!")

def export_assets(output_dir):
    """Xuất định dạng FBX và glTF/GLB tương thích Unity 6 URP."""
    os.makedirs(output_dir, exist_ok=True)
    fbx_path = os.path.join(output_dir, "CaiSon_Ambush_Environment.fbx")
    gltf_path = os.path.join(output_dir, "CaiSon_Ambush_Environment.glb")

    # Chọn tất cả các mesh & groups
    bpy.ops.object.select_all(action='SELECT')

    # Export FBX chuẩn trục Unity (Forward: -Z, Up: Y)
    try:
        bpy.ops.export_scene.fbx(
            filepath=fbx_path,
            use_selection=True,
            axis_forward='-Z',
            axis_up='Y',
            apply_unit_scale=True,
            bake_space_transform=True
        )
        print(f"[EXPORT] Thành công FBX -> {fbx_path}")
    except Exception as e:
        print(f"[WARN] FBX exporter warning: {e}")

    # Export glTF/GLB
    try:
        bpy.ops.export_scene.gltf(
            filepath=gltf_path,
            export_format='GLB',
            use_selection=True
        )
        print(f"[EXPORT] Thành công glTF -> {gltf_path}")
    except Exception as e:
        print(f"[WARN] glTF exporter warning: {e}")

if __name__ == "__main__":
    build_full_cai_son_scene()

    # Kiểm tra cờ tham số dòng lệnh
    out_dir = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Assets", "Models")
    if "--export-fbx" in sys.argv or "--export-gltf" in sys.argv or True:
        export_assets(out_dir)
