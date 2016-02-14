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

	SortedList<int, int> m_shortestNode = new SortedList<int, int>(new Helper.DuplicateKeyComparer<int>());
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
		m_shortestNode.Clear();
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
					}
					GoTo(m_target, m_oriDigy);
				}
			}
		}

		transform.position = Vector3.MoveTowards(transform.position, m_smallGoal, m_speed*Time.deltaTime);
		m_background.SetPixel(transform.position, TiledMap.Type.OPEN_TILE);
	}





	struct PathNode
	{
		public int nextNode;
		public PathNode(int nextNode)
		{
			this.nextNode = nextNode;
		}
	}

	List<int> searchShortestAStarPath(Point cpt, Point gpt)
	{

		m_shortestNode.Clear();


		Dictionary<int, PathNode> pathNodes = new Dictionary<int, PathNode>();

		int[] ax = {-1, 0, 1, 0};
		int[] ay = {0, -1, 0, 1};
		m_shortestNode.Add(0, m_background.getNodeID(cpt.x, cpt.y));
		int prevNodeID = m_shortestNode.Values[0];
		int lastNodeID = prevNodeID;
		while(0 < m_shortestNode.Count && pathNodes.Count < 10)
		{
			int curNodeID = m_shortestNode.Values[0];
			m_shortestNode.RemoveAt(0);

			if (false == m_closeNode.Contains(curNodeID))
				m_closeNode.Add(curNodeID);

			if (pathNodes.ContainsKey(prevNodeID))
				pathNodes[prevNodeID] = new PathNode(curNodeID);
			else
				pathNodes.Add(prevNodeID, new PathNode(curNodeID));

			prevNodeID = curNodeID;
			lastNodeID = curNodeID;

			if (0 == Point.Distance(m_background.getPoint(curNodeID), gpt))
				break;

			for(int i = 0; i < 4; ++i)
			{
				int yy = m_background.getPoint(curNodeID).y+ay[i];
				int xx = m_background.getPoint(curNodeID).x+ax[i];
				int nodeID = m_background.getNodeID(xx, yy);
				if (m_background.UnableTo(xx, yy))
					continue;
				if (m_closeNode.Contains(nodeID))
					continue;
				if (m_background.Tiles[yy, xx] != TiledMap.Type.OPEN_TILE && m_digy == false)
					continue;
				
				int dy = gpt.y-yy;
				int dx = gpt.x-xx;
				
				int distance = dy*dy + dx*dx;				
				
				if (m_background.Tiles[yy, xx] == TiledMap.Type.CLOSE_TILE)
					distance += (int)(distance*0.2F);
				else if (m_background.Tiles[yy, xx] == TiledMap.Type.HILL_TILE)
					distance += (int)(distance*0.5F);

				int a = m_shortestNode.IndexOfValue(nodeID);
				if (0 <= a)
					m_shortestNode.RemoveAt(a);

				m_shortestNode.Add(distance, nodeID);

			}
		}
		List<int> path = new List<int>();
		int nID = m_background.getNodeID(cpt.x, cpt.y);
		while(true)
		{
			path.Add(nID);

			if (pathNodes.ContainsKey(nID) == false)
				break;

			if (nID == lastNodeID)
				break;			

			nID = pathNodes[nID].nextNode;
		}
		return path;
	}

}
