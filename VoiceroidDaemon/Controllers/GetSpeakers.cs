using System.Collections.Generic;
using Aitalk;
using Microsoft.AspNetCore.Mvc;

namespace VoiceroidDaemon.Controllers
{
    /// <summary>
    /// 話者情報を取得するするコントローラ
    /// </summary>
    [Route("api/get/speakers")]
    [ApiController]
    public class GetSpeakersController : ControllerBase
    {
        /// <summary>
        /// 話者情報を取得する
        /// </summary>
        /// <returns></returns>
        public IActionResult GetSpeakers()
        {
            var libname2speaker = new Dictionary<string, string[]>();

            Setting.Lock();
            foreach (var libname in AitalkWrapper.VoiceDbList)
            {
                AitalkWrapper.LoadVoice(libname);
                var voice_names = AitalkWrapper.Parameter.VoiceNames;
                libname2speaker.Add(libname, voice_names);
            }
            Setting.ApplySpeakerSetting(Setting.Speaker);
            Setting.Unlock();
           return Ok(libname2speaker);
        }
    }
}
