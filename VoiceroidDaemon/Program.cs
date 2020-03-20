using System.IO;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace VoiceroidDaemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Aitalk内でShift-JISの変換を行うため
            // .Net Core標準でサポートされないエンコーディングを追加
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            // 設定ファイルを読み込む
            var setting_path = config.GetValue<string>("setting");
            if (setting_path == null || 0 == setting_path.Length)
            {
                Setting.Path = Path.GetFullPath(path: Setting.DefaultPath);
            }
            else
            {
                Setting.Path = Path.GetFullPath(setting_path);
            }
            if (Setting.Load() == false)
            {
                // 読み出しに失敗したら設定ファイルを新規作成する
                if (Setting.Save() == false)
                {
                    // 作成にも失敗したら終了する
                    throw new IOException("設定ファイルの作成に失敗しました。");
                }
            }

            // Webサーバーを構成する
            var web_host_builder = WebHost.CreateDefaultBuilder();
            web_host_builder.UseConfiguration(config);
            web_host_builder.UseStartup<Startup>();
            if (0 < Setting.System.ListeningAddress.Length)
            {
                // 待ち受けURLが指定されていればURLを設定する
                web_host_builder.UseUrls(Setting.System.ListeningAddress);
            }
            var web_host = web_host_builder.Build();

            // Webサーバーを開始する
            web_host.Run();
        }
    }
}
