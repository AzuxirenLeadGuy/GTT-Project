namespace GraphAlgos
{
	public static partial class GraphAlgos
	{
	}
	internal class DFS_Search_State
	{
		internal byte[] VertexDistance, Pred;
		internal bool[] Visited;
		internal byte Pass;
		internal Stack<byte> DFS_Stack;
		public DFS_Search_State(byte size, byte source)
		{
			VertexDistance = new byte[size];
			Pred = new byte[size];
			Visited = new bool[size];
			Pass = 0;
			DFS_Stack = new();
			DFS_Stack.Push(source);
			for (byte i = 0; i < size; i++) Pred[i] = i;
		}
	}
}