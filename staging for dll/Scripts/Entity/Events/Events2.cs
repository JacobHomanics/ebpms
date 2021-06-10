namespace JacobHomanics.Core.PoolManagement.Entity.Events
{
	[System.Serializable]
	public class Events
	{
		public Initialized Initialized = new Initialized();
		public Spawned Spawned = new Spawned();
		public Despawned Despawned = new Despawned();
		public Terminated Terminated = new Terminated();
	}
}
