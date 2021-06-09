namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class SpawningEvents
	{
		public Spawning Spawning = new Spawning();
		public OnEntitySpawned OnEntitySpawned = new OnEntitySpawned();
	}
}