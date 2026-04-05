using Azuxiren.MG.Components;
using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;

namespace GTT
{
	public struct LoadingScreen : IGameStage<CommonDataStruct>
	{
		public readonly void Draw(
			GameTime gt,
			in RenderTargetDrawer drawer,
			in CommonDataStruct settings
		)
		{ }

		public readonly GameUpdateResult Update(
			GameTime gt,
			ref CommonDataStruct settings
		) => GameUpdateResult.NoAction;
	}
}