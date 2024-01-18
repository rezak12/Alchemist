using System;

namespace Code.Infrastructure.Services.Settings
{
    [Serializable]
    public class GameSettings
    {
        public VolumeSettings VolumeSettings;

        public GameSettings(VolumeSettings volumeSettings) => VolumeSettings = volumeSettings;
    }
}