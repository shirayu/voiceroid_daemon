using System.Runtime.Serialization;

namespace VoiceroidDaemon
{
    [DataContract]
    internal struct SettingJson
    {
        [DataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        public Models.SystemSettingModel System;

        [DataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        public Models.SpeakerSettingModel Speaker;
    }
}
