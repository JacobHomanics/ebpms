using System.Collections.Generic;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Containers.Weighted
{
	[System.Serializable]
	public class WeightedPoolEntities
	{
		public List<PoolEntityManager> poolEntityManagers = new List<PoolEntityManager>();

		public float minWeight;
		public float maxWeight;
	}
}