using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Point {

	public int x;
	public int y;

	public override string ToString ()
	{
		return "x:" + x + " y:" + y;
	}

	public static Point ToPoint(Vector3 v)
	{
		v *= 10f;
		Point pt;
		pt.x = (int)(v.x);
		pt.y = -(int)(v.y);

		return pt;
	}
	
	public static Vector3 ToVector(Point pt)
	{
		return new Vector3(pt.x*0.1f, -pt.y*0.1f, 0);
	}

	public static int Distance(Vector3 src, Vector3 dest)
	{
		Point s = ToPoint(src);
		Point d = ToPoint(dest);
		int dx = d.x-s.x;
		int dy = d.y-s.y;
		return dx*dx + dy*dy;
	}
}
