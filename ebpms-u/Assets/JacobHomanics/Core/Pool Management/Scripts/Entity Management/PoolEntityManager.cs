using System.Collections.Generic;
using UnityEngine;
using JacobHomanics.Core.PoolManagement.EntityManagement.Events;

namespace JacobHomanics.Core.PoolManagement
{
	public class PoolEntityManager : MonoBehaviour
	{
		[Header("Properties")]
		public PoolEntity poolEntity;
		public int initialInstanceCount = 1;

		public enum OverflowType { Expandable, Recycable }
		public OverflowType overflowType;


		private List<PoolEntity> _readyInstance = new List<PoolEntity>();
		public List<PoolEntity> ReadyInstances
		{
			get { return _readyInstance; }
			private set { _readyInstance = value; }
		}

		private List<PoolEntity> _activeInstances = new List<PoolEntity>();
		public List<PoolEntity> ActiveInstances
		{
			get { return _activeInstances; }
			private set { _activeInstances = value; }
		}

		public List<PoolEntity> AllInstances
		{
			get
			{
				List<PoolEntity> total = new List<PoolEntity>();
				for (int x = 0; x < ReadyInstances.Count; x++)
					total.Add(ReadyInstances[x]);

				for (int x = 0; x < ActiveInstances.Count; x++)
					total.Add(ActiveInstances[x]);

				return total;
			}
		}

		public PoolEntitiesManager PoolEntitiesManager { get; private set; }

		[Header("Events")]
		public InitializationEvents InitializationEvents = new InitializationEvents();
		public SpawningEvents SpawningEvents = new SpawningEvents();
		public DespawningEvents DespawningEvents = new DespawningEvents();
		public TerminationEvents TerminationEvents = new TerminationEvents();

		public Initialized Initialized { get { return InitializationEvents.Initialized; } set { InitializationEvents.Initialized = value; } }
		public OnEntityInitialized OnEntityInitialized { get { return InitializationEvents.OnEntityInitialized; } set { InitializationEvents.OnEntityInitialized = value; } }

		public Spawning Spawning { get { return SpawningEvents.Spawning; } set { SpawningEvents.Spawning = value; } }		
		public OnEntitySpawned OnEntitySpawned { get { return SpawningEvents.OnEntitySpawned; } set { SpawningEvents.OnEntitySpawned = value; } }

		public DespawnedAll DespawnedAll { get { return DespawningEvents.DespawnedAll; } set { DespawningEvents.DespawnedAll = value; } }
		public OnEntityDespawned OnEntityDespawned{ get { return DespawningEvents.OnEntityDespawned; } set { DespawningEvents.OnEntityDespawned = value; } }

		public Terminated Terminated { get { return TerminationEvents.Terminated; } set { TerminationEvents.Terminated = value; } }
		public OnEntityTerminated OnEntityTerminated { get { return TerminationEvents.OnEntityTerminated; } set { TerminationEvents.Terminated = value; } }

		public void Initialize(PoolEntitiesManager poolEntityManager)
		{
			this.PoolEntitiesManager = poolEntityManager;
			Initialize();
		}

		[ContextMenu("Initialize")]
		public void Initialize()
		{
			for (int x = 0; x < initialInstanceCount; x++)
			{
				var instance = CreateAndInitializeInstance();
			}

			InitializationEvents.Initialized?.Invoke(this, ReadyInstances);
		}

		private void Initialize(PoolEntity instance)
		{
			instance.Initialized.AddListener(OnInitialized);
			instance.Initialize(this);
		}

		private void OnInitialized(PoolEntity instance)
		{
			instance.Initialized.RemoveListener(OnInitialized);
			instance.Terminated.AddListener(_OnTerminated);

			ReadyInstances.Add(instance);

			InitializationEvents.OnEntityInitialized?.Invoke(this, instance);
		}

		private PoolEntity CreateAndInitializeInstance()
		{
			var instance = Instantiate(poolEntity);
			Initialize(instance);
			return instance;
		}

		[ContextMenu("Terminate")]
		public void Terminate()
		{
			var aiCount = ActiveInstances.Count;
			for (int x = 0; x < aiCount; x++)
				ActiveInstances[0].Terminate();

			var riCount = ReadyInstances.Count;
			for (int x = 0; x < riCount; x++)
				ReadyInstances[0].Terminate();

			TerminationEvents.Terminated?.Invoke(this);
		}



		[ContextMenu("Despawn All")]
		public void DespawnAll()
		{
			var despawned = new List<PoolEntity>();
			while (ActiveInstances.Count > 0)
			{
				despawned.Add(ActiveInstances[0]);
				ActiveInstances[0].Despawn();
			}

			DespawningEvents.DespawnedAll?.Invoke(this, despawned);
		}

		public void _HandlePreSpawn(PoolEntity instance)
		{

		}

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			var instance = GetInstance();

			ReadyInstances.Remove(instance);
			ActiveInstances.Add(instance);

			instance.Despawned.AddListener(_OnDespawn);
			instance.Spawned.AddListener(OnSpawn);
			SpawningEvents.Spawning?.Invoke(this, instance);

			instance.Spawn();
		}

		private void OnSpawn(PoolEntity instance)
		{
			SpawningEvents.OnEntitySpawned?.Invoke(this, instance);
			instance.Spawned.RemoveListener(OnSpawn);
		}

		private void _OnDespawn(PoolEntity instance)
		{
			ActiveInstances.Remove(instance);
			ReadyInstances.Add(instance);

			instance.Despawned.RemoveListener(_OnDespawn);

			DespawningEvents.OnEntityDespawned?.Invoke(this, instance);
		}

		private PoolEntity GetInstance()
		{
			PoolEntity selectedInstance = null;

			if (ReadyInstances.Count <= 0 && ActiveInstances.Count <= 0)
			{
				Initialize();
			}

			if (ReadyInstances.Count > 0)
			{
				selectedInstance = ReadyInstances[0];
			}
			else
			{
				if (overflowType == OverflowType.Expandable)
				{
					selectedInstance = CreateAndInitializeInstance();
				}
				else if (overflowType == OverflowType.Recycable)
				{
					ActiveInstances[0].Despawn();
					selectedInstance = ReadyInstances[0];
				}
			}

			return selectedInstance;
		}

		private void _OnTerminated(PoolEntity instance)
		{
			instance.Spawned.RemoveListener(OnSpawn);
			instance.Despawned.RemoveListener(_OnDespawn);
			instance.Terminated.RemoveListener(_OnTerminated);

			if (ReadyInstances.Contains(instance))
				ReadyInstances.Remove(instance);
			else if (ActiveInstances.Contains(instance))
				ActiveInstances.Remove(instance);

			TerminationEvents.OnEntityTerminated?.Invoke(this, instance);
			Destroy(instance.gameObject);
		}

		public void SetOverflowTypeToExpandable()
		{
			overflowType = OverflowType.Expandable;
		}

		public void SetOverflowTypeToRecyclable()
		{
			overflowType = OverflowType.Recycable;
		}
	}
}
