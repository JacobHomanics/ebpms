using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class OnEntitySpawned : UnityEvent<PoolEntityManager, PoolEntity> { }
}