using UnityEngine.Events;
using System.Collections.Generic;

namespace JacobHomanics.Core.PoolManagement.PoolEvents
{
	[System.Serializable]
	public class Initialized : UnityEvent<Pool, List<PoolObject>>
	{

	}
}

