using System.Collections.Generic;
using Aitalk;
using Microsoft.AspNetCore.Mvc;

namespace VoiceroidDaemon.Controllers
{

    [Route("api/get/current_speaker")]
    [ApiController]
    public class GetCurrentSpeakerController : ControllerBase
    {
        /// <summary>
        /// 現在の話者情報を取得する
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCurrentSpeaker()
        {
            return Ok(Setting.Speaker);
        }
    }


    [Route("api/get/speakers")]
    [ApiController]
    public class GetSpeakersController : ControllerBase
    {
        /// <summary>
        /// 話者リストを取得する
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
