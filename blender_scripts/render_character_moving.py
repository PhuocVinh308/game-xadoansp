import bpy
import math
import os

# Xóa scene cũ
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)
for col in bpy.data.collections:
    bpy.data.collections.remove(col)

# Tạo Collection
col = bpy.data.collections.new("CaiSon_3D_Scene")
bpy.context.scene.collection.children.link(col)

# Materials
def get_mat(name, color, roughness=0.8):
    mat = bpy.data.materials.new(name=name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = color
        bsdf.inputs["Roughness"].default_value = roughness
    return mat

mat_baba = get_mat("Mat_BaBa", (0.07, 0.07, 0.08, 1.0), 0.9)
mat_pants = get_mat("Mat_Pants", (0.05, 0.05, 0.06, 1.0), 0.9)
mat_skin = get_mat("Mat_Skin", (0.82, 0.60, 0.44, 1.0), 0.5)
mat_scarf = get_mat("Mat_Scarf", (0.85, 0.85, 0.85, 1.0), 0.7)
mat_nonla = get_mat("Mat_NonLa", (0.82, 0.70, 0.40, 1.0), 0.8)
mat_sandal = get_mat("Mat_Sandal", (0.08, 0.08, 0.08, 1.0), 0.95)
mat_wood = get_mat("Mat_Wood", (0.28, 0.18, 0.10, 1.0), 0.75)
mat_metal = get_mat("Mat_Metal", (0.12, 0.12, 0.12, 1.0), 0.3)
mat_road = get_mat("Mat_Road", (0.55, 0.32, 0.18, 1.0), 0.95)
mat_grass = get_mat("Mat_Grass", (0.22, 0.38, 0.14, 1.0), 0.9)
mat_palm = get_mat("Mat_Palm", (0.18, 0.35, 0.12, 1.0), 0.7)
mat_water = get_mat("Mat_Water", (0.28, 0.38, 0.32, 1.0), 0.2)

# 1. Địa hình đường đất và rạch nước
bpy.ops.mesh.primitive_plane_add(size=40, location=(0, 0, 0))
ground = bpy.context.active_object
ground.data.materials.append(mat_grass)
col.objects.link(ground)
bpy.context.scene.collection.objects.unlink(ground)

bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 0.02))
road = bpy.context.active_object
road.scale = (6.0, 40.0, 0.04)
road.data.materials.append(mat_road)
col.objects.link(road)
bpy.context.scene.collection.objects.unlink(road)

# 2. Cây dừa nước bên đường
for z in [-10, -3, 5, 12]:
    bpy.ops.mesh.primitive_cylinder_add(radius=0.18, depth=2.4, location=(-5.5, z, 1.2))
    trunk = bpy.context.active_object
    trunk.rotation_euler = (0.15, 0.2, 0)
    trunk.data.materials.append(mat_wood)
    col.objects.link(trunk)
    bpy.context.scene.collection.objects.unlink(trunk)

    for a in [0, 1.1, 2.2, 3.3, 4.4, 5.5]:
        bpy.ops.mesh.primitive_cube_add(size=1, location=(-5.5 + math.cos(a)*1.4, z + math.sin(a)*1.4, 2.3))
        leaf = bpy.context.active_object
        leaf.scale = (0.25, 1.8, 0.05)
        leaf.rotation_euler = (0.4, 0, a)
        leaf.data.materials.append(mat_palm)
        col.objects.link(leaf)
        bpy.context.scene.collection.objects.unlink(leaf)

# 3. NHÂN VẬT 3D TRONG GAME TRONG TƯ THẾ DI CHUYỂN / CHẠY NHANH
char_root = bpy.data.objects.new("Character_3D_Moving", None)
col.objects.link(char_root)
char_root.location = (-1.2, 0, 0)
char_root.rotation_euler = (0, 0, -math.pi / 2.3) # Quay hơi chéo về hướng camera

# Thân người hơi nghiêng về phía trước (tư thế chạy phục kích)
bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 1.05))
torso = bpy.context.active_object
torso.scale = (0.44, 0.28, 0.62)
torso.rotation_euler = (0.25, 0, 0) # Nghiêng về phía trước
torso.parent = char_root
torso.data.materials.append(mat_baba)
col.objects.link(torso)
bpy.context.scene.collection.objects.unlink(torso)

