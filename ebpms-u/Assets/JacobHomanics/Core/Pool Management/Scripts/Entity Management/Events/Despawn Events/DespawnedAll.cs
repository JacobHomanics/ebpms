using System.Collections.Generic;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class DespawnedAll : UnityEvent<PoolEntityManager, List<PoolEntity>>
	{

	}
}