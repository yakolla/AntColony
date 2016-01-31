using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Background : MonoBehaviour {

	ModifiedTexture2D	m_modifiedTexture = new ModifiedTexture2D();
	byte[,] m_tiles;

	[SerializeField]
	Color	m_color;

	[SerializeField]
	int		m_maxFadingTile = 10;

	Vector3 m_startedTouchPos = Vector3.zero;
	RectTransform	m_topPannel;

	Dictionary<int, int>	m_openTiles = new Dictionary<int, int>();

	AICommandQueue[]	m_antAICommandQueue = new AICommandQueue[(int)Helper.SpawnObjType.Count];

	Vector3[]		m_randomGround = new Vector3[3];

	// Use this for initialization
	void Awake () {
		Application.runInBackground = true;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		m_topPannel = GameObject.Find("HudGUI/Canvas/TopPanel").GetComponent<RectTransform>();
		Debug.Log(m_topPannel.rect);

		for(int i = 0; i < (int)Helper.SpawnObjType.Count; ++i)
		{
			m_antAICommandQueue[i] = new AICommandQueue();
		}

		SpriteRenderer renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
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

		m_randomGround[0] = Vector3.zero;
		m_randomGround[1] = new Vector3(m_modifiedTexture.Width/2, 0, 0);
		m_randomGround[2] = new Vector3(m_modifiedTexture.Width-1, 0, 0);
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
		m_modifiedTexture.Update();
		LoopFadingTile();
		Helper.CheckTouchDraging((touchedCount, touchPos, touchPhase)=>{

			if (touchPos.y > Screen.height-m_topPannel.rect.height)
				return;

			for (int i = 0; i < touchedCount; ++i) 
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

				break;
				
			}
		});
	}

	public void SetPixel(int x, int y, byte value)
	{
		m_tiles[y, x] = value;

		switch(value)
		{
		case Helper.OPEN_TILE:
			if (m_openTiles.ContainsKey(getNodeID(x,y)) == false)
				m_openTiles.Add(getNodeID(x,y), m_maxFadingTile);
			else
				m_openTiles[getNodeID(x,y)] = m_maxFadingTile;
			m_modifiedTexture.SetPixel(x, y, value, new Color32(0, 0, 0, 1));
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

	public AICommandQueue AICommandQueue(Helper.SpawnObjType type)
	{
		return m_antAICommandQueue[(int)type];
	}

	public Vector3	GetRandomGround()
	{
		return m_randomGround[Random.Range(0, m_randomGround.Length)];
	}
}
