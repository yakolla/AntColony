using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntNavigator  : MonoBehaviour{

	[SerializeField]
	Vector3 m_goal;
	Vector3	m_smallGoal;

	[SerializeField]
	float	m_speed = 1f;

	[SerializeField]
	bool	m_oriDigy = false;
	bool	m_digy = true;

	SpawnBaseObj	m_target;
	System.Action m_callbackReachToGoal;

	HashSet<int> m_closeNode = new HashSet<int>();

	bool		m_stop = false;


	List<int>		m_path = new List<int>();
	Background m_background;
	// Use this for initialization
	public void Start () {
		m_background = Helper.GetBackground();

		m_background.SetPixel(transform.position, TiledMap.Type.OPEN_TILE);

	}

	public bool Digy
	{
		get {return m_oriDigy;}
	}


	public void GoTo(Vector3 goal, bool digy)
	{
		m_goal = goal;
		m_smallGoal = transform.position;
		m_digy = digy;
		m_stop = false;

	}

	public void RestartGo()
	{
		m_stop = false;
	}

	public void Stop()
	{
		m_stop = true;
	}

	public void GoTo(SpawnBaseObj target, bool digy)
	{
		if (target == null)
		{
			GoTo(transform.position, false);
			m_target = null;
			return;
		}

		GoTo(target.transform.position, digy);
		m_target = target;

	}

	void OnReachToGoal()
	{
		m_closeNode.Clear();
		m_path.Clear();
		SpawnBaseObj backupTarget = m_target;
		m_target = null;
		GetComponent<SpawnBaseObj>().OnReachToGoal(backupTarget);
	}
	// Update is called once per frame
	public void Update () 
	{
		if (m_stop == true)
			return;

		if (0 == Point.Distance(m_smallGoal, transform.position))
		{
			if (0 == Point.Distance(m_goal, transform.position))
			{			
				OnReachToGoal();
			}
			else
			{
				if (0 < m_path.Count)
				{
					m_smallGoal = Point.ToVector(m_background.getPoint(m_path[0]));
					float radian = Mathf.Atan2(m_smallGoal.y-transform.position.y, m_smallGoal.x-transform.position.x);
					transform.rotation = Quaternion.Euler(0, 0, radian*Mathf.Rad2Deg);
					m_path.RemoveAt(0);
				}
				else
				{
					m_path = searchShortestAStarPath(Point.ToPoint(transform.position), Point.ToPoint(m_goal));
					if (m_path.Count == 1)
					{
						m_closeNode.Clear();
						GoTo(m_target, m_oriDigy);
					}
				}
			}
		}

		transform.position = Vector3.MoveTowards(transform.position, m_smallGoal, m_speed*Time.deltaTime);
		m_background.SetPixel(transform.position, TiledMap.Type.OPEN_TILE);
	}

	struct PathNode
	{
		public int h;
		public int g;
		public int nodeId;
		public int parentNodeId;
		public PathNode(int nodeId, int parentId)
		{
			h = 0;
			g = 0;
			this.nodeId = nodeId;
			this.parentNodeId = parentId;
		}
		public int f
		{
			get {return h+g;}
		}

	}

	List<int> searchShortestAStarPath(Point cpt, Point gpt)
	{

		SortedList<int, int> opneNodes = new SortedList<int, int>(new Helper.DuplicateKeyComparer<int>());

		Dictionary<int, PathNode> pathNodes = new Dictionary<int, PathNode>();

		int[] ax = {-1, 0, 1, 0};
		int[] ay = {0, -1, 0, 1};
		opneNodes.Add(0, m_background.getNodeID(cpt.x, cpt.y));
		pathNodes.Add(m_background.getNodeID(cpt.x, cpt.y), new PathNode(m_background.getNodeID(cpt.x, cpt.y), m_background.getNodeID(cpt.x, cpt.y)));
		int lastNodeID = 0;
		while(0 < opneNodes.Count && pathNodes.Count < 20)
		{
			lastNodeID = opneNodes.Values[0];
			opneNodes.RemoveAt(0);


			if (0 == Point.Distance(m_background.getPoint(lastNodeID), gpt))
				break;

			for(int i = 0; i < 4; ++i)
			{

				int yy = m_background.getPoint(lastNodeID).y+ay[i];
				int xx = m_background.getPoint(lastNodeID).x+ax[i];

				if (m_background.UnableTo(xx, yy))
					continue;
				if (m_background.Tiles[yy, xx] != TiledMap.Type.OPEN_TILE && m_digy == false)
					continue;

				int childNodeID = m_background.getNodeID(xx, yy);

				if (false == pathNodes.ContainsKey(childNodeID))				
					pathNodes.Add(childNodeID, new PathNode(childNodeID, lastNodeID));


				int gdy = gpt.y-yy;
				int gdx = gpt.x-xx;

				PathNode childNode = new PathNode(childNodeID, lastNodeID);
				childNode.g = (gdy*gdy + gdx*gdx);
				childNode.h = pathNodes[lastNodeID].h + 1;

				if (m_background.Tiles[yy, xx] == TiledMap.Type.CLOSE_TILE)
					childNode.g += (int)(childNode.g*0.1F);
				else if (m_background.Tiles[yy, xx] == TiledMap.Type.HILL_TILE)
					childNode.g += (int)(childNode.g*0.5F);

				if (m_closeNode.Contains(childNodeID) && pathNodes[childNodeID].f < childNode.f)
					continue;
				int a = opneNodes.IndexOfValue(childNodeID);
				if (0 <= a && pathNodes[childNodeID].f < childNode.f)
					continue;

				pathNodes[childNodeID] = childNode;

				if (0 <= a)
					opneNodes.RemoveAt(a);

				opneNodes.Add(childNode.f, childNodeID);

			}

			m_closeNode.Add(lastNodeID);
		}
		List<int> path = new List<int>();
		int nID = lastNodeID;
		while(true)
		{
			path.Add(nID);

			if (nID == m_background.getNodeID(cpt.x, cpt.y))
				break;			

			nID = pathNodes[nID].parentNodeId;
		}
		path.Reverse();
		return path;
	}

}
