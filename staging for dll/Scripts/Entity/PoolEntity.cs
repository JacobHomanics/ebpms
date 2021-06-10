using UnityEngine;
using JacobHomanics.Core.PoolManagement.Entity.Events;

namespace JacobHomanics.Core.PoolManagement
{
	public class PoolEntity : MonoBehaviour
	{
		public Events events = new Events();

		public Initialized Initialized { get { return events.Initialized; } set { events.Initialized = value; } }
		public Spawned Spawned { get { return events.Spawned; } set { events.Spawned = value; } }
		public Despawned Despawned { get { return events.Despawned; } set { events.Despawned = value; } }
		public Terminated Terminated { get { return events.Terminated; } set { events.Terminated = value; } }

		public PoolEntityManager PoolEntityManager { get; private set; }

		public void Initialize(PoolEntityManager poolEntityManager)
		{
			this.PoolEntityManager = poolEntityManager;
			Initialize();
		}

		private void Initialize()
		{
			Initialized?.Invoke(this);
		}

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			Spawned?.Invoke(this);
		}

		[ContextMenu("Despawn")]
		public void Despawn()
		{
			Despawned?.Invoke(this);
		}

		[ContextMenu("Terminate")]
		public void Terminate()
		{
			Terminated?.Invoke(this);
		}
	}
}