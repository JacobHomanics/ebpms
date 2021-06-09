namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class DespawningEvents
	{
		public DespawnedAll DespawnedAll = new DespawnedAll();
		public OnEntityDespawned OnEntityDespawned = new OnEntityDespawned();
	}
}