using System;

namespace DataDynamics.PageFX.FlashLand.Flv
{
    [Flags]
    public enum FlvFileFlags
    {
        None = 0,
        HasVideoTags = 1,
        HasAudioTags = 4,
    }

    public enum FlvTagType
    {
        Audio = 8,
        Video = 9,
        ScriptData = 18,
    }

    public enum FlvSoundFormat
    {
        Uncompressed,
        ADPCM,
        MP3,
        NellymoserMono,
        Nellymoser
    }

    public enum FlvSoundRate
    {
        Kz5_5,
        Kz11,
        Kz22,
        Kz44,
    }

    public enum FlvSoundType
    {
        Mono,
        Stereo
    }

    public enum FlvFrameType
    {
        KeyFrame = 1,
        InterFrame = 2,
        DisposableInterFrame = 3, //(H.263 only)
    }

    public enum FlvCodecID
    {
        H263 = 2,
        ScreenVideo = 3,
        VP6 = 4,
        VP6Alpha = 5,
        ScreenVideo2 = 6,
    }
}