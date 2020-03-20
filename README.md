# voiceroid_daemon

VOICEROID2のHTTPサーバーデーモン

## 概要

VOICEROID2のDLL(aitalked.dll)を直接叩いて、音声データをHTTPで取得できるサーバーソフトです。  
よってエディターを起動しておく必要はありません。  
ライセンス認証はDLLレベルで行われているため当然ながら動作には有効なライセンスが必要です。  

## ビルド環境

- Visual Studio 2019
- .NET Core 3.1

## 使い方

コマンドプロンプトやPowerShellよりVoiceroidDaemon.exeを起動します。  
初期状態ではポート8080にHTTPサーバーが立ち上がりますので、  
ウェブブラウザを使って`http://127.0.0.1:8080/`にアクセスしてください。  

始めにページ上部のナビゲーションバーから「システムの設定」ページを開き必要な設定事項を入力してください。
aitalked.dllを利用するためにはDLLの認証コードが必要ですが、本ソフトウェアはこれをVOICEROID2エディタから取得することができます。  
「認証コードのシード値」のテキストボックスの下にあるボタンを押すと、取得された文字列がテキストボックスに入力されます。  
待ち受けアドレスを`http://+:8080/`などに設定することで外部からの接続を受け入れることができます。  
その場合、管理者権限でコマンドプロンプトやPowerShellを起動して以下のコマンドを入力しアクセスを許可してください。

```powershell
netsh http add urlacl url=http://+:8080/ user=<Windowsのユーザー名>
```

設定できたら「保存する」ボタンを押してください。  

次にナビゲーションバーから「話者の設定」ページを開き、使用するボイスライブラリと話者を選択してください。  
ここで表示されるボイスライブラリや話者の名前はキャラクターの日本語名ではなくファイルの名前です。  
設定したら「保存する」ボタンを押してください。  

ここまでダイアログでエラーが表示されなかったなら「Home」ページにて仮名変換や音声変換を試すことができるはずです。  

なお、ここまでの設定はカレントディレクトリに「setting.json」というファイルで保存されます。  
しかしVoiceroidDaemonの起動オプションに`/setting=<ファイル名>`と付け加えることでファイル名を変更することができます。

## Web API

仮名変換や音声変換はWeb APIとして呼び出すことが可能です。  
以下にAPIの使用例を示します。
IPアドレスとポートはそれぞれの環境に読み替えてください。

- 文章をVOICEROIDの読み仮名に変換する①  
`http://127.0.0.1:8080/api/converttext/こんにちは` (GETメソッド)  
にアクセスすると`<S>(Irq MARK=_AI@5)コ^ンニチワ<F>`というテキストが返ります。  
[仮名変換についての詳細はこちら](https://blankalilio.blogspot.com/2019/03/voiceroid2aikana.html)  

- 文章をVOICEROIDの読み仮名に変換する②  
`http://127.0.0.1:8080/api/converttext` (POSTメソッド)  
に後述する構造のJSONテキストをPOSTしても変換できます。  

- 文章を音声に変換する。  
`http://127.0.0.1:8080/api/speechtext/こんばんは` (GETメソッド)  
にアクセスするとwavファイルが返ります。  
WAVEファイルのフォーマットは44.1kHz,16bit,モノラルです。  

- 文章を音声に変換する。  
`http://127.0.0.1:8080/api/speechtext` (POSTメソッド)  
に後述する構造のJSONテキストをPOSTしても変換できます。  
この場合、文章の代わりに読み仮名を指定して音声変換することもできます。  

## スピーチパラメータ

読み仮名変換や音声変換はURLリクエストの形で指定する他、JSON形式のテキストをPOSTすることでも行えます。  
以下にフォーマットを示します。  

```txt
{
  "Text" : <読み仮名あるいは音声に変換する文章>,
  "Kana" : <音声に変換する読み仮名>,
  "Speaker" : {
    "Volume" : <音量 (0～2)>,
    "Speed" : <話速 (0.5～4)>,
    "Pitch" : <高さ (0.5～2)>,
    "Emphasis" : <抑揚 (0～2)>,
    "PauseMiddle" : <短ポーズ時間[ms] (80～500) PauseLong以下>,
    "PauseLong" : <長ポーズ時間[ms] (100～2000) PauseMiddle以上>,
    "PauseSentence" : <文末ポーズ時間[ms] (0～10000)>
  }
}
```

指定しないパラメータは省略できます。その場合、ボイスライブラリの初期値が使用されます。  
また、`Volume`, `Speed`, `Pitch`, `Emphasis`はNaNを指定すると初期値が、  
`PauseMiddle`, `PauseLong`, `PauseSentence`は-1を指定すると初期値が使用されます。

## ライセンス

本ソフトウェアの製作にあたって以下のライブラリを使用しています。  

- [Friendly](https://github.com/Codeer-Software/Friendly)
