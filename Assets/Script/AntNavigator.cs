using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntNavigator  : MonoBehaviour{

	[SerializeField]
	Vector3 m_start;

	[SerializeField]
	Vector3 m_goal;
	Vector3	m_smallGoal;

	[SerializeField]
	float	m_speed = 1f;

	[SerializeField]
	bool	m_digy = true;

	SpawnBaseObj	m_target;
	System.Action m_callbackReachToGoal;

	HashSet<int> m_openNode = new HashSet<int>();
	HashSet<int> m_closeNode = new HashSet<int>();



	SortedList<int, int> m_shortestNode = new SortedList<int, int>(new Helper.DuplicateKeyComparer<int>());

	Background m_background;
	// Use this for initialization
	public void Start () {
		transform.position = m_start;
		m_background = Helper.GetBackground();

		m_smallGoal = transform.position;
		m_background.SetPixel(transform.position, Helper.OPEN_TILE);
	}

	public void GoTo(Vector3 goal, bool digy)
	{
		m_goal = goal;
		m_digy = digy;
		m_target = null;
	}

	public void GoTo(SpawnBaseObj target, bool digy)
	{
		GoTo(target.transform.position, digy);
		m_target = target;

	}

	void OnReachToGoal()
	{
		m_closeNode.Clear();
		m_shortestNode.Clear();
		m_openNode.Clear();
		SpawnBaseObj backupTarget = m_target;
		m_target = null;
		GetComponent<SpawnBaseObj>().OnReachToGoal(backupTarget);
	}
	// Update is called once per frame
	public void Update () 
	{
		if (0 == Point.Distance(m_smallGoal, transform.position))
		{
			if (0 == Point.Distance(m_goal, transform.position))
			{
				OnReachToGoal();
			}
			else
			{
				searchShortest(Point.ToPoint(transform.position), Point.ToPoint(m_goal));
				if (0 < m_shortestNode.Count)
				{
					int nodeID = m_shortestNode.Values[0];

					Point next = m_background.getPoint(nodeID);

					m_smallGoal = Point.ToVector(next);
					m_closeNode.Add(nodeID);
					m_openNode.Remove(nodeID);
					m_shortestNode.RemoveAt(0);
				}
				else
				{
					OnReachToGoal();
				}
			}
		}

		transform.position = Vector3.MoveTowards(transform.position, m_smallGoal, m_speed*Time.deltaTime);
		m_background.SetPixel(transform.position, Helper.OPEN_TILE);
	}



	void searchShortest(Point cpt, Point gpt)
	{
		int[] ax = {-1, 0, 1, 0};
		int[] ay = {0, -1, 0, 1};
		for(int i = 0; i < 4; ++i)
		{
			int yy = cpt.y+ay[i];
			int xx = cpt.x+ax[i];
			int nodeID = m_background.getNodeID(xx, yy);
			if (m_background.UnableTo(xx, yy))
				continue;
			if (m_closeNode.Contains(nodeID))
				continue;
			if (m_background.Tiles[yy, xx] == Helper.ROOM_TILE)
				continue;
			if (m_background.Tiles[yy, xx] != Helper.OPEN_TILE && m_digy == false)
				continue;

			int dy = gpt.y-yy;
			int dx = gpt.x-xx;
			
			int distance = dy*dy + dx*dx;

			if (m_background.Tiles[yy, xx] == Helper.CLOSE_TILE)
			{
				distance += (int)(distance*0.2F);
			}
			else if (m_background.Tiles[yy, xx] == Helper.BLOCK_TILE)
			{
				distance += (int)(distance*0.5F);
			}

			m_openNode.Add(nodeID);
			m_shortestNode.Add(distance, nodeID);

		}

	}


}
