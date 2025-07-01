using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace UDEngine.src.Engine
{
    public static class Utils
    {
        public static float GetTime()
        {
            return (float)RL.GetTime();
        }
    }
} 