# Đầu & Gương mặt
bpy.ops.mesh.primitive_uv_sphere_add(radius=0.18, location=(0, 0.08, 1.48))
head = bpy.context.active_object
head.parent = char_root
head.data.materials.append(mat_skin)
col.objects.link(head)
bpy.context.scene.collection.objects.unlink(head)

# Nón lá Nam Bộ
bpy.ops.mesh.primitive_cone_add(radius1=0.55, depth=0.25, location=(0, 0.08, 1.62))
nonla = bpy.context.active_object
nonla.rotation_euler = (0.15, 0, 0)
nonla.parent = char_root
nonla.data.materials.append(mat_nonla)
col.objects.link(nonla)
bpy.context.scene.collection.objects.unlink(nonla)

# Khăn Rằn Nam Bộ bay theo chuyển động
bpy.ops.mesh.primitive_torus_add(major_radius=0.18, minor_radius=0.06, location=(0, 0.06, 1.34))
scarf_neck = bpy.context.active_object
scarf_neck.parent = char_root
scarf_neck.data.materials.append(mat_scarf)
col.objects.link(scarf_neck)
bpy.context.scene.collection.objects.unlink(scarf_neck)

bpy.ops.mesh.primitive_cube_add(size=1, location=(-0.1, -0.22, 1.25))
scarf_tail = bpy.context.active_object
scarf_tail.scale = (0.12, 0.35, 0.04)
scarf_tail.rotation_euler = (-0.45, 0.2, 0.3) # Bay về sau theo gió chạy
scarf_tail.parent = char_root
scarf_tail.data.materials.append(mat_scarf)
col.objects.link(scarf_tail)
bpy.context.scene.collection.objects.unlink(scarf_tail)

# CHÂN TRÁI BƯỚC VỀ PHÍA TRƯỚC (stride forward)
bpy.ops.mesh.primitive_cube_add(size=1, location=(-0.16, 0.28, 0.55))
left_leg = bpy.context.active_object
left_leg.scale = (0.16, 0.16, 0.58)
left_leg.rotation_euler = (-0.55, 0, 0)
left_leg.parent = char_root
left_leg.data.materials.append(mat_pants)
col.objects.link(left_leg)
bpy.context.scene.collection.objects.unlink(left_leg)

# Bàn chân trái & Dép lốp
bpy.ops.mesh.primitive_cube_add(size=1, location=(-0.16, 0.48, 0.12))
left_foot = bpy.context.active_object
left_foot.scale = (0.15, 0.26, 0.06)
left_foot.parent = char_root
left_foot.data.materials.append(mat_sandal)
col.objects.link(left_foot)
bpy.context.scene.collection.objects.unlink(left_foot)

# CHÂN PHẢI ĐẨY LÙI VỀ PHÍA SAU (kick back)
bpy.ops.mesh.primitive_cube_add(size=1, location=(0.16, -0.32, 0.62))
right_leg = bpy.context.active_object
right_leg.scale = (0.16, 0.16, 0.58)
right_leg.rotation_euler = (0.65, 0, 0)
right_leg.parent = char_root
right_leg.data.materials.append(mat_pants)
col.objects.link(right_leg)
bpy.context.scene.collection.objects.unlink(right_leg)

# Bàn chân phải nhấc gót lên
bpy.ops.mesh.primitive_cube_add(size=1, location=(0.16, -0.56, 0.42))
right_foot = bpy.context.active_object
right_foot.scale = (0.15, 0.26, 0.06)
right_foot.rotation_euler = (0.5, 0, 0)
right_foot.parent = char_root
right_foot.data.materials.append(mat_sandal)
col.objects.link(right_foot)
bpy.context.scene.collection.objects.unlink(right_foot)

# TAY TRÁI ĐÁNH VỀ PHÍA TRƯỚC
bpy.ops.mesh.primitive_cube_add(size=1, location=(-0.35, 0.22, 1.05))
left_arm = bpy.context.active_object
left_arm.scale = (0.14, 0.14, 0.48)
left_arm.rotation_euler = (-0.6, 0, 0)
left_arm.parent = char_root
left_arm.data.materials.append(mat_baba)
col.objects.link(left_arm)
bpy.context.scene.collection.objects.unlink(left_arm)

