using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModifiedTexture2D {

	Texture2D m_img;


	// Use this for initialization
	public Texture2D Init (Texture2D img) {

		//m_img = img;

		m_img = new Texture2D(img.width, img.height);
		m_img.SetPixels(img.GetPixels());



		return m_img;
	}
	
	// Update is called once per frame
	public void Update () {
		m_img.Apply();
	}

	public void SetPixel(int x, int y, Color32 color)
	{
		m_img.SetPixel(x, m_img.height-y-1, color);
	}

	public void SetPixels32ByIndex(int index, int cols, int blockWidth, int blockHeight, Color32[] colors)
	{
		for(int yy = 0; yy < blockHeight; ++yy)
		{
			for(int xx = 0; xx < blockWidth; ++xx)
			{
				int x = ((index%cols)*blockWidth);
				int y = ((index/cols)*blockWidth);

				SetPixel(xx+x, yy+y, colors[(xx+yy*blockWidth)]);
			}
		}
	}

	public Color32 GetPixelColor(int x, int y)
	{
		return m_img.GetPixel(x, y);
	}

	public Texture2D Slice(int x, int y, int blockWidth, int blockHeight)
	{
		Color[] colors = m_img.GetPixels(x, (m_img.height-y)-blockHeight, blockWidth, blockHeight);
		Texture2D tex = new Texture2D(blockWidth, blockHeight);
		tex.SetPixels(colors);
		tex.Apply();

		for(int yy = 0; yy < blockHeight; ++yy)
		{
			for(int xx = 0; xx < blockWidth; ++xx)
			{
				SetPixel(x+xx, y+yy,  new Color32(255,255,255,0));
			}
		}

		return tex;
	}

	public Texture2D SliceByIndex(int index, int cols, int blockWidth, int blockHeight)
	{
		int x = ((index%cols)*blockWidth);
		int y = ((index/cols)*blockWidth);
		
		return Slice(x, y, blockWidth, blockHeight);
	}


	public bool UnableTo(int x, int y)
	{
		return x < 0 || y < 0 || x >= m_img.width || y >= m_img.height;
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
