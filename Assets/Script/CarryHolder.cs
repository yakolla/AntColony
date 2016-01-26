using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarryHolder {

	SpriteRenderer	m_carryHolder;
	int				m_width = 128;
	int				m_height = 128;
	int				m_rows;
	int				m_cols;
	int				m_carryCount = 0;

	List<CarryAble>	m_carryAbles = new List<CarryAble>();

	ModifiedTexture2D	m_modifiedTexture2D = new ModifiedTexture2D();
	SpawnBaseObj	m_owner;
	// Use this for initialization
	public void Init (SpawnBaseObj owner, SpriteRenderer renderer, int rows, int cols, int carryCount) {

		m_owner = owner;
		m_carryHolder = renderer;
		m_width = cols*Helper.ONE_PEACE_SIZE;
		m_height = rows*Helper.ONE_PEACE_SIZE;
		m_rows = rows;
		m_cols = cols;

		Texture2D aa = new Texture2D(m_width, m_height);
		Color32[] colors = new Color32[m_width*m_height];
		for(int i = 0; i < m_width*m_height; ++i)
			colors[i] = new Color32(255, 255, 255, 0);

		m_carryHolder.sprite = Sprite.Create(m_modifiedTexture2D.Init(aa), new Rect(0, 0, m_width, m_height), new Vector2(0.5f, 0.5f));
		m_modifiedTexture2D.SetPixels32ByIndex(0, m_cols, m_width, m_height, 0, colors);
		m_modifiedTexture2D.Update();
	}


	public void PutOn(CarryAble carry)
	{
		if (carry == null)
			return;

		if (m_carryCount == m_rows*m_cols)
			return;

		carry.SetCarryHolder(this);
		m_modifiedTexture2D.SetPixels32ByIndex(m_carryCount, m_cols, Helper.ONE_PEACE_SIZE, Helper.ONE_PEACE_SIZE, 0, carry.Img.GetPixels32());
		m_modifiedTexture2D.Update();
		++m_carryCount;
		m_carryAbles.Add(carry);
	}

	public CarryAble Takeout()
	{
		if (m_carryCount == 0)
			return null;


		CarryAble carry = m_carryAbles[0];
		carry.SetCarryHolder(null);
		m_carryAbles.RemoveAt(0);

		--m_carryCount;
		Texture2D tex = m_modifiedTexture2D.SliceByIndex(m_carryCount, m_cols, m_width/m_cols, m_height/m_rows, 0);
		m_modifiedTexture2D.Update();

		return carry;
	}

	public void Update()
	{
		for (int i = 0; i < m_carryAbles.Count; ++i)
		{
			m_carryAbles[i].Update();
		}
	}

	public int CarryCount
	{
		get { return m_carryCount;}
	}

	public SpawnBaseObj Onwer
	{
		get {return m_owner;}
	}
}
