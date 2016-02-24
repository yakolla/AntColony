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
	int				m_maxCarryCount = 0;

	List<Peace>	m_peaces = new List<Peace>();

	ModifiedTexture2D	m_modifiedTexture2D = new ModifiedTexture2D();
	SpawnBaseObj	m_owner;
	// Use this for initialization
	public void Init (SpawnBaseObj owner, SpriteRenderer renderer, int rows, int cols) {

		m_owner = owner;
		m_carryHolder = renderer;
		m_width = cols*Helper.ONE_PEACE_SIZE;
		m_height = rows*Helper.ONE_PEACE_SIZE;
		m_rows = rows;
		m_cols = cols;
		m_maxCarryCount = m_rows*m_cols;

		Texture2D aa = new Texture2D(m_width, m_height);
		Color32[] colors = new Color32[m_width*m_height];
		for(int i = 0; i < m_width*m_height; ++i)
			colors[i] = new Color32(255, 255, 255, 0);

		m_carryHolder.sprite = Sprite.Create(m_modifiedTexture2D.Init(aa), new Rect(0, 0, m_width, m_height), new Vector2(0.5f, 0.5f));
		m_modifiedTexture2D.SetPixels32ByIndex(0, m_cols, m_width, m_height, colors);
		m_modifiedTexture2D.Update();
	}


	public void PutOn(Peace peace)
	{
		if (peace == null)
			return;

		if (m_carryCount == MaxCarryCount)
			return;

		peace.Holder = this;
		m_modifiedTexture2D.SetPixels32ByIndex(m_carryCount, m_cols, Helper.ONE_PEACE_SIZE, Helper.ONE_PEACE_SIZE, peace.Img.GetPixels32());
		m_modifiedTexture2D.Update();
		++m_carryCount;
		m_peaces.Add(peace);
	}

	public Peace Takeout()
	{
		if (m_carryCount == 0)
			return null;


		Peace peace = m_peaces[0];
		peace.Holder = null;
		m_peaces.RemoveAt(0);

		--m_carryCount;
		Texture2D tex = m_modifiedTexture2D.SliceByIndex(m_carryCount, m_cols, m_width/m_cols, m_height/m_rows);
		m_modifiedTexture2D.Update();

		return peace;
	}

	public void Update()
	{
		for (int i = 0; i < m_peaces.Count; ++i)
		{
			if (m_peaces[i].Holder == null)
				continue;

			m_peaces[i].Update();
		}
	}

	public void Clear()
	{
		while(0 < m_peaces.Count)
		{
			Takeout();
		}
	}

	public int CarryCount
	{
		get { return m_carryCount;}
	}

	public int MaxCarryCount
	{
		get {return m_maxCarryCount;}
	}

	public SpawnBaseObj Onwer
	{
		get {return m_owner;}
	}
}
