using System.Collections.Generic;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Events
{
	[System.Serializable]
	public class DespawnedAll : UnityEvent<PoolEntitiesManager, List<PoolEntityManager>>
	{

	}
}