using System.Collections.Generic;
using UnityEngine;

namespace JacobHomanics.Core.PoolManagement.EntitiesManagement.Containers
{
	public abstract class BasePoolEntitiesContainer : MonoBehaviour
	{
		public abstract PoolEntityManager GetRandomEntity { get; }
		public abstract List<PoolEntityManager> GetAllEntities { get; }
	}
}