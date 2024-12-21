namespace JacobHomanics.Core.PoolManagement.PoolEvents
{
	[System.Serializable]
	public class TerminationEvents
	{
		public Terminated Terminated = new Terminated();
		public PoolEvent OnEntityTerminated = new();
	}
}
