using System.Collections;
using UnityEngine;

public class CoordinateManager : MonoBehaviour {

	private bool is_input;
	private Vector3 mouse_pos;

	//ST84
	private bool hitsts;
	private double x = 0f;
	private double y = 0f;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && is_input)
		{
			is_input = false;
			hitsts = true;
			mouse_pos = Input.mousePosition;
			x = mouse_pos.x;
			y = mouse_pos.y;
		}
		if (Input.GetMouseButtonUp(0) && !is_input)
		{
			is_input = true;
		}
	}

	// 座標の取得
	public Hashtable Get()
	{
		var ret = new Hashtable();
		ret.Add("x", (float)x);
		ret.Add("y", (float)y);
		hitsts = false;
		return ret;
	}

	// 着弾情報があるかのチェック
	public bool isUpdate()
	{
		return hitsts;
	}
}
