using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Background : MonoBehaviour {

	TiledMap	m_tiledMap = new TiledMap();

	[SerializeField]
	Color	m_color = Color.black;

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
		m_tiledMap.Init(renderer, m_color);



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
					SetPixel(pt.x, pt.y, 0);
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

	public void SetPixel(int x, int y, byte value)
	{
		m_tiledMap.SetPixel(x, y, value);

		if (Helper.OPEN_TILE == value)
		{
			if (m_openTiles.ContainsKey(getNodeID(x,y)) == false)
				m_openTiles.Add(getNodeID(x,y), m_maxFadingTime);
			else
				m_openTiles[getNodeID(x,y)] = m_maxFadingTime;
		}

	}

	public void SetPixel(Vector3 pos, byte value)
	{
		m_tiledMap.SetPixel(pos, value);
	}

	public byte[,] Tiles
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
