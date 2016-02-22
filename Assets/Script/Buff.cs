using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuffType
{
	Speed,
	Count
}

[System.Serializable]
public struct Buff 
{
	public BuffType type;
	public float	value;
	public float	duration;
	public Color	color;
}
