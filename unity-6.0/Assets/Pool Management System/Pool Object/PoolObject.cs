using UnityEngine;
using JacobHomanics.Core.PoolManagement.PoolObjectEvents;

namespace JacobHomanics.Core.PoolManagement
{
	public class PoolObject : MonoBehaviour
	{
		public bool destroyOnTerminate;

		public Events events = new();

		public Pool Pool { get; private set; }

		public void Initialize(Pool pool)
		{
			Pool = pool;
			events.OnInitialize?.Invoke(this, Pool);
		}

		[ContextMenu("Spawn")]
		public void SpawnSelf()
		{
			Pool.Spawn(this);
		}

		public void Spawn()
		{
			events.OnSpawn?.Invoke(this);
		}

		[ContextMenu("Despawn")]
		public void Despawn()
		{
			events.OnDespawn?.Invoke(this);
		}

		[ContextMenu("Terminate")]
		public void Terminate()
		{
			events.OnTerminate?.Invoke(this);
			if (destroyOnTerminate)
				Destroy(this.gameObject);
		}
	}
}