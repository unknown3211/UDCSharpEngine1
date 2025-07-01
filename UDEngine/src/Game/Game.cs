using UDEngine.src.Engine;
using System.Numerics;

namespace UDEngine.src.Game
{
    internal class Game
    {
        private Window window = new Window();
        private Mesh? cubetest;
        private Vector3? playerpos = null;

        private readonly int s_width = 800;
        private readonly int s_height = 600;
        private readonly int t_fps = 60;
        private readonly string title = "UD Game 01";

        private void OnStart()
        {
            cubetest = new Mesh("../../../resources/models/bottles/whisky_bottle.glb");
            cubetest.LoadTexture("../../../resources/textures/bottles/beer_bottle.png");
            cubetest.SetPosition(new Vector3(0, 2, 0));
            cubetest.SetScale(0.2f);

            var camera = window.GetCamera();
            if (camera != null)
            {
                camera.SetPosition(new Vector3(0, 10, 10));
                camera.SetTarget(new Vector3(0, 0, 0));
                camera.SetFOV(50.0f);
            }
        }

        private void OnUpdate()
        {
            Draw.Default3DScene();
            cubetest?.Draw();

            if (cubetest?.M_Debug == true)
            {
                cubetest?.DrawBoundingBoxes();
            }

            cubetest?.SpinDatShitBruh();

            if (Input.IsKeyPressed(KeyCode.KEY_J))
            {
                playerpos = cubetest?.GetPosition();
                if (playerpos != null)
                {
                    Console.WriteLine($"Player Position: {playerpos.Value.X}, {playerpos.Value.Y}, {playerpos.Value.Z}");
                }
            }
            if (Input.IsKeyPressed(KeyCode.KEY_G))
            {
                cubetest?.RemoveTexture();
            }
        }

        private void OnShutdown()
        {
            cubetest?.Unload();
        }

        public void Init()
        {
            window.CreateWindow(s_width, s_height, t_fps, title, enable3D: true);
            window.Start(OnStart);
            window.Update(OnUpdate);
            window.Shutdown(OnShutdown);
        }
    }
}