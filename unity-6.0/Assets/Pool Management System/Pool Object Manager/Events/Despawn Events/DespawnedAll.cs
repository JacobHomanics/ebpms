using System.Collections.Generic;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.PoolEvents
{
	[System.Serializable]
	public class DespawnedAll : UnityEvent<Pool, List<PoolObject>>
	{

	}
}