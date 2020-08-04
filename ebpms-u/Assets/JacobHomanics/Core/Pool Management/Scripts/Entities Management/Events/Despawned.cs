using System.Collections.Generic;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Events
{
	[System.Serializable]
	public class Despawned : UnityEvent<PoolEntitiesManager, List<PoolEntityManager>>
	{

	}
}