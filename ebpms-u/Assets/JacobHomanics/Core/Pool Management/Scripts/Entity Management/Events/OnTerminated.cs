using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class OnTerminated : UnityEvent<PoolEntityManager, PoolEntity>
	{

	}
}