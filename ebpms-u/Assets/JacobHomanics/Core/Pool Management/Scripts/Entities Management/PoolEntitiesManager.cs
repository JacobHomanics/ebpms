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
				entities[x].Initialized.AddListener(OnInitialize);
				entities[x].Initialize(this);
			}

			Initialized?.Invoke(this, entities);
		}

		private void OnInitialize(PoolEntityManager entity, List<PoolEntity> instances)
		{
			entity.OnSpawned.AddListener(OnSpawn);
			entity.OnDespawned.AddListener(_OnDespawn);
			entity.Terminated.AddListener(_OnTerminated);
			OnInitialized?.Invoke(this, entity);

		}

		[ContextMenu("Terminate")]
		public void Terminate()
		{
			var entities = container.GetAllEntities;

			for (int x = 0; x < entities.Count; x++)
				entities[x].Terminate();

			Terminated?.Invoke(this, entities);
		}

		private void _OnTerminated(PoolEntityManager entity)
		{
			entity.OnSpawned.RemoveListener(OnSpawn);
			entity.OnDespawned.RemoveListener(_OnDespawn);
			entity.Terminated.RemoveListener(_OnTerminated);
			OnTerminated?.Invoke(this, entity);
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