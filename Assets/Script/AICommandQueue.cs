using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AICommandQueue {

	SortedList<int, AICommand>	m_commands = new SortedList<int, AICommand>(new Helper.DuplicateKeyComparer<int>());

	public void PushCommand(AICommand cmd)
	{
		m_commands.Add(cmd.Priority, cmd);
	}

	public AICommand PopCommand()
	{
		if (0 == m_commands.Count)
			return null;

		AICommand cmd = m_commands.Values[0];
		if (cmd.Target != null)
		{
			if (0 == cmd.Target.HP)
				m_commands.RemoveAt(0);
		}
		else
		{
			m_commands.RemoveAt(0);
		}

		return cmd;
	}
}
