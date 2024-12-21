// using System.Collections.Generic;
// using UnityEngine;
// using JacobHomanics.Core.PoolManagement.PoolEvents;

// namespace JacobHomanics.Core.PoolManagement
// {
// 	public class Pool : MonoBehaviour
// 	{
// 		[Header("Properties")]
// 		public PoolObject poolObject;
// 		public int initialInstanceCount = 1;

// 		public enum OverflowType { Expandable, Recycable }
// 		public OverflowType overflowType;


// 		private List<PoolObject> _readyInstance = new List<PoolObject>();
// 		public List<PoolObject> ReadyInstances
// 		{
// 			get { return _readyInstance; }
// 			private set { _readyInstance = value; }
// 		}

// 		private List<PoolObject> _activeInstances = new List<PoolObject>();
// 		public List<PoolObject> ActiveInstances
// 		{
// 			get { return _activeInstances; }
// 			private set { _activeInstances = value; }
// 		}

// 		public List<PoolObject> AllInstances
// 		{
// 			get
// 			{
// 				List<PoolObject> total = new List<PoolObject>();
// 				for (int x = 0; x < ReadyInstances.Count; x++)
// 					total.Add(ReadyInstances[x]);

// 				for (int x = 0; x < ActiveInstances.Count; x++)
// 					total.Add(ActiveInstances[x]);

// 				return total;
// 			}
// 		}

// 		// public PoolEntitiesManager PoolEntitiesManager { get; private set; }

// 		public Events events = new Events();

// 		public void Initialize(PoolEntitiesManager poolEntityManager)
// 		{
// 			this.PoolEntitiesManager = poolEntityManager;
// 			Initialize();
// 		}

// 		[ContextMenu("Initialize")]
// 		public void Initialize()
// 		{
// 			for (int x = 0; x < initialInstanceCount; x++)
// 			{
// 				var instance = CreateAndInitializeInstance();
// 			}

// 			Initialized?.Invoke(this, ReadyInstances);
// 		}

// 		private void Initialize(PoolObject instance)
// 		{
// 			instance.Initialized.AddListener(OnInitialized);
// 			instance.Initialize(this);
// 		}

// 		private void OnInitialized(PoolObject instance)
// 		{
// 			instance.Initialized.RemoveListener(OnInitialized);
// 			instance.Terminated.AddListener(_OnTerminated);

// 			ReadyInstances.Add(instance);

// 			OnEntityInitialized?.Invoke(this, instance);
// 		}

// 		private PoolObject CreateAndInitializeInstance()
// 		{
// 			var instance = Instantiate(poolEntity);
// 			Initialize(instance);
// 			return instance;
// 		}

// 		[ContextMenu("Terminate")]
// 		public void Terminate()
// 		{
// 			var aiCount = ActiveInstances.Count;
// 			for (int x = 0; x < aiCount; x++)
// 				ActiveInstances[0].Terminate();

// 			var riCount = ReadyInstances.Count;
// 			for (int x = 0; x < riCount; x++)
// 				ReadyInstances[0].Terminate();

// 			events.OnTerminate?.Invoke(this);
// 		}



// 		[ContextMenu("Despawn All")]
// 		public void DespawnAll()
// 		{
// 			var despawned = new List<PoolObject>();
// 			while (ActiveInstances.Count > 0)
// 			{
// 				despawned.Add(ActiveInstances[0]);
// 				ActiveInstances[0].Despawn();
// 			}

// 			events.OnDespawn?.Invoke(this, despawned);
// 		}

// 		public void _HandlePreSpawn(PoolObject instance)
// 		{

// 		}

// 		[ContextMenu("Spawn")]
// 		public void Spawn()
// 		{
// 			var instance = GetInstance();

// 			ReadyInstances.Remove(instance);
// 			ActiveInstances.Add(instance);

// 			instance.events.OnSpawn.AddListener(OnDespawn);
// 			instance.events.OnSpawn.AddListener(OnSpawn);
// 			Spawning?.Invoke(this, instance);

// 			instance.Spawn();
// 		}

// 		public PoolObject LastSpawned { get; private set; }
// 		public PoolObject LastDespawned { get; private set; }

// 		private void OnSpawn(PoolObject instance)
// 		{
// 			instance.events.OnSpawn.RemoveListener(OnSpawn);
// 			LastSpawned = instance;

// 			OnEntitySpawned?.Invoke(this, instance);
// 		}

// 		private void OnDespawn(PoolObject instance)
// 		{
// 			ActiveInstances.Remove(instance);
// 			ReadyInstances.Add(instance);

// 			instance.events.OnDespawn.RemoveListener(OnDespawn);

// 			LastDespawned = instance;

// 			OnEntityDespawned?.Invoke(this, instance);
// 		}

// 		private PoolObject GetInstance()
// 		{
// 			PoolObject selectedInstance = null;

// 			if (ReadyInstances.Count <= 0 && ActiveInstances.Count <= 0)
// 			{
// 				// Initialize();
// 			}

// 			if (ReadyInstances.Count > 0)
// 			{
// 				selectedInstance = ReadyInstances[0];
// 			}
// 			else
// 			{
// 				if (overflowType == OverflowType.Expandable)
// 				{
// 					// selectedInstance = CreateAndInitializeInstance();
// 				}
// 				else if (overflowType == OverflowType.Recycable)
// 				{
// 					ActiveInstances[0].Despawn();
// 					selectedInstance = ReadyInstances[0];
// 				}
// 			}

// 			return selectedInstance;
// 		}

// 		private void _OnTerminated(PoolObject instance)
// 		{
// 			instance.events.OnSpawn.RemoveListener(OnSpawn);
// 			instance.events.OnDespawn.RemoveListener(OnDespawn);
// 			instance.events.OnTerminate.RemoveListener(_OnTerminated);

// 			if (ReadyInstances.Contains(instance))
// 				ReadyInstances.Remove(instance);
// 			else if (ActiveInstances.Contains(instance))
// 				ActiveInstances.Remove(instance);

// 			OnEntityTerminated?.Invoke(this, instance);
// 			Destroy(instance.gameObject);
// 		}

// 		public void SetOverflowTypeToExpandable()
// 		{
// 			overflowType = OverflowType.Expandable;
// 		}

// 		public void SetOverflowTypeToRecyclable()
// 		{
// 			overflowType = OverflowType.Recycable;
// 		}
// 	}
// }
