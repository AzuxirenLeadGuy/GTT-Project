using System;

namespace GTT.GL
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using var game = new GameApp(new MouseInputManager());
			game.Run();
		}
	}
}