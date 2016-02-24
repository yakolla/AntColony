using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpawnObjType
{
	AntQueen,
	AntWorker,
	AntSoldier,
	NaturalEnemy,
	Food,
	RoomQueen,
	RoomFood,
	RoomEgg,
	RoomAntWorker,
	RoomAntSoldier,
	Egg,
	Count
}

public class Helper 
{

	public const int ONE_PEACE_SIZE = 16;
	public const int CAPACITY_ROOM = 5;


	public const int MY_COLONY=0;
	public const int NATURAL_ENEMY_COLONY=10;

	public class DuplicateKeyComparer<TKey>
		:
			IComparer<TKey> where TKey : System.IComparable
	{
		#region IComparer<TKey> Members
		
		public int Compare(TKey x, TKey y)
		{
			int result = x.CompareTo(y);
			
			if (result == 0)
				return 1;   // Handle equality as beeing greater
			else
				return result;
		}
		
		#endregion
	}

	static Background	_background = null;
	public static Background GetBackground()
	{
		if (_background == null)
			_background = GameObject.Find("Background").GetComponent<Background>();

		return _background;

	}

	static Dictionary<int, Colony>	_colnoys = new Dictionary<int, Colony>();
	public static Colony	GetColony(int colony)
	{
		if (_colnoys.ContainsKey(colony) == false)
			_colnoys.Add(colony, GameObject.Find("Colonys/Colony"+colony).GetComponent<Colony>());

		return _colnoys[colony];
	}

	public static void CheckTouched(System.Action<int, Vector3[]> callback)
	{
		int touchedCount = 0;
		Vector3[] touchPos = new Vector3[5];
#if UNITY_EDITOR
		touchedCount = Input.GetMouseButtonUp(0) == true ? 1 : 0;
		if (touchedCount > 0)
			touchPos[0] = Input.mousePosition;
		
#else
		touchedCount = Input.touchCount;
		if (touchedCount > 0)
		{
			int aa = 0;
			for(int i = 0; i < touchedCount; ++i)
			{
				if (Input.GetTouch (i).phase == TouchPhase.Began)
				{
					touchPos[aa] = Input.GetTouch(i).position;
					++aa;
				}
			}
			
			touchedCount = aa;
		}
#endif
		
		callback(touchedCount, touchPos);
	}

	public static void CheckTouchDraging(System.Action<int, Vector3, TouchPhase> callback)
	{
		TouchPhase phase = TouchPhase.Began;
		int touchedCount = 0;
		Vector3 touchPos = Vector3.zero;
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0) == true)
		{
			phase = TouchPhase.Began;
			touchedCount = 1;
		}
		else if (Input.GetMouseButton(0) == true)
		{
			phase = TouchPhase.Moved;
			touchedCount = 1;
		}
		else if (Input.GetMouseButtonUp(0) == true)
		{
			phase = TouchPhase.Ended;
			touchedCount = 1;
		}

		if (touchedCount > 0)
			touchPos = Input.mousePosition;

		#else
		touchedCount = Input.touchCount;
		if (touchedCount > 0)
		{
			phase = Input.GetTouch (0).phase;
			touchPos = Input.GetTouch(0).position;
		}
		#endif

		callback(touchedCount, touchPos, phase);
	}

}
