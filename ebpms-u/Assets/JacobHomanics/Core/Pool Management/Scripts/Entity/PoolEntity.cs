using UnityEngine;
using JacobHomanics.Core.PoolManagement.Entity.Events;

namespace JacobHomanics.Core.PoolManagement
{
	public class PoolEntity : MonoBehaviour
	{
		public Spawned Spawned;
		public Despawned Despawned;
		public Initialized Initialized;
		public Terminated Terminated;

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