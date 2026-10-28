import bpy
import math
import os

# Clear scene
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)
for col in bpy.data.collections:
    bpy.data.collections.remove(col)

col = bpy.data.collections.new("Bo_Doi_Collection")
bpy.context.scene.collection.children.link(col)

def create_mat(name, color, roughness=0.75, metalness=0.1):
    mat = bpy.data.materials.new(name=name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = color
        bsdf.inputs["Roughness"].default_value = roughness
        bsdf.inputs["Metallic"].default_value = metalness
    return mat

# Materials matching Sketchfab "Bộ Đội Việt Nam" reference
mat_uniform = create_mat("Mat_Uniform", (0.16, 0.28, 0.14, 1.0), 0.85, 0.05)       # Quân phục xanh rêu
mat_uniform_dark = create_mat("Mat_Uniform_Dark", (0.10, 0.18, 0.09, 1.0), 0.9, 0.05)
mat_chestrig = create_mat("Mat_ChestRig", (0.38, 0.35, 0.25, 1.0), 0.85, 0.05)      # Bao đạn ngực kaki
mat_backpack = create_mat("Mat_Backpack", (0.14, 0.22, 0.12, 1.0), 0.9, 0.05)      # Ba lô dã chiến
mat_canteen = create_mat("Mat_Canteen", (0.24, 0.35, 0.22, 1.0), 0.4, 0.3)         # Bình tông nước
mat_skin = create_mat("Mat_Skin", (0.84, 0.62, 0.46, 1.0), 0.55, 0.0)             # Màu da
mat_hair = create_mat("Mat_Hair", (0.08, 0.07, 0.06, 1.0), 0.9, 0.0)              # Tóc ngắn
mat_boots = create_mat("Mat_Boots", (0.05, 0.05, 0.05, 1.0), 0.6, 0.1)            # Giày bốt lính
mat_wood = create_mat("Mat_Wood", (0.32, 0.18, 0.10, 1.0), 0.7, 0.0)              # Báng súng gỗ
mat_metal = create_mat("Mat_Metal", (0.12, 0.12, 0.14, 1.0), 0.3, 0.85)           # Kim loại nòng súng

root = bpy.data.objects.new("Bo_Doi_Viet_Nam", None)
col.objects.link(root)

# Helper to link mesh to collection and parent to root
def add_part(obj, parent=root):
    col.objects.link(obj)
    if obj.name in bpy.context.scene.collection.objects:
        bpy.context.scene.collection.objects.unlink(obj)
    obj.parent = parent
    return obj

# 1. Torso
bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 1.45))
torso = bpy.context.active_object
torso.scale = (0.42, 0.26, 0.58)
torso.data.materials.append(mat_uniform)
add_part(torso)

# V-Neck & Collar
bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0.13, 1.74))
collar = bpy.context.active_object
collar.scale = (0.18, 0.04, 0.16)
collar.data.materials.append(mat_skin)
add_part(collar)

# 2. Chest Rig (4 Ammo pouches)
bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0.15, 1.42))
rig_base = bpy.context.active_object
rig_base.scale = (0.44, 0.04, 0.22)
rig_base.data.materials.append(mat_chestrig)
add_part(rig_base)

for px in [-0.15, -0.05, 0.05, 0.15]:
    bpy.ops.mesh.primitive_cube_add(size=1, location=(px, 0.18, 1.42))
    pouch = bpy.context.active_object
    pouch.scale = (0.08, 0.07, 0.19)
    pouch.data.materials.append(mat_chestrig)
    add_part(pouch)

# 3. Backpack (Ba lô con cóc)
bpy.ops.mesh.primitive_cube_add(size=1, location=(0, -0.21, 1.42))
backpack = bpy.context.active_object
backpack.scale = (0.36, 0.21, 0.44)
backpack.data.materials.append(mat_backpack)
add_part(backpack)

# 4. Canteen (Bình tông nước) on right hip
bpy.ops.mesh.primitive_cylinder_add(radius=0.045, depth=0.16, location=(0.24, 0.02, 1.05))
canteen = bpy.context.active_object
canteen.data.materials.append(mat_canteen)
add_part(canteen)

