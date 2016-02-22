using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Background : MonoBehaviour {

	TiledMap	m_tiledMap = new TiledMap();

	[SerializeField]
	int		m_maxFadingTime = 10;


	Vector3 m_startedTouchPos = Vector3.zero;
	RectTransform	m_topPannel;

	Dictionary<int, int>	m_openTiles = new Dictionary<int, int>();



	// Use this for initialization
	void Awake () {
		Application.runInBackground = true;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		m_topPannel = GameObject.Find("HudGUI/Canvas/TopPanel").GetComponent<RectTransform>();



		SpriteRenderer renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
		m_tiledMap.Init(renderer);


		for (int x = 0; x < m_tiledMap.GetWidth(); ++x)
		{
			SetPixel(x, 0, TiledMap.Type.OPEN_TILE);
		}
	}

	void Start()
	{
		StartCoroutine(LoopFadingTile());
	}

	IEnumerator	LoopFadingTile()
	{
		List<int> deletedTile = new List<int>();

		while(true)
		{
			List<int> keys = new List<int> (m_openTiles.Keys);
			foreach(int key in keys)
			{

				--m_openTiles[key];
				
				if (m_openTiles[key] <= 0)
				{
					deletedTile.Add(key);
					Point pt = getPoint(key);
					SetPixel(pt.x, pt.y, TiledMap.Type.CLOSE_TILE);
				}
				
			}

			foreach(int nodeID in deletedTile)
			{
				m_openTiles.Remove(nodeID);
			}

			deletedTile.Clear();

			yield return new WaitForSeconds(1f);
		}


	}
	
	// Update is called once per frame
	void Update () {
		m_tiledMap.Update();

		Helper.CheckTouchDraging((touchedCount, touchPos, touchPhase)=>{

			if (touchPos.y > Screen.height-m_topPannel.rect.height)
				return;

			if (0 < touchedCount) 
			{
				if (touchPhase == TouchPhase.Began)
				{
					m_startedTouchPos = touchPos;
				}
				else if (touchPhase == TouchPhase.Moved)
				{
					float dis = Vector3.Distance(touchPos, m_startedTouchPos);
					Vector3 deltaPos = touchPos - m_startedTouchPos;
					deltaPos.z = 0;
					Camera.main.transform.position += deltaPos.normalized*dis*Time.deltaTime;
					m_startedTouchPos = touchPos;
				}
				else
				{
					m_startedTouchPos = touchPos;
				}				
			}
		});
	}

	public void SetPixel(int x, int y, TiledMap.Type value)
	{
		m_tiledMap.SetPixel(x, y, value);

		if (TiledMap.Type.OPEN_TILE == value)
		{
			if (0 < y)
			{
				int nodeID = getNodeID(x,y);
				if (m_openTiles.ContainsKey(nodeID) == false)
					m_openTiles.Add(nodeID, m_maxFadingTime);
				else
					m_openTiles[nodeID] = m_maxFadingTime;
			}
		}

	}

	public void SetPixel(Vector3 pos, TiledMap.Type value)
	{
		Point pt = Point.ToPoint(pos);
		SetPixel(pt.x, pt.y, value);
	}

	public Texture2D GetPixelTex(int x, int y, int blockSize)
	{
		Color32 color = m_tiledMap.GetPixelColor(x, y);
		Texture2D tex = new Texture2D(blockSize, blockSize);
		for(int yy = 0; yy < blockSize; ++yy)
		{
			for(int xx = 0; xx < blockSize; ++xx)
			{
				tex.SetPixel(xx, yy, color);
			}
		}

		tex.Apply();
		return tex;
	}

	public TiledMap.Type[,] Tiles
	{
		get {
			return m_tiledMap.Tiles;
		}
	}

	public bool UnableTo(int x, int y)
	{
		return m_tiledMap.UnableTo(x, y);
	}

	public int getNodeID(int x, int y)
	{
		return m_tiledMap.getNodeID(x, y);
	}

	public Point getPoint(int nodeID)
	{
		return m_tiledMap.getPoint(nodeID);
	}
}
