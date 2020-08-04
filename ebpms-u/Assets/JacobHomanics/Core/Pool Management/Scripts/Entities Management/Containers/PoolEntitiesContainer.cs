using System.Collections.Generic;
using UnityEngine;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Containers
{
	public class PoolEntitiesContainer : BasePoolEntitiesContainer
	{
		public List<PoolEntityManager> poolEntityManagers = new List<PoolEntityManager>();

		public override List<PoolEntityManager> GetAllEntities { get { return poolEntityManagers; } }

		public override PoolEntityManager GetRandomEntity
		{
			get
			{
				var rn = Random.Range(0, poolEntityManagers.Count);
				return poolEntityManagers[rn];
			}
		}
	}
}