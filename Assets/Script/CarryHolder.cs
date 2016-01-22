using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarryHolder {

	SpriteRenderer	m_carryHolder;

	// Use this for initialization
	public void Init (SpriteRenderer renderer) {
		m_carryHolder = renderer;
	}


	public void PutSomething(Texture2D tex)
	{
		m_carryHolder.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
	}

	public Texture2D Takeout()
	{
		Texture2D tex = m_carryHolder.sprite.texture;
		m_carryHolder.sprite = null;
		return tex;
	}

	public bool IsExists()
	{
		return m_carryHolder.sprite != null;
	}
}
