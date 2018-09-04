
■全体

	・最初に起動するシーンはGamemManagerとなります。
	・[Edit] > [Project Settings] > [Player]を開き、Inspectorの[Optimization]直下の[ApiCompatibility Level]を[.NET2.0 Subset]から[.NET2.0]に変更してください。
	・Class名、タグ名等、ゲームオブジェクト名はG00_(以下ゲームNO)などの接頭語を付加して他のゲームと当たらないようにしてください。
	・関連ファイルはゲームNOでフォルダを作り、そこにに配置してください。
	・Resourcesを利用する場合はゲーム番号でフォルダを作り、そこを利用してください。

	<フォルダ構成>
	例:G00
	　Assets/
	　　├ G00/←*G00用Assets
	　　├ G01/
	　　├ ...
	　　├ G09/
	　　├ GameManager/
	　　├ Resources/
	　　│　├ G00/←*G00用Resources
	　　│　├ G01/
	　　│　├ ...
	　　│　└ G09/
	　　└ StreamingAssets/
	　　　　├ node.js
	　　　　└ .node/
　　　　　　　　└server.js(SocketIOサーバープログラム)

	<Hierarchy構成>
		[Scene:GameManager]
		├	GameManager … ゲームシーンのロード/アンロードをコントロールします。
		├	SocketIO    … タブレットコントローラーとのsocket通信用コンポーネントです。
		├	NodeJS	    … node.jsのプロセス起動用のコンポーネントです。
		└[Scene:Game00]この下層にゲームをロードします

■座標関連

	<コンポーネント>
	    CoordinateManager CM;    
	
	    void Start(){
	        
	        CM = GameObject.Find("GameManager").GetComponent<CoordinateManager>();
	    
	    }

	<判定部分>
	    if (CM.isUpdate()) {//着弾確認
	        Hashtable res = CM.Get(); //Getを実行するまでisUpdateはtrueを返し続ける。
	       
		Vector3 pos = new Vector3((float)pos["x"], (float)pos["y"], 0f);

	<Hashtableの中身>
	    x:raycast用x座標
	    y:raycast用y座標
	    energy:エネルギー(J)
	    speed:弾速(m/s)
	    hittim:謎
	    ui_x:UGUIでのx座標
	    ui_y:UGUIでのy座標
	    st_x:センサーの生x座標(mm)
	    st_y:センサーの生y座標(mm)
	    sc_w:画面の幅(px)
	    sc_h:画面の高さ(px)
	    se_w:センサーの幅(mm)
	    se_h:センサーの高さ(mm)
	    adjust_x:現在補正しているx方向の量(px)
	    adjust_y:現在補正しているy方向の量(px)


■スコア関連
	
	配列で、プレイヤーID、スコア、プレイ終了時間を関数に渡します。

	<例:プレイヤー情報取得>
	GameController GC = GameObject.Find("GameManager").GetComponent<GameController>();
        string[] idm = new string[2];//プレイヤーID
        string[] name = new string[2];//プレイヤー名
        int[] score = new int[2];//スコア
        string[] idate = new string[2];//プレイ終了時間
	
	//プレイヤーID、プレイヤー名の取得
        int cnt = 0;
        for (int i=0;i<= GC.player_isentry.Length; i++)
        {
            if (GC.player_isentry[i] == true)//player_isentryがtrueの人が参加
            {
                idm[cnt] = GC.player_id[i];
                name[cnt] = GC.player_id[i];
                cnt++;
            }
        }

	<例:点数登録>
	//現在時刻を取得
	idate[0] = GC.Now();
        idate[1] = idate[0];

        GC.ScoreUpdate(idm, score, idate);


■ゲーム終了

	次のメソッドを実行するとゲーム終了シーンを呼び出します。
	GameObject.Find("GameManager").GetComponent<GameController>().GameEnd();


■タブレットコントローラー関連

	<コンポーネント>
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

	<イベントリスナー>
        socket.On("/change", (SocketIOEvent e) => {
            Debug.Log(e);
            /*

                チェンジ処理

            */
        });

	<イベント送信>
		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("error", 0);
        jsonObject.AddField("data", "送りたいデータ");
        socket.Emit("/change", jsonObject);

	<イベント一覧>
		/title       … タイトルシーンを呼び出します。
		/start       … スタート画面で実行するとゲームセレクトシーンを呼び出します。
		/roll        … 役割を設定します。(machine/controller)
		/rollcheck   … 現在設定されている役割を返します。
		/demo        … デモムービーを再生します。
		/isinput     … 入力の許可/不可を返します。
		/calibration … 座標のキャリブレーションシーンを呼び出します。
		/entry       … 参加するユーザーを選択肢として設定します。
		/join        … /entryのユーザーリストからゲームに参加するユーザーを設定します。
		/game        … 指定されたゲームを呼び出します。

■実行環境

	<ディスプレイ>
		サイズ:84インチ
		画面解像度:FULLHD(1920×1080px)
		画面寸法:1864.8mm×1050.8mm
		縦横比:16:9

	<PC>
		OS:Win10
		CPU:i5-7600 3.5GHz
		メモリ:DDR4 16GB
		HDD:SSD256GB
		グラボ:GTX1050Ti