﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace VoiceroidDaemon.Controllers
{
    /// <summary>
    /// テキストを読み仮名変換するコントローラ
    /// </summary>
    [Route("api/getkey")]
    [ApiController]
    public class GetKeyApiController : ControllerBase
    {
        /// <summary>
        /// 起動中のVOICEROID2エディタから認証コードを取得する
        /// </summary>
        /// <returns></returns>
        [HttpGet("{exe}")]
        public string GetKey(string exe)
        {
            try
            {
                var start_info = new ProcessStartInfo
                {
                    FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Injecter.exe",
                    Arguments = "\"" + Setting.System.InstallPath + "\\" + (exe ?? Setting.System.VoiceroidEditorExe) + "\"",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                };
                using var process = Process.Start(start_info);
                process.WaitForExit(10000);
                if (process.HasExited == true)
                {
                    var result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return result;
                }
                else
                {
                    process.Kill();
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}
