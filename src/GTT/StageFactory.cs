using Azuxiren.MG.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT;

public class StageFactory : GameStageFactory<CommonDataStruct>
{
	public override IGameStage<CommonDataStruct>? Create(in byte scene_code, in Game content, in CommonDataStruct settings)
	=> scene_code switch
	{
		0 => LoadStage(content, settings),
		1 => StartStage(content, settings),
		_ => null,
	};

	public override IGameStage<CommonDataStruct> StartStage(in Game game, CommonDataStruct settings)
		=> new WelcomeScreen(game, settings);

	public override IGameStage<CommonDataStruct> LoadStage(in Game game, CommonDataStruct settings)
		=> new DemoStage(game, settings);

	public override void ReloadContent(ref IGameStage<CommonDataStruct> stage, in Game content, CommonDataStruct settings)
	{
	}

	public override CommonDataStruct InitializeSettings(in Game game)
	{
		var patch = new Texture2D(game.GraphicsDevice, 1, 1);
		patch.SetData([Color.White]);
		return new CommonDataStruct()
		{
			Patch = patch,
			Font = game.Content.Load<SpriteFont>("font"),
			FormalFont = game.Content.Load<SpriteFont>("ffont"),
			Triangle = game.Content.Load<Texture2D>("triangle"),
			Circle = game.Content.Load<Texture2D>("circle"),
			ComponentPalette =
			[
				Color.Black,
				Color.Yellow,
				Color.Green,
				Color.Gold,
				Color.Red,
				Color.Orange
			],
			ClearColor = Color.Wheat,
			GraphDrawingBackColor = Color.BlueViolet,
			GameWidth = game.Window.ClientBounds.Width,
			GameHeight = game.Window.ClientBounds.Height,
			Input = new GL.MouseInputManager(),
			ComponentTextColor = Color.Yellow,
		};
	}

}