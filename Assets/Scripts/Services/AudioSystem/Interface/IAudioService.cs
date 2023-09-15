using Domain.Enum;
using UnityEngine;

namespace Services.AudioSystem.Interface
{
    public interface IAudioService
    {
        void PlayUIAudio(AudioUITypes openPage, Transform transform);
    }
}