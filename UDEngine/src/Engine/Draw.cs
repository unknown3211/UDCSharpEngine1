using Raylib_cs;
using RL = Raylib_cs.Raylib;
using System.Numerics;

namespace UDEngine.src.Engine
{
    public static class Draw
    {
        private static Vector3 groundPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private static Vector2 groundSize = new Vector2(10.0f, 10.0f);

        public static void DrawText(string text, int posX, int posY, int fontSize, Color color)
        {
            RL.DrawText(text, posX, posY, fontSize, color);
        }

        public static void DrawRectangle(int posX, int posY, int width, int height, Color color)
        {
            RL.DrawRectangle(posX, posY, width, height, color);
        }

        public static void DrawCircle(int centerX, int centerY, float radius, Color color)
        {
            RL.DrawCircle(centerX, centerY, radius, color);
        }

        public static void DrawFPS(int posX, int posY)
        {
            RL.DrawFPS(posX, posY);
        }

        public static void Default3DScene()
        {
            RL.DrawPlane(groundPosition, groundSize, Colour.Gray);
        }
    }
}

/*
Example:

    Draw.DrawText("Hello, World!", 10, 10, 20, Colour.White);
    Draw.DrawRectangle(50, 50, 100, 50, Colour.Blue);
    Draw.DrawCircle(200, 200, 30.0f, Colour.Red);
    Draw.DrawFPS(10, 10);

*/