using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Events
{
	[System.Serializable]
	public class OnSpawned : UnityEvent<PoolEntitiesManager, PoolEntityManager, PoolEntity>
	{

	}
}