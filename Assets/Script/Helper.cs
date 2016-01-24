using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Helper 
{
	public const int CLOSE_TILE = 0;
	public const int OPEN_TILE = 1;
	public const int ROOM_TILE = 2;
	public const int FOOD_TILE = 3;
	public const int NATURAL_ENEMY_TILE = 4;
	public const int BLOCK_TILE = 5;
	public const int ONE_PEACE_SIZE = 16;

	public enum SpawnObjType
	{
		QueenAnt,
		WorkerAnt,
		SolderAnt,
		NaturalEnemy,
		Food,
		QueenRoom,
		FoodRoom,
		EggRoom,
		Egg,
		Count
	}

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
			_background = GameObject.Find("GameObjects").GetComponentInChildren<Background>();

		return _background;

	}

	static RoomSpawningPool	_roomSpawningPool = null;
	public static RoomSpawningPool GetRoomSpawningPool()
	{
		if (_roomSpawningPool == null)
			_roomSpawningPool = GameObject.Find("GameObjects").GetComponentInChildren<RoomSpawningPool>();
		
		return _roomSpawningPool;
		
	}

	static NaturalEnemySpawningPool	_naturalEnemySpawningPool = null;
	public static NaturalEnemySpawningPool GetNaturalEnemySpawningPool()
	{
		if (_naturalEnemySpawningPool == null)
			_naturalEnemySpawningPool = GameObject.Find("GameObjects").GetComponentInChildren<NaturalEnemySpawningPool>();
		
		return _naturalEnemySpawningPool;
		
	}

	static AntSpawningPool	_antSpawningPool = null;
	public static AntSpawningPool GetAntSpawningPool()
	{
		if (_antSpawningPool == null)
			_antSpawningPool = GameObject.Find("GameObjects").GetComponentInChildren<AntSpawningPool>();
		
		return _antSpawningPool;
		
	}

	static FoodSpawningPool	_foodSpawningPool = null;
	public static FoodSpawningPool GetFoodSpawningPool()
	{
		if (_foodSpawningPool == null)
			_foodSpawningPool = GameObject.Find("GameObjects").GetComponentInChildren<FoodSpawningPool>();
		
		return _foodSpawningPool;
		
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