# 5. Head & Hair
bpy.ops.mesh.primitive_cube_add(size=1, location=(0, 0, 1.95))
head = bpy.context.active_object
head.scale = (0.24, 0.24, 0.26)
head.data.materials.append(mat_skin)
add_part(head)

bpy.ops.mesh.primitive_cube_add(size=1, location=(0, -0.01, 2.08))
hair_top = bpy.context.active_object
hair_top.scale = (0.25, 0.25, 0.08)
hair_top.data.materials.append(mat_hair)
add_part(hair_top)

bpy.ops.mesh.primitive_cube_add(size=1, location=(0, -0.10, 1.96))
hair_back = bpy.context.active_object
hair_back.scale = (0.245, 0.08, 0.18)
hair_back.data.materials.append(mat_hair)
add_part(hair_back)

# 6. Left Leg & Boot
bpy.ops.mesh.primitive_cube_add(size=1, location=(-0.13, 0, 0.65))
l_leg = bpy.context.active_object
l_leg.scale = (0.17, 0.18, 0.50)
l_leg.data.materials.append(mat_uniform)
add_part(l_leg)

bpy.ops.mesh.primitive_cube_add(size=1, location=(-0.13, 0.03, 0.14))
l_boot = bpy.context.active_object
l_boot.scale = (0.18, 0.26, 0.28)
l_boot.data.materials.append(mat_boots)
add_part(l_boot)

# 7. Right Leg & Boot
bpy.ops.mesh.primitive_cube_add(size=1, location=(0.13, 0, 0.65))
r_leg = bpy.context.active_object
r_leg.scale = (0.17, 0.18, 0.50)
r_leg.data.materials.append(mat_uniform)
add_part(r_leg)

bpy.ops.mesh.primitive_cube_add(size=1, location=(0.13, 0.03, 0.14))
r_boot = bpy.context.active_object
r_boot.scale = (0.18, 0.26, 0.28)
r_boot.data.materials.append(mat_boots)
add_part(r_boot)

# 8. Left Arm
bpy.ops.mesh.primitive_cube_add(size=1, location=(-0.28, 0, 1.45))
l_arm = bpy.context.active_object
l_arm.scale = (0.13, 0.13, 0.42)
l_arm.data.materials.append(mat_uniform)
add_part(l_arm)

# 9. Right Arm
bpy.ops.mesh.primitive_cube_add(size=1, location=(0.28, 0, 1.45))
r_arm = bpy.context.active_object
r_arm.scale = (0.13, 0.13, 0.42)
r_arm.data.materials.append(mat_uniform)
add_part(r_arm)

# 10. Vintage Sniper Rifle (K-44 / Mosin Nagant)
bpy.ops.mesh.primitive_cube_add(size=1, location=(0.18, 0.20, 1.25))
stock = bpy.context.active_object
stock.scale = (0.06, 0.80, 0.10)
stock.rotation_euler = (math.pi / 2, 0, 0)
stock.data.materials.append(mat_wood)
add_part(stock)

bpy.ops.mesh.primitive_cylinder_add(radius=0.02, depth=0.90, location=(0.18, 0.40, 1.28))
barrel = bpy.context.active_object
barrel.rotation_euler = (math.pi / 2, 0, 0)
barrel.data.materials.append(mat_metal)
add_part(barrel)

bpy.ops.mesh.primitive_cylinder_add(radius=0.03, depth=0.32, location=(0.18, 0.12, 1.34))
scope = bpy.context.active_object
scope.rotation_euler = (math.pi / 2, 0, 0)
scope.data.materials.append(mat_metal)
add_part(scope)

# Select all objects in collection
bpy.ops.object.select_all(action='DESELECT')
for obj in col.objects:
    obj.select_set(True)

bpy.context.view_layer.objects.active = root

output_glb = os.path.abspath("bo_doi.glb")
bpy.ops.export_scene.gltf(
    filepath=output_glb,
    export_format='GLB',
    use_selection=True,
    export_apply=True
)
print(f"EXPORTED GLB SUCCESSFULLY: {output_glb} (Size: {os.path.getsize(output_glb)} bytes)")
