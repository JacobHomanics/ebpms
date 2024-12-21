namespace JacobHomanics.Core.PoolManagement.PoolEvents
{
	[System.Serializable]
	public class InitializationEvents
	{
		public Initialized Initialized = new Initialized();
		public PoolEvent OnEntityInitialized = new();
	}
}