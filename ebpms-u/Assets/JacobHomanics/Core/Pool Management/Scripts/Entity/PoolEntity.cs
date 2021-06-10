using UnityEngine;
using JacobHomanics.Core.PoolManagement.Entity.Events;

namespace JacobHomanics.Core.PoolManagement
{
	public class PoolEntity : MonoBehaviour
	{
		public Events Events = new Events();

		public Initialized Initialized { get { return Events.Initialized; } set { Events.Initialized = value; } }
		public Spawned Spawned { get { return Events.Spawned; } set { Events.Spawned = value; } }
		public Despawned Despawned { get { return Events.Despawned; } set { Events.Despawned = value; } }
		public Terminated Terminated { get { return Events.Terminated; } set { Events.Terminated = value; } }

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