using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.PoolObjectEvents
{
	[System.Serializable]
	public class Events
	{
		public UnityEvent<PoolObject, Pool> OnInitialize = new();
		public UnityEvent<PoolObject> OnSpawn = new();
		public UnityEvent<PoolObject> OnDespawn = new();
		public UnityEvent<PoolObject> OnTerminate = new();
	}
}
