using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Food : SpawnBaseObj {

	ModifiedTexture2D	m_modifiedTexture = new ModifiedTexture2D();


	int		m_sqrtHP;

	[SerializeField]
	Point m_slice;

	void Start()
	{

		SpriteRenderer renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
		renderer.sprite = Sprite.Create(m_modifiedTexture.Init(renderer.sprite.texture), renderer.sprite.rect, new Vector2(0.5f, 0.5f));

		m_sqrtHP = (int)Mathf.Sqrt(MaxHP);
		m_slice.x = (m_modifiedTexture.Width/m_sqrtHP);
		m_slice.y = (m_modifiedTexture.Height/m_sqrtHP);
		HP = MaxHP;

	}

	void Update()
	{
		m_modifiedTexture.Update();
	}

	override public void StartBuilding()
	{
		Point st = Point.ToPoint(transform.position);
	}

	public Texture2D	Slice()
	{
		Texture2D tex = null;
		int index = (MaxHP-HP);
		--HP;
		
		int x = ((index%m_sqrtHP)*m_slice.x);
		int y = (m_slice.y*(index/m_sqrtHP));
		
		tex = m_modifiedTexture.Slice(x, y, m_slice.x, m_slice.y, 0);
		
		if (0 == HP)
			Helper.GetFoodSpawningPool().Kill(this);

		return tex;
	}
}
