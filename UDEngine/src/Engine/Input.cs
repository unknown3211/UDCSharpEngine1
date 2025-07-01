using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace UDEngine.src.Engine
{
    public enum KeyCode // will need redo as this seems like terrible way
    {
        KEY_A = 65,
        KEY_B = 66,
        KEY_C = 67,
        KEY_D = 68,
        KEY_E = 69,
        KEY_F = 70,
        KEY_G = 71,
        KEY_H = 72,
        KEY_I = 73,
        KEY_J = 74,
        KEY_K = 75,
        KEY_L = 76,
        KEY_M = 77,
        KEY_N = 78,
        KEY_O = 79,
        KEY_P = 80,
        KEY_Q = 81,
        KEY_R = 82,
        KEY_S = 83,
        KEY_T = 84,
        KEY_U = 85,
        KEY_V = 86,
        KEY_W = 87,
        KEY_X = 88,
        KEY_Y = 89,
        KEY_Z = 90,
        KEY_LEFT_ARROW = 263,
        KEY_RIGHT_ARROW = 262,
        KEY_UP_ARROW = 265,
        KEY_DOWN_ARROW = 264,
    }

    public static class Input
    {
        public static bool IsKeyPressed(KeyCode key)
        {
            return RL.IsKeyPressed((KeyboardKey)key);
        }

        public static bool IsKeyDown(KeyCode key)
        {
            return RL.IsKeyDown((KeyboardKey)key);
        }

        public static bool IsKeyReleased(KeyCode key)
        {
            return RL.IsKeyReleased((KeyboardKey)key);
        }

        public static bool IsKeyUp(KeyCode key)
        {
            return RL.IsKeyUp((KeyboardKey)key);
        }
    }
}