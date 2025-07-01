using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace UDEngine.src.Engine
{
    public static class Audio
    {
        public static void PlayAudio_SetVol(Sound sound, float volume)
        {
            if (RL.IsAudioDeviceReady())
            {
                RL.SetSoundVolume(sound, volume);
                RL.PlaySound(sound);
            }
            else
            {
                Console.WriteLine("[ERROR]: Audio Device Not Ready");
            }
        }
    }
}