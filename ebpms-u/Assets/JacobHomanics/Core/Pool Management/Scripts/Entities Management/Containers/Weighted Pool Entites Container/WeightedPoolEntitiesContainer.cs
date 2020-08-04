using System.Collections.Generic;
using JacobHomanics.Core.PoolManagement.EntitiesManagement.Containers.Weighted;
using UnityEngine;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Containers
{
	public class WeightedPoolEntitiesContainer : BasePoolEntitiesContainer
	{
		public List<WeightedPoolEntities> weightedPoolEntities = new List<WeightedPoolEntities>();

		public override List<PoolEntityManager> GetAllEntities
		{
			get
			{
				List<PoolEntityManager> entities = new List<PoolEntityManager>();

					for (int x = 0; x < weightedPoolEntities.Count; x++)
					{
							for (int y = 0; y < weightedPoolEntities[x].poolEntities.Count; y++)
								entities.Add(weightedPoolEntities[x].poolEntities[y]);
					}

					return entities;
			}
		}

		public override PoolEntityManager GetRandomEntity
		{
			get
			{
				List<PoolEntityManager> entities = new List<PoolEntityManager>();

				var randomWeight = Random.Range(0f, 1f);
				for (int x = 0; x < weightedPoolEntities.Count; x++)
				{
					if (randomWeight >= weightedPoolEntities[x].minWeight && randomWeight < weightedPoolEntities[x].maxWeight)
					{
						for (int y = 0; y < weightedPoolEntities[x].poolEntities.Count; y++)
							entities.Add(weightedPoolEntities[x].poolEntities[y]);
					}
				}

				var rn = Random.Range(0, entities.Count);
				return entities[rn];
			}
		}
	}
}