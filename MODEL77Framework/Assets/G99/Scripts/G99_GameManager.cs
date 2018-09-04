using System.Collections;
using UnityEngine;

public class G99_GameManager : MonoBehaviour
{
	// フレームワークの関数を使うために必要
	private GameController _gameController;
	private CoordinateManager _coordinateManager;

	// GameController.cs から値をもらって保存しておく変数
	private int playerNum;
	private string[] playerId;

	private G99_UIView _uiView;

	[SerializeField]
	private int _targetCount = 5;
	public int TargetCount {
		get { return _targetCount; }
		set { _targetCount = value; }
	}

	private bool _isGameEnd;

	[SerializeField]
	private GameObject _hitEffect;

	void Start()
	{
		_gameController = GameObject.Find("GameManager").GetComponent<GameController>();
		_coordinateManager = GameObject.Find("GameManager").GetComponent<CoordinateManager>();

		_uiView = this.GetComponent<G99_UIView>();
	}

	void Update()
	{
		// 画面に弾がヒットした場合 isUpdate() はtrueを返す。
		if (_coordinateManager.isUpdate() && !_isGameEnd)
		{
			// CoordinateManagerから情報を取得。
			Hashtable res = _coordinateManager.Get();
			Vector3 pos = new Vector3((float)res["x"], (float)res["y"], 0f);

			// 受け取った座標をRayに変換
			Ray ray = Camera.main.ScreenPointToRay(pos);

			// Raycast処理
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit))
			{
				GameObject hitObj = hit.collider.gameObject;

				PlayEffect(pos);

				if (hitObj.name == "Target")
					_targetCount--;

				_uiView.SetTargetCount(_targetCount);

				if (_targetCount <= 0)
				{
					_isGameEnd = true;
					_uiView.DisplayGameEnd();

					// scoreをGameControllerに送信。
					SendScore();

					// ゲームの終わりを通知し、メニュー画面へ
					_gameController.GameEnd();
				}
			}
		}
	}

	public void PlayEffect(Vector3 pos)
	{
		GameObject eff = Instantiate(_hitEffect);
		pos.z = 18f;
		eff.transform.position = Camera.main.ScreenToWorldPoint(pos);
	}

	public void SendScore()
	{
		var score = new int[1];
		score[0] = 10;
		var idm = playerId;
		var idate = new string[1];
		idate[0] = _gameController.Now();

		// 順番は配列の0番目から順にプレイヤー1, プレイヤー2・・・となる。
		// id: プレイヤーの識別番号
		// score: プレイヤーのスコア
		// idate: スコア送信時刻
		_gameController.ScoreUpdate(idm, score, idate);
	}
}
