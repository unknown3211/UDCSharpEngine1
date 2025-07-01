using Raylib_cs;
using RL = Raylib_cs.Raylib;
using SharpGLTF.Schema2;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

// CHAT GPT HELPED ME WRITE THIS PART AS THIS MESH CLASS IS VERY HARD TO WRITE

namespace UDEngine.src.Engine
{
    public class Mesh
    {
        private Model model;
        private Vector3 position;
        private Vector3 rotation;
        private float scale;
        private List<BoundingBox> boundingBoxes;
        private bool isLoaded = false;
        private Texture2D? texture = null;
        private bool hasTexture = false;
        public bool M_Debug = false;

        public Mesh(string modelPath)
        {
            position = Vector3.Zero;
            rotation = Vector3.Zero;
            scale = 1.0f;
            boundingBoxes = new List<BoundingBox>();
            LoadModelWithSharpGLTF(modelPath);
        }

        private void LoadModelWithSharpGLTF(string modelPath)
        {
            try
            {
                var gltfModel = ModelRoot.Load(modelPath);
                model = ConvertSharpGLTFToRaylibModel(gltfModel);
                isLoaded = true;
                GenerateBoundingBoxes();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading model {modelPath}: {ex.Message}");
                model = CreateFallbackCubeModel();
                isLoaded = false;
            }
        }

