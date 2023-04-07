using Stacks;

namespace Resources
{
	public struct TransferTicket
	{
		public Stack SourceStack;
		public Stack DestinationStack;

		public TransferTicket(Stack SourceStack, Stack DestinationStack)
		{
			this.SourceStack = SourceStack;
			this.DestinationStack = DestinationStack;
		}
	}
}