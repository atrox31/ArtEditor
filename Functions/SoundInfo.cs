using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArtCore_Editor.Functions;

public static class SoundInfo
{
    [DllImport("winmm.dll")]
    private static extern uint mciSendString(
        string command,
        StringBuilder returnValue,
        int returnLength,
        IntPtr winHandle);

    // Get wav file length in ms
    public static int GetSoundLength(string fileName)
    {
        StringBuilder lengthBuf = new StringBuilder(32);

        _ = mciSendString($"open \"{fileName}\" type waveaudio alias wave", null, 0, IntPtr.Zero);
        _ = mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
        _ = mciSendString("close wave", null, 0, IntPtr.Zero);

        int.TryParse(lengthBuf.ToString(), out int length);

        return length;
    }
}