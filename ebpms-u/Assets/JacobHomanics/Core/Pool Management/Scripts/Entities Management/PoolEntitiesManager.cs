using UnityEngine;
using JacobHomanics.Core.PoolManagement.EntitiesManagement.Containers;
using JacobHomanics.Core.PoolManagement.EntitiesManagement.Events;
using System.Collections.Generic;

namespace JacobHomanics.Core.PoolManagement
{
	public class PoolEntitiesManager : MonoBehaviour
	{
		public BasePoolEntitiesContainer container;

		[Header("Events")]
		public Initialized Initialized;
		public DespawnedAll DespawnedAll;
		public Terminated Terminated;
		public OnInitialized OnInitialized;
		public OnSpawned OnSpawned;
		public OnDespawned OnDespawned;
		public OnTerminated OnTerminated;

		[ContextMenu("Initialize")]
		public void Initialize()
		{
			var entities = container.GetAllEntities;

			for (int x = 0; x < entities.Count; x++)
			{
				entities[x].InitializationEvents.Initialized.AddListener(OnInitialize);
				entities[x].Initialize(this);
			}

			Initialized?.Invoke(this, entities);
		}

		private void OnInitialize(PoolEntityManager entityManager, List<PoolEntity> instances)
		{
			entityManager.SpawningEvents.OnEntitySpawned.AddListener(OnSpawn);
			entityManager.DespawningEvents.OnEntityDespawned.AddListener(_OnDespawn);
			entityManager.TerminationEvents.Terminated.AddListener(_OnTerminated);
			OnInitialized?.Invoke(this, entityManager);

		}

		[ContextMenu("Terminate")]
		public void Terminate()
		{
			var entities = container.GetAllEntities;

			for (int x = 0; x < entities.Count; x++)
				entities[x].Terminate();

			Terminated?.Invoke(this, entities);
		}

		private void _OnTerminated(PoolEntityManager entityManager)
		{
			entityManager.SpawningEvents.OnEntitySpawned.RemoveListener(OnSpawn);
			entityManager.DespawningEvents.OnEntityDespawned.RemoveListener(_OnDespawn);
			entityManager.TerminationEvents.Terminated.RemoveListener(_OnTerminated);
			OnTerminated?.Invoke(this, entityManager);
		}

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			container.GetRandomEntity.Spawn();
		}

		private void OnSpawn(PoolEntityManager entity, PoolEntity instance)
		{
			OnSpawned?.Invoke(this, entity, instance);
		}

		private void _OnDespawn(PoolEntityManager entity, PoolEntity instance)
		{
			OnDespawned?.Invoke(this, entity, instance);
		}

		[ContextMenu("Despawn All")]
		public void DespawnAll()
		{
			var entities = container.GetAllEntities;

			for (int x = 0; x < entities.Count; x++)
			{
				entities[x].DespawnAll();
			}

			DespawnedAll?.Invoke(this, entities);
		}
	}
}