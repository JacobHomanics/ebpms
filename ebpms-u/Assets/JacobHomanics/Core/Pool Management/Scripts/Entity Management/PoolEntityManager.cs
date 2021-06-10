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

		public Events Events = new Events();

		public Initialized Initialized { get { return Events.InitializationEvents.Initialized; } set { Events.InitializationEvents.Initialized = value; } }
		public OnEntityInitialized OnEntityInitialized { get { return Events.InitializationEvents.OnEntityInitialized; } set { Events.InitializationEvents.OnEntityInitialized = value; } }

		public Spawning Spawning { get { return Events.SpawnEvents.Spawning; } set { Events.SpawnEvents.Spawning = value; } }		
		public OnEntitySpawned OnEntitySpawned { get { return Events.SpawnEvents.OnEntitySpawned; } set { Events.SpawnEvents.OnEntitySpawned = value; } }

		public DespawnedAll DespawnedAll { get { return Events.DespawnEvents.DespawnedAll; } set { Events.DespawnEvents.DespawnedAll = value; } }
		public OnEntityDespawned OnEntityDespawned{ get { return Events.DespawnEvents.OnEntityDespawned; } set { Events.DespawnEvents.OnEntityDespawned = value; } }

		public Terminated Terminated { get { return Events.TerminationEvents.Terminated; } set { Events.TerminationEvents.Terminated = value; } }
		public OnEntityTerminated OnEntityTerminated { get { return Events.TerminationEvents.OnEntityTerminated; } set { Events.TerminationEvents.OnEntityTerminated = value; } }

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

			Initialized?.Invoke(this, ReadyInstances);
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

			OnEntityInitialized?.Invoke(this, instance);
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

			Terminated?.Invoke(this);
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

			DespawnedAll?.Invoke(this, despawned);
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
			Spawning?.Invoke(this, instance);

			instance.Spawn();
		}

		private void OnSpawn(PoolEntity instance)
		{
			OnEntitySpawned?.Invoke(this, instance);
			instance.Spawned.RemoveListener(OnSpawn);
		}

		private void _OnDespawn(PoolEntity instance)
		{
			ActiveInstances.Remove(instance);
			ReadyInstances.Add(instance);

			instance.Despawned.RemoveListener(_OnDespawn);

			OnEntityDespawned?.Invoke(this, instance);
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

			OnEntityTerminated?.Invoke(this, instance);
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
