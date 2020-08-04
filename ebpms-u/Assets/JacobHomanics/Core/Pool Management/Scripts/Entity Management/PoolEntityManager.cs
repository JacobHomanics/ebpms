using System.Collections.Generic;
using UnityEngine;
using JacobHomanics.Core.PoolManagement.EntityManagement.Events;

namespace JacobHomanics.Core.PoolManagement
{
	public class PoolEntityManager : MonoBehaviour
	{
		[Header("Properties")]
		public PoolEntity poolEntity;
		public int initialInstanceCount =  1;

		public enum OverflowType { Expandable, Recycable }
		public OverflowType overflowType;


		private List<PoolEntity> _readyInstance = new List<PoolEntity>();
		public List<PoolEntity> ReadyInstances
		{
			get { return _readyInstance; } private set { _readyInstance = value; }
		}

		private List<PoolEntity> _activeInstances = new List<PoolEntity>();
		public List<PoolEntity> ActiveInstances
		{
			get { return _activeInstances; } private set { _activeInstances = value; }
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

		[Header("Events")]
		public Initialized Initialized;
		public DespawnedAll DespawnedAll;
		public Terminated Terminated;
		public OnSpawned OnSpawned;
		public OnDespawned OnDespawned;
		public OnTerminated OnTerminated;
		public OnInitialized OnInitialized;

		public PoolEntitiesManager PoolEntitiesManager { get; private set; }

		public PoolEntity LastSpawned { get; private set; }
		public PoolEntity LastDespawned { get; private set; }

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
			instance.Initialized.AddListener(_OnInitialized);
			instance.Initialize(this);

		}

		private void _OnInitialized(PoolEntity instance)
		{
			instance.Initialized.RemoveListener(_OnInitialized);

			instance.Spawned.AddListener(OnSpawn);
			instance.Terminated.AddListener(_OnTerminated);
			ReadyInstances.Add(instance);

			OnInitialized?.Invoke(this, instance);

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

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			var instance = GetInstance();
			instance.Spawn();
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


		private void OnSpawn(PoolEntity instance)
		{
			ReadyInstances.Remove(instance);
			ActiveInstances.Add(instance);
			instance.Spawned.RemoveListener(OnSpawn);
			instance.Despawned.AddListener(_OnDespawn);

			LastSpawned = instance;
			OnSpawned?.Invoke(this, instance);
		}

		private void _OnDespawn(PoolEntity instance)
		{
			ActiveInstances.Remove(instance);
			ReadyInstances.Add(instance);

			instance.Spawned.AddListener(OnSpawn);
			instance.Despawned.RemoveListener(_OnDespawn);

			LastDespawned = instance;
			OnDespawned?.Invoke(this, instance);
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

			OnTerminated?.Invoke(this, instance);
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
