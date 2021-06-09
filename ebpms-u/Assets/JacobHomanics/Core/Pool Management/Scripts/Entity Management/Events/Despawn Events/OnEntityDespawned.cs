using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class OnEntityDespawned : UnityEvent<PoolEntityManager, PoolEntity>
	{

	}
}