        private Model ConvertSharpGLTFToRaylibModel(ModelRoot gltfModel)
        {
            var gltfMesh = gltfModel.LogicalMeshes.FirstOrDefault();
            if (gltfMesh == null)
            {
                Console.WriteLine("No mesh found in GLTF model");
                return CreateFallbackCubeModel();
            }

            Console.WriteLine($"Found mesh with {gltfMesh.Primitives.Count} primitives");

            var vertices = new List<Vector3>();
            var indices = new List<ushort>();
            var normals = new List<Vector3>();
            var texCoords = new List<Vector2>();
            var colors = new List<Color>();

            foreach (var primitive in gltfMesh.Primitives)
            {
                Console.WriteLine($"Processing primitive with {primitive.GetVertexAccessor("POSITION")?.Count ?? 0} vertices");

                var positions = primitive.GetVertexAccessor("POSITION");
                var normalsAccessor = primitive.GetVertexAccessor("NORMAL");
                var texCoordsAccessor = primitive.GetVertexAccessor("TEXCOORD_0");
                var colorsAccessor = primitive.GetVertexAccessor("COLOR_0");
                var indexAccessor = primitive.GetIndexAccessor();

                if (positions != null)
                {
                    var positionArray = positions.AsVector3Array();
                    var normalArray = normalsAccessor?.AsVector3Array();
                    var texCoordArray = texCoordsAccessor?.AsVector2Array();
                    var colorArray = colorsAccessor?.AsVector4Array();

                    Color materialColor = Color.White;
                    var material = primitive.Material;
                    if (material != null)
                    {
                        Console.WriteLine($"Found material: {material.Name}");

                        var baseColorFactor = material.FindChannel("BaseColor");
                        if (baseColorFactor != null)
                        {
                            Console.WriteLine("Found BaseColor channel");
                            materialColor = new Color(150, 150, 200, 255);
                        }

                        var metallicFactor = material.FindChannel("MetallicFactor");
                        var roughnessFactor = material.FindChannel("RoughnessFactor");

                        if (metallicFactor != null)
                        {
                            Console.WriteLine("Found MetallicFactor channel");
                        }

                        if (roughnessFactor != null)
                        {
                            Console.WriteLine("Found RoughnessFactor channel");
                        }
                    }

                    for (int i = 0; i < positions.Count; i++)
                    {
                        var pos = positionArray[i];
                        vertices.Add(new Vector3(pos.X, pos.Y, pos.Z));

                        if (normalArray != null && i < normalArray.Count)
                        {
                            var normal = normalArray[i];
                            normals.Add(new Vector3(normal.X, normal.Y, normal.Z));
                        }
                        else
                        {
                            normals.Add(Vector3.UnitY);
                        }

                        if (texCoordArray != null && i < texCoordArray.Count)
                        {
                            var texCoord = texCoordArray[i];
                            texCoords.Add(new Vector2(texCoord.X, texCoord.Y));
                        }
                        else
                        {
                            texCoords.Add(Vector2.Zero);
                        }

                        if (colorArray != null && i < colorArray.Count)
                        {
                            var color = colorArray[i];
                            colors.Add(new Color(
                                (byte)(color.X * 255),
                                (byte)(color.Y * 255),
                                (byte)(color.Z * 255),
                                (byte)(color.W * 255)
                            ));
                        }
                        else
                        {
                            colors.Add(materialColor);
                        }
                    }

                    if (indexAccessor != null)
                    {
                        var indexArray = indexAccessor.AsIndicesArray();
                        foreach (var index in indexArray)
                        {
                            indices.Add((ushort)index);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < vertices.Count; i += 3)
                        {
                            if (i + 2 < vertices.Count)
                            {
                                indices.Add((ushort)i);
                                indices.Add((ushort)(i + 1));
                                indices.Add((ushort)(i + 2));
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Converted {vertices.Count} vertices and {indices.Count} indices");

            if (vertices.Count == 0)
            {
                Console.WriteLine("No vertices found, using fallback cube");
                return CreateFallbackCubeModel();
            }

            return CreateRaylibModelFromData(vertices.ToArray(), normals.ToArray(), texCoords.ToArray(), colors.ToArray(), indices.ToArray());
        }

        private unsafe Model CreateRaylibModelFromData(Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, Color[] colors, ushort[] indices)
        {
            var mesh = new Raylib_cs.Mesh();

            mesh.VertexCount = vertices.Length;
            mesh.TriangleCount = indices.Length / 3;

            mesh.Vertices = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal(vertices.Length * 3 * sizeof(float));
            mesh.Normals = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal(normals.Length * 3 * sizeof(float));
            mesh.TexCoords = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal(texCoords.Length * 2 * sizeof(float));
            mesh.Colors = (byte*)System.Runtime.InteropServices.Marshal.AllocHGlobal(colors.Length * 4 * sizeof(byte));
            mesh.Indices = (ushort*)System.Runtime.InteropServices.Marshal.AllocHGlobal(indices.Length * sizeof(ushort));

            for (int i = 0; i < vertices.Length; i++)
            {
                mesh.Vertices[i * 3] = vertices[i].X;
                mesh.Vertices[i * 3 + 1] = vertices[i].Y;
                mesh.Vertices[i * 3 + 2] = vertices[i].Z;
            }

            for (int i = 0; i < normals.Length; i++)
            {
                mesh.Normals[i * 3] = normals[i].X;
                mesh.Normals[i * 3 + 1] = normals[i].Y;
                mesh.Normals[i * 3 + 2] = normals[i].Z;
            }

            for (int i = 0; i < texCoords.Length; i++)
            {
                mesh.TexCoords[i * 2] = texCoords[i].X;
                mesh.TexCoords[i * 2 + 1] = texCoords[i].Y;
            }

            for (int i = 0; i < colors.Length; i++)
            {
                mesh.Colors[i * 4] = colors[i].R;
                mesh.Colors[i * 4 + 1] = colors[i].G;
                mesh.Colors[i * 4 + 2] = colors[i].B;
                mesh.Colors[i * 4 + 3] = colors[i].A;
            }

            for (int i = 0; i < indices.Length; i++)
            {
                mesh.Indices[i] = indices[i];
            }

            RL.UploadMesh(ref mesh, false);

            var model = RL.LoadModelFromMesh(mesh);
            return model;
        }

        private Model CreateFallbackCubeModel()
        {
            var mesh = RL.GenMeshCube(1.0f, 1.0f, 1.0f);
            var model = RL.LoadModelFromMesh(mesh);
            return model;
        }

        private void GenerateBoundingBoxes()
        {
            boundingBoxes.Clear();

            if (!isLoaded) return;

            Matrix4x4 transform =
                Matrix4x4.CreateScale(scale) *
                Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                Matrix4x4.CreateTranslation(position);

            var modelBox = RL.GetModelBoundingBox(model);

            var worldMin = Vector3.Transform(modelBox.Min, transform);
            var worldMax = Vector3.Transform(modelBox.Max, transform);

            boundingBoxes.Add(new BoundingBox
            {
                Min = worldMin,
                Max = worldMax
            });
        }

        public void SetPosition(Vector3 newPosition)
        {
            position = newPosition;
            GenerateBoundingBoxes();
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetRotation(Vector3 newRotation)
        {
            rotation = newRotation;
            GenerateBoundingBoxes();
        }

        public void SetScale(float newScale)
        {
            scale = newScale;
            GenerateBoundingBoxes();
        }

        public void Draw()
        {
            if (!isLoaded) return;

            if (hasTexture && texture.HasValue)
            {
                //Console.WriteLine("Drawing model with texture");

                unsafe
                {
                    RL.SetMaterialTexture(ref model.Materials[0], 0, texture.Value);
                }

                RL.DrawModelEx(
                    model,
                    position,
                    rotation,
                    rotation.Length(),
                    new Vector3(scale, scale, scale),
                    Color.White
                );
            }
            else
            {
                //Console.WriteLine("Drawing model without texture");
                RL.DrawModelEx(
                    model,
                    position,
                    rotation,
                    rotation.Length(),
                    new Vector3(scale, scale, scale),
                    Color.White
                );
            }
        }

        public void DrawBoundingBoxes()
        {
            foreach (var box in boundingBoxes)
            {
                RL.DrawBoundingBox(box, Color.Red);
            }
        }

        public void Unload()
        {
            if (hasTexture && texture.HasValue)
            {
                RL.UnloadTexture(texture.Value);
                texture = null;
                hasTexture = false;
            }

            RL.UnloadModel(model);
        }

        public void SpinDatShitBruh()
        {
            float rotationSpeed = 1.2f;
            float time = (float)RL.GetTime();

            Vector3 newRotation = new Vector3(
                0,
                time * rotationSpeed * 180.0f,
                0
            );

            SetRotation(newRotation);
        }

        public void LoadTexture(string texturePath)
        {
            try
            {
                Console.WriteLine($"Attempting to load texture: {texturePath}");

                if (!System.IO.File.Exists(texturePath))
                {
                    Console.WriteLine($"Texture file does not exist: {texturePath}");
                    hasTexture = false;
                    return;
                }

                texture = RL.LoadTexture(texturePath);
                hasTexture = true;
                Console.WriteLine($"Texture loaded successfully: {texturePath}");
                Console.WriteLine($"Texture dimensions: {texture.Value.Width}x{texture.Value.Height}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture {texturePath}: {ex.Message}");
                hasTexture = false;
            }
        }

        public void ApplyTexture(Texture2D textureToApply)
        {
            texture = textureToApply;
            hasTexture = true;
        }

        public void RemoveTexture()
        {
            if (texture.HasValue)
            {
                RL.UnloadTexture(texture.Value);
            }
            texture = null;
            hasTexture = false;
        }

        public bool HasTexture()
        {
            return hasTexture;
        }
    }
}