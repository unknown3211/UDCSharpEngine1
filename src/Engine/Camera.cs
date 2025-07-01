using Raylib_cs;
using RL = Raylib_cs.Raylib;
using System.Numerics;

namespace UDEngine.src.Engine
{
    public class Camera
    {
        private Camera3D camera;

        public Camera()
        {
            camera = new Camera3D();
            camera.Position = new Vector3(0, 10, 10);
            camera.Target = new Vector3(0, 0, 0);
            camera.Up = new Vector3(0, 1, 0);
            camera.FovY = 45.0f;
            camera.Projection = CameraProjection.Perspective;
        }

        public void SetPosition(Vector3 position)
        {
            camera.Position = position;
        }

        public void SetTarget(Vector3 target)
        {
            camera.Target = target;
        }

        public void SetUp(Vector3 up)
        {
            camera.Up = up;
        }

        public void SetFOV(float fov)
        {
            camera.FovY = fov;
        }

        public void BeginMode3D()
        {
            RL.BeginMode3D(camera);
        }

        public void EndMode3D()
        {
            RL.EndMode3D();
        }

        public Vector3 GetPosition()
        {
            return camera.Position;
        }
    }
} 