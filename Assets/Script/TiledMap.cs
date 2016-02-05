using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TiledMap  {

	ModifiedTexture2D	m_modifiedTexture = new ModifiedTexture2D();
	byte[,] m_tiles;

	Color	m_color;

	// Use this for initialization
	public void Init (SpriteRenderer renderer, Color color) {

		m_color = color;
		renderer.sprite = Sprite.Create(m_modifiedTexture.Init(renderer.sprite.texture), renderer.sprite.rect, new Vector2(0f, 1f), 10);
		m_tiles = new byte[m_modifiedTexture.Height, m_modifiedTexture.Width];
		for(int y = 0; y < m_modifiedTexture.Height; ++y)
		{
			for(int x = 0; x < m_modifiedTexture.Width; ++x)
			{
				if (80 <= Random.Range(0, 100))
					SetPixel(x, y, Helper.HILL_TILE);
				else
					SetPixel(x, y, Helper.CLOSE_TILE);
			}
		}

		for(int x = 0; x < m_modifiedTexture.Width; ++x)
			SetPixel(x, 0, Helper.CLOSE_TILE);


	}

	
	// Update is called once per frame
	public void Update () {
		m_modifiedTexture.Update();
	}

	public void SetPixel(int x, int y, byte value)
	{
		m_tiles[y, x] = value;

		switch(value)
		{
		case Helper.OPEN_TILE:

			m_modifiedTexture.SetPixel(x, y, value, new Color32(0, 0, 0, 0));
			break;
		case Helper.ROOM_TILE:
			m_modifiedTexture.SetPixel(x, y, value, Color.red);
			break;
		case Helper.FOOD_TILE:
			m_modifiedTexture.SetPixel(x, y, value, Color.cyan);
			break;
		//case Helper.BLOCK_TILE:
		//	m_img.SetPixel(x, m_img.height-y, Color.gray);
		//	break;
		default:
			m_modifiedTexture.SetPixel(x, y, value, m_color);
			break;
		}			
	}

	public void SetPixel(Vector3 pos, byte value)
	{
		Point cpt = Point.ToPoint(pos);
		if (UnableTo(cpt.x, cpt.y))
			return;

		SetPixel(cpt.x, cpt.y, value);
	}

	public byte[,] Tiles
	{
		get {
			return m_tiles;
		}
	}

	public bool UnableTo(int x, int y)
	{
		return x < 0 || y < 0 || x >= m_modifiedTexture.Width || y >= m_modifiedTexture.Height;
	}

	public int getNodeID(int x, int y)
	{
		return x+y*m_modifiedTexture.Width;
	}

	public Point getPoint(int nodeID)
	{
		Point pt;
		pt.x = nodeID%m_modifiedTexture.Width;
		pt.y = nodeID/m_modifiedTexture.Width;
		return pt;
	}

	public int GetWidth()
	{
		return m_modifiedTexture.Width;
	}
}
