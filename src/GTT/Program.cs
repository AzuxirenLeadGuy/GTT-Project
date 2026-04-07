using System;

using Azuxiren.MG.Components;

using Microsoft.Xna.Framework;

namespace GTT
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using var game = new AzuxirenMonogameClass<CommonDataStruct>(
				new Point(1024, 768),
				new StageFactory()
			);
			game.Run();
		}
	}
}