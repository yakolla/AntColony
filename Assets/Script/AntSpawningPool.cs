using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntSpawningPool : SpawningPool<Ant> {

	[SerializeField]
	int[]		m_ratioHatching;

	int			m_maxRatio;

	void Start()
	{
		for(int i = 0 ; i < m_ratioHatching.Length; ++i)
		{
			int ratio = m_ratioHatching[i];
			m_ratioHatching[i] += m_maxRatio;
			m_maxRatio += ratio;
		}
	}

	override public void OnClickSpawn(int index)
	{
		StartBuilding(Spawn(index));
	}

	public void OnClickAttack()
	{
		if (Helper.GetNaturalEnemySpawningPool().SpawnKeys[Helper.SpawnObjType.NaturalEnemy].Count == 0)
			return;

		string uid = Helper.GetNaturalEnemySpawningPool().SpawnKeys[Helper.SpawnObjType.NaturalEnemy][0];
		AICommand cmd = new AICommand(AICommandType.GEN_NATURAL_ENEMY, uid);
		Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.NaturalEnemy).PushCommand(cmd);
	}

	public int RandomPrefIndexToHatch()
	{
		int random = Random.Range(1, m_maxRatio);
		for(int i = 0 ; i < m_ratioHatching.Length; ++i)
		{
			if (random < m_ratioHatching[i])
			{
				return i;
			}
		}

		return 0;
	}
}
