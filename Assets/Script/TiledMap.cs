using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TiledMap  {

	public enum Type
	{
		CLOSE_TILE = 1,
		OPEN_TILE,
		ROOM_TILE,
		FOOD_TILE,
		NATURAL_ENEMY_TILE,
		HILL_TILE,
		COUNT
	}

	ModifiedTexture2D	m_modifiedTexture = new ModifiedTexture2D();
	TiledMap.Type[,] m_tiles;
	bool m_dirty = false;

	Color	m_color;

	// Use this for initialization
	public void Init (SpriteRenderer renderer, Color color) {

		m_color = color;
		renderer.sprite = Sprite.Create(m_modifiedTexture.Init(renderer.sprite.texture), renderer.sprite.rect, new Vector2(0f, 1f), 10);
		m_tiles = new TiledMap.Type[m_modifiedTexture.Height, m_modifiedTexture.Width];
		for(int y = 0; y < m_modifiedTexture.Height; ++y)
		{
			for(int x = 0; x < m_modifiedTexture.Width; ++x)
			{
				if (80 <= Random.Range(0, 100))
					SetPixel(x, y, TiledMap.Type.HILL_TILE);
				else
					SetPixel(x, y, TiledMap.Type.CLOSE_TILE);
			}
		}

		for(int x = 0; x < m_modifiedTexture.Width; ++x)
			SetPixel(x, 0, TiledMap.Type.CLOSE_TILE);


	}

	
	// Update is called once per frame
	public void Update () {
		if (m_dirty == true)
		{
			m_modifiedTexture.Update();
			m_dirty = false;
		}

	}

	public void SetPixel(int x, int y, TiledMap.Type value)
	{
		if (UnableTo(x, y))
			return;

		if (m_tiles[y, x] == value)
		{
			return;
		}

		m_dirty = true;
		m_tiles[y, x] = value;

		switch(value)
		{
		case TiledMap.Type.OPEN_TILE:

			m_modifiedTexture.SetPixel(x, y, new Color32(0, 0, 0, 0));
			break;
		case TiledMap.Type.ROOM_TILE:
			m_modifiedTexture.SetPixel(x, y, Color.red);
			break;
		case TiledMap.Type.FOOD_TILE:
			m_modifiedTexture.SetPixel(x, y, Color.cyan);
			break;
		//case Helper.BLOCK_TILE:
		//	m_img.SetPixel(x, m_img.height-y, Color.gray);
		//	break;
		default:
			m_modifiedTexture.SetPixel(x, y, m_color);
			break;
		}			
	}

	public void SetPixel(Vector3 pos, TiledMap.Type value)
	{
		Point cpt = Point.ToPoint(pos);
		SetPixel(cpt.x, cpt.y, value);
	}

	public TiledMap.Type[,] Tiles
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
