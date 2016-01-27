using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Background : MonoBehaviour {

	ModifiedTexture2D	m_modifiedTexture = new ModifiedTexture2D();

	[SerializeField]
	Color	m_color;

	Vector3 m_startedTouchPos = Vector3.zero;
	RectTransform	m_topPannel;

	AICommandQueue[]	m_antAICommandQueue = new AICommandQueue[(int)Helper.SpawnObjType.Count];

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
	void Update () {
		m_modifiedTexture.Update();

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
		switch(value)
		{
		case Helper.OPEN_TILE:
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
			return m_modifiedTexture.Tiles;
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
}
