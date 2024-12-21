namespace JacobHomanics.Core.PoolManagement.PoolEvents
{
	[System.Serializable]
	public class SpawnEvents
	{
		public Spawning Spawning = new Spawning();
		public PoolEvent OnEntitySpawned = new();
	}
}