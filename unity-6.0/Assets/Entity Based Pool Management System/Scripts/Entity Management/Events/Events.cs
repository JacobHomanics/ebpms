namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class Events
	{
		public InitializationEvents InitializationEvents = new InitializationEvents();
		public SpawnEvents SpawnEvents = new SpawnEvents();
		public DespawnEvents DespawnEvents = new DespawnEvents();
		public TerminationEvents TerminationEvents = new TerminationEvents();
	}
}