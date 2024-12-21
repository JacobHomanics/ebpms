namespace JacobHomanics.Core.PoolManagement.PoolEvents
{
	[System.Serializable]
	public class Events
	{
		public PoolEvent OnInitialize;
		public PoolEvent OnSpawn;
		public PoolEvent OnDespawn;
		public PoolEvent OnTerminate;

		// public InitializationEvents InitializationEvents = new InitializationEvents();
		// public SpawnEvents SpawnEvents = new SpawnEvents();
		// public DespawnEvents DespawnEvents = new DespawnEvents();
		// public TerminationEvents TerminationEvents = new TerminationEvents();
	}
}