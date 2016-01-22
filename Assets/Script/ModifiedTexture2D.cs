using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModifiedTexture2D {

	Texture2D m_img;
	byte[,] m_tiles;

	// Use this for initialization
	public Texture2D Init (Texture2D img) {

		//m_img = img;

		m_img = new Texture2D(img.width, img.height);
		m_img.SetPixels(img.GetPixels());

		m_tiles = new byte[m_img.height, m_img.width];

		return m_img;
	}
	
	// Update is called once per frame
	public void Update () {
		m_img.Apply();
	}

	public void SetPixel(int x, int y, byte value, Color32 color)
	{
		m_tiles[y, x] = value;
		m_img.SetPixel(x, m_img.height-y-1, color);
	}

	public void SetPixels(int x, int y, int blockWidth, int blockHeight, byte value, Color[] colors)
	{
		for(int yy = 0; yy < blockHeight; ++yy)
		{
			for(int xx = 0; xx < blockWidth; ++xx)
			{
				m_tiles[yy+y, xx+x] = value;
			}
		}
		m_img.SetPixels(x, m_img.height-y, blockWidth, blockHeight, colors);
	}

	public Texture2D Slice(int x, int y, int blockWidth, int blockHeight, byte value)
	{
		Color[] colors = m_img.GetPixels(x, (m_img.height-y)-blockHeight, blockWidth, blockHeight);
		Texture2D tex = new Texture2D(blockWidth, blockHeight);
		tex.SetPixels(colors);
		tex.Apply();

		for(int yy = 0; yy < blockHeight; ++yy)
		{
			for(int xx = 0; xx < blockWidth; ++xx)
			{
				SetPixel(x+xx, y+yy, value, new Color32(255,255,255,0));
			}
		}

		return tex;
	}


	public bool UnableTo(int x, int y)
	{
		return x < 0 || y < 0 || x >= m_img.width || y >= m_img.height;
	}

	public byte[,] Tiles
	{
		get {
			return m_tiles;
		}
	}

	public int Height
	{
		get { return m_img.height; }
	}

	public int Width
	{
		get { return m_img.width; }
	}
}
