namespace JacobHomanics.Core.PoolManagement.PoolEvents
{
	[System.Serializable]
	public class DespawnEvents
	{
		public DespawnedAll DespawnedAll = new DespawnedAll();
		public PoolEvent OnEntityDespawned = new();
	}
}