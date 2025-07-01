using Raylib_cs;
using RL = Raylib_cs.Raylib;
using System.Numerics;

namespace UDEngine.src.Engine
{
    internal class Window
    {
        private Camera? camera;
        private bool is3DMode;
        public Camera? GetCamera() => camera;

        public void Start(Action startCB)
        {
            startCB?.Invoke();
            Console.WriteLine("[INFO]: UD Engine Started...\n");
        }

        public void CreateWindow(int width, int height, int fps, string title, bool enable3D = false)
        {
            RL.InitWindow(width, height, title);
            RL.InitAudioDevice();
            RL.SetTargetFPS(fps);

            is3DMode = enable3D;
            if (is3DMode)
            {
                camera = new Camera();
            }
        }

        public void Update(Action updateCB)
        {
            while (!RL.WindowShouldClose())
            {
                RL.BeginDrawing();
                RL.ClearBackground(Color.Black);
                RL.DrawFPS(10, 10);

                if (is3DMode)
                {
                    camera?.BeginMode3D();
                }

                updateCB?.Invoke();

                if (is3DMode)
                {
                    camera?.EndMode3D();
                }

                RL.EndDrawing();
            }
        }

        public void Shutdown(Action shutdownCB)
        {
            shutdownCB?.Invoke();
            RL.CloseAudioDevice();
            RL.CloseWindow();
            Console.WriteLine("[INFO]: UD Engine Successfully Shutdown !\n");
        }
    }
}
