using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ant : SpawnBaseObj {

	protected AntNavigator	m_navigator;

	[SerializeField]
	int			m_hunger = 10;
	[SerializeField]
	int			m_maxHunger = 10;

	[SerializeField]
	int			m_age = 0;
	[SerializeField]
	int			m_maxAge = 10;

	Animator	m_animator;

	HashSet<string>	m_buff = new HashSet<string>();

	AICommand m_cmd = null;

	float		m_alphaSpeed = 0f;

	protected CarryHolder	m_carryHolder = new CarryHolder();

	public void Awake()
	{
		m_navigator = GetComponent<AntNavigator>();
		m_hunger = m_maxHunger;
		m_carryHolder.Init(this, transform.Find("CarryHolder").GetComponent<SpriteRenderer>(), 1, 1);
		HP = MaxHP;
		m_animator = GetComponent<Animator>();

	}

	override public void StartBuilding()
	{
		Point st = Point.ToPoint(transform.position);
		m_navigator.GoTo(transform.position, m_navigator.Digy);

		StartCoroutine(LoopHenger());
		if (0 < m_maxAge)
			StartCoroutine(LoopAge());
	}

	IEnumerator LoopHenger()
	{
		while (m_hunger > 0)
		{
			yield return new WaitForSeconds(1f);
			--m_hunger;
		}
	}

	IEnumerator LoopAge()
	{
		while (m_age < m_maxAge)
		{
			yield return new WaitForSeconds(60f);
			++m_age;
		}

		Helper.GetColony(Colony).AntSpawningPool.Kill(this);
	}

	public void Update()
	{
		if (m_hunger <= 0)
			Helper.GetColony(Colony).AntSpawningPool.Kill(this);
	}


	virtual public void DoDefaultAI()
	{

		if (Random.Range(0, 2) == 0)			
		{
			m_navigator.GoTo(Helper.GetColony(Colony).GetRandomGround(), false);
		}
		else
		{
			SpawnBaseObj target = Helper.GetColony(Colony).SelectRandomRoom(m_navigator.Digy);
			m_navigator.GoTo(target, m_navigator.Digy);
		}

	}

	public void ContinueNextAICommand()
	{

		SpawnBaseObj target = null;

		if (m_hunger < 60)
		{
			Room room = Helper.GetColony(Colony).SelectRandomRoom(SpawnObjType.RoomFood, false);
			if (room != null)
				m_cmd = new AICommand(AICommandType.EAT_FOOD, room.UID );
			else
				m_cmd = null;
		}
		else
		{
			m_cmd = Helper.GetColony(Colony).AICommandQueue(Type).PopCommand();
		}

		if (m_cmd != null)
		{
			bool digy = m_navigator.Digy;
			switch(m_cmd.CommandType)
			{
			case AICommandType.GEN_EGG:
				target = Helper.GetColony(Colony).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_FOOD:
				target = Helper.GetColony(Colony).FoodSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.EAT_FOOD:
				target = Helper.GetColony(Colony).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_NATURAL_ENEMY:
				target = Helper.GetColony(Colony).NaturalEnemySpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_ROOM:
				target = Helper.GetColony(Colony).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = true;
				break;
			case AICommandType.ATTACK:
				target = Helper.GetColony(1).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;

				break;
			}

			m_navigator.GoTo(target, digy);
		}
		else
		{
			DoDefaultAI();
		}

	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		if (target != null && target.GetComponent<Room>() != null)
			target.GetComponent<Room>().HasPath = true;

		if (target != null && m_cmd != null)
		{
			SpawnObjType type = target.Type;
			
			switch(type)
			{
			case SpawnObjType.RoomFood:
				RoomFood roomFood = target.GetComponent<RoomFood>();
				if (m_cmd.CommandType == AICommandType.EAT_FOOD)
				{
					if (roomFood.CarryHolder.CarryCount > 0)
					{
						FoodPeace foodPeace = (FoodPeace)roomFood.CarryHolder.Takeout();
						m_hunger = Mathf.Min(m_hunger+m_maxHunger, m_maxHunger);
						StartCoroutine(LoopForBuffSpeed(AICommandType.EAT_FOOD.ToString(), foodPeace.Buff));
					}
				}
				break;
			
			}

		}

	}

	public IEnumerator LoopForBuffSpeed(string name, Buff buff)
	{
		if (false == m_buff.Contains(name))
		{
			m_buff.Add(name);
			SpriteRenderer ren = GetComponentInChildren<SpriteRenderer>();
			Color backup = ren.color;
			ren.color = buff.color;
			m_navigator.AlphaSpeed += buff.value;
			yield return new WaitForSeconds(buff.duration);

			if (m_navigator != null)
			{
				m_navigator.AlphaSpeed -= buff.value;
				ren.color = backup;
				m_buff.Remove(name);
			}
		}
	}

	override public void OnKill()
	{
		m_carryHolder.Clear();
	}

	public void Attack(Ant victim)
	{
		victim.HP--;
		if (victim.HP <= 0)
		{
			if (victim.Type == SpawnObjType.AntQueen)
			{
				Helper.GetColony(victim.Colony).RoomSpawningPool.MoveToColony(Helper.GetColony(Colony).RoomSpawningPool);
				Helper.GetColony(Colony).AntSpawningPool.MaxSpawnCount += Helper.GetColony(victim.Colony).AntSpawningPool.MaxSpawnCount;
				Helper.GetColony(victim.Colony).AntSpawningPool.Clear();
				Helper.GetColony(victim.Colony).FoodSpawningPool.Clear();
			}
			else
			{
				Helper.GetColony(victim.Colony).AntSpawningPool.Kill(victim);
			}

		}
		m_navigator.Stop();
		m_animator.SetTrigger("Attack");
	}

	public void Work()
	{
		m_navigator.Stop();
		m_animator.SetTrigger("Work");
	}

	public void OnAttackAniFinish()
	{
		m_navigator.RestartGo();
	}

	public CarryHolder CarryHolder
	{
		get { return m_carryHolder; }
	}

	void OnTriggerEnter2D(Collider2D other) {
		Ant ant = other.gameObject.GetComponent<Ant>();
		if (ant != null && ant.Colony != Colony)
		{
			Attack (ant);
		}
	}
}
