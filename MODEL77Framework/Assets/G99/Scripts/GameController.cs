using System;
using UnityEngine;

public class GameController : MonoBehaviour {

	[Header("PLAYER INFO")]
	public string[] player_id = { "0000000000000001", "0000000000000002"}; //idm
	public bool[] player_isentry = { true, true }; //参加/不参加
	public string[] player_name = { "プレイヤー1", "プレイヤー2" }; //username
	public string[] player_no2id;//順番
	public int[] player_score = { 10000, 100}; //スコア
	public int[] player_rank = { 1, 2 }; //ランキング
	public int[] player_rate; //命中率

	public void ScoreUpdate(string[] idm, int[] score, string[] idate)
	{

	}

	public void GameEnd()
	{

	}

	public string Now()
	{
		DateTime dt = DateTime.Now;
		return dt.ToString("u", System.Globalization.DateTimeFormatInfo.InvariantInfo).Substring(0, 19);
	}
}
