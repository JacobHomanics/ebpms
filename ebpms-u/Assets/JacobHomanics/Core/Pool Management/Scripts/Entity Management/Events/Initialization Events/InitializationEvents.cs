namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class InitializationEvents
	{
		public Initialized Initialized = new Initialized();
		public OnEntityInitialized OnEntityInitialized = new OnEntityInitialized();
	}
}