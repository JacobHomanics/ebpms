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
		public Despawned Despawned;
		public Terminated Terminated;
		public OnSpawned OnSpawned;
		public OnDespawned OnDespawned;

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
			entity.Terminated.AddListener(OnTerminated);
		}

		[ContextMenu("Terminate")]
		public void Terminate()
		{
			var entities = container.GetAllEntities;

			for (int x = 0; x < entities.Count; x++)
				entities[x].Terminate();

			Terminated?.Invoke(this, entities);
		}

		private void OnTerminated(PoolEntityManager entity)
		{
			entity.OnSpawned.RemoveListener(OnSpawn);
			entity.OnDespawned.RemoveListener(_OnDespawn);
			entity.Terminated.RemoveListener(OnTerminated);
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

		[ContextMenu("Despawn")]
		public void Despawn()
		{
			var entities = container.GetAllEntities;

			for (int x = 0; x < entities.Count; x++)
			{
				entities[x].DespawnAll();
			}

			Despawned?.Invoke(this, entities);
		}
	}
}