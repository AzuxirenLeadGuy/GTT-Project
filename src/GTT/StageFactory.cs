using Azuxiren.MG.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT;

public class StageFactory : GameStageFactory<CommonDataStruct>
{
	public override IGameStage<CommonDataStruct>? Create(
		in byte scene_code,
		in AzuxirenMonogameClass<CommonDataStruct> content,
		in CommonDataStruct settings
	) => scene_code switch
	{
		0 => LoadStage(content, settings),
		1 => StartStage(content, settings),
		_ => null,
	};

	public override IGameStage<CommonDataStruct> StartStage(
		in AzuxirenMonogameClass<CommonDataStruct> game,
		CommonDataStruct settings
	) => new WelcomeScreen(settings);

	public override IGameStage<CommonDataStruct> LoadStage(
		in AzuxirenMonogameClass<CommonDataStruct> game,
		CommonDataStruct settings
	) => new LoadingScreen();

	public override void ReloadContent(
		ref IGameStage<CommonDataStruct> stage,
		in AzuxirenMonogameClass<CommonDataStruct> content,
		CommonDataStruct settings
	)
	{ }

	public override CommonDataStruct InitializeSettings(in AzuxirenMonogameClass<CommonDataStruct> game)
	{
		var patch = new Texture2D(game.GraphicsDevice, 1, 1);
		patch.SetData([Color.White]);
		var instance = new CommonDataStruct()
		{
			Patch = patch,
			Font = game.Content.Load<SpriteFont>("font"),
			FormalFont = game.Content.Load<SpriteFont>("ffont"),
			Triangle = game.Content.Load<Texture2D>("triangle"),
			Circle = game.Content.Load<Texture2D>("circle"),
			ComponentPalette = new Color[4],
			GameWidth = game.Window.ClientBounds.Width,
			GameHeight = game.Window.ClientBounds.Height,
			Input = new GL.MouseInputManager(),
			ClearColor = new(0x00, 0x3b, 0x75),
			ComponentTextColor = Color.Black,
			ComponentUnselectedColor = Color.DarkGray,
			ComponentHoverColor = Color.LimeGreen,
			ComponentDisabledColor = Color.DarkRed,
			ComponentPressColor = Color.GreenYellow,
			GDArrowColor = Color.PaleGoldenrod,
			GDBaseColor = Color.DarkGray,
			GDNodeColor = Color.Black,
			GdColors = new Color[6],
		};
		game.TargetClearColor = Color.LightSkyBlue;
		instance.ComponentPalette[0] = instance.ComponentUnselectedColor;
		instance.ComponentPalette[1] = instance.ComponentHoverColor;
		instance.ComponentPalette[2] = instance.ComponentPressColor;
		instance.ComponentPalette[3] = instance.ComponentDisabledColor;
		instance.GdColors[0] = instance.ComponentUnselectedColor;
		instance.GdColors[1] = Color.LightPink;
		instance.GdColors[2] = Color.Cyan;
		instance.GdColors[3] = Color.Orange;
		instance.GdColors[4] = Color.LimeGreen;
		instance.GdColors[5] = Color.DarkRed;
		return instance;
	}

}