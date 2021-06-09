namespace JacobHomanics.Core.PoolManagement.EntityManagement.Events
{
	[System.Serializable]
	public class TerminationEvents
	{
		public Terminated Terminated = new Terminated();
		public OnEntityTerminated OnEntityTerminated = new OnEntityTerminated();
	}
}
