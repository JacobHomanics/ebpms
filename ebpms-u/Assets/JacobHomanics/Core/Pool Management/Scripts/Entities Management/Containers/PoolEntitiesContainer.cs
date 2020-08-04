using System.Collections.Generic;
using UnityEngine;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Containers
{
	public class PoolEntitiesContainer : BasePoolEntitiesContainer
	{
		public List<PoolEntityManager> poolEntities = new List<PoolEntityManager>();

		public override List<PoolEntityManager> GetAllEntities { get { return poolEntities; } }

		public override PoolEntityManager GetRandomEntity
		{
			get
			{
				var rn = Random.Range(0, poolEntities.Count);
				return poolEntities[rn];
			}
		}
	}
}