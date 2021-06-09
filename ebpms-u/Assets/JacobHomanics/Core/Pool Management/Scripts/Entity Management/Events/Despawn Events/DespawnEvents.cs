namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class DespawnEvents
	{
		public DespawnedAll DespawnedAll = new DespawnedAll();
		public OnEntityDespawned OnEntityDespawned = new OnEntityDespawned();
	}
}