# TAY PHẢI VÀ SÚNG TRƯỜNG K-44 (cầm súng chắc chắn chạy)
bpy.ops.mesh.primitive_cube_add(size=1, location=(0.32, -0.05, 1.0))
right_arm = bpy.context.active_object
right_arm.scale = (0.14, 0.14, 0.48)
right_arm.rotation_euler = (0.35, 0, 0)
right_arm.parent = char_root
right_arm.data.materials.append(mat_baba)
col.objects.link(right_arm)
bpy.context.scene.collection.objects.unlink(right_arm)

# Súng K-44 báng gỗ nòng thép
bpy.ops.mesh.primitive_cube_add(size=1, location=(0.34, 0.15, 1.05))
rifle_stock = bpy.context.active_object
rifle_stock.scale = (0.06, 0.95, 0.1)
rifle_stock.rotation_euler = (0.4, 0.2, 0)
rifle_stock.parent = char_root
rifle_stock.data.materials.append(mat_wood)
col.objects.link(rifle_stock)
bpy.context.scene.collection.objects.unlink(rifle_stock)

bpy.ops.mesh.primitive_cylinder_add(radius=0.02, depth=1.1, location=(0.34, 0.45, 1.18))
rifle_barrel = bpy.context.active_object
rifle_barrel.rotation_euler = (math.pi / 2 - 0.4, 0, 0)
rifle_barrel.parent = char_root
rifle_barrel.data.materials.append(mat_metal)
col.objects.link(rifle_barrel)
bpy.context.scene.collection.objects.unlink(rifle_barrel)

# Kính ngắm súng bắn tỉa
bpy.ops.mesh.primitive_cylinder_add(radius=0.025, depth=0.32, location=(0.34, 0.22, 1.25))
rifle_scope = bpy.context.active_object
rifle_scope.rotation_euler = (math.pi / 2 - 0.4, 0, 0)
rifle_scope.parent = char_root
rifle_scope.data.materials.append(mat_metal)
col.objects.link(rifle_scope)
bpy.context.scene.collection.objects.unlink(rifle_scope)

# 4. CAMERA & ĐÈN CHIẾU SÁNG
cam_data = bpy.data.cameras.new("RenderCamera")
cam_data.lens = 50
cam_obj = bpy.data.objects.new("RenderCamera", cam_data)
col.objects.link(cam_obj)
bpy.context.scene.camera = cam_obj
cam_obj.location = (-1.0, -3.8, 1.6)
cam_obj.rotation_euler = (math.radians(78), 0, math.radians(-12))

# Ánh sáng mặt trời chiều vàng 16:15
sun_data = bpy.data.lights.new(name="SunLight", type='SUN')
sun_data.energy = 4.5
sun_data.color = (1.0, 0.92, 0.78)
sun_obj = bpy.data.objects.new(name="SunLight", object_data=sun_data)
sun_obj.location = (10, -10, 15)
sun_obj.rotation_euler = (math.radians(45), math.radians(25), math.radians(-30))
col.objects.link(sun_obj)

# Đèn phụ làm mềm bóng
fill_data = bpy.data.lights.new(name="FillLight", type='POINT')
fill_data.energy = 150
fill_data.color = (0.7, 0.85, 1.0)
fill_obj = bpy.data.objects.new(name="FillLight", object_data=fill_data)
fill_obj.location = (-3, -2, 2.5)
col.objects.link(fill_obj)

# 5. CÀI ĐẶT RENDER (EEVEE / CYCLES)
scene = bpy.context.scene
scene.render.engine = 'BLENDER_EEVEE_NEXT' if hasattr(bpy.types, 'RenderSettings') and 'BLENDER_EEVEE_NEXT' in [e.identifier for e in bpy.types.RenderSettings.bl_rna.properties['engine'].enum_items] else 'BLENDER_EEVEE'
scene.render.resolution_x = 1920
scene.render.resolution_y = 1080
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = 'PNG'
scene.render.filepath = os.path.abspath("nhan_vat_3d_game_di_chuyen.png")

print("Rendering 3D Character Moving Scene to:", scene.render.filepath)
bpy.ops.render.render(write_still=True)
print("RENDER COMPLETED SUCCESSFULLY!")
