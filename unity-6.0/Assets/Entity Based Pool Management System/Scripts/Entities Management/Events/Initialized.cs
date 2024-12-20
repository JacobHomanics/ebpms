using System.Collections.Generic;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Events
{
	[System.Serializable]
	public class Initialized : UnityEvent<PoolEntitiesManager, List<PoolEntityManager>>
	{

	}
}