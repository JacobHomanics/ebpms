using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class Spawning : UnityEvent<PoolEntityManager, PoolEntity> { }
}
