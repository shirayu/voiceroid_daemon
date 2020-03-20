using System.Collections.Generic;
using Aitalk;
using Microsoft.AspNetCore.Mvc;
using VoiceroidDaemon.Models;

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
        /// 話者リストを取得する．全ライブラリをロードするので時間がかかる．
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
            _ = Setting.ApplySpeakerSetting(Setting.Speaker);
            Setting.Unlock();
            return Ok(libname2speaker);
        }
    }

    [Route("api/set/speaker")]
    [ApiController]
    public class SetSpeakerController : ControllerBase
    {
        /// <summary>
        /// 現在の話者情報を更新する
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SpeakerSetting(SpeakerSettingModel speaker_setting)
        {
            if (speaker_setting != null)
            {
                string error_message = null;
                var saved = false;
                Setting.Lock();
                try
                {
                    error_message = Setting.ApplySpeakerSetting(speaker_setting);
                    saved = Setting.Save();
                }
                finally
                {
                    Setting.Unlock();
                }
                if (saved == false)
                {
                    return BadRequest("Failed to save");
                }
                else if (error_message != null)
                {
                    return BadRequest($"Saved but {error_message}");
                }
                else
                {
                    return Ok("OK");
                }
            }
            return NotFound();
        }
    }
}
