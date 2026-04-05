using Azuxiren.MG.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GTT;

public class DemoStage : IGameStage<CommonDataStruct>
{
	protected Texture2D _patch;
	protected Vector2 _location;
	protected Vector2 _velocity;
	protected Vector2 _center;
	protected float _zoom;
	protected Rectangle _region;

	public DemoStage(in Game game, CommonDataStruct settings)
	{
		settings.Patch = new Texture2D(game.GraphicsDevice, 1, 1);
		settings.Patch.SetData([Color.White]);
		_patch = settings.Patch;
		_zoom = 20;
		_location = new(_zoom, _zoom);
		_velocity = new(1, 1);
		_center = new(.5f, .5f);
		_region = new(0, 0, settings.GameWidth, settings.GameHeight);
	}

	public void Draw(GameTime gt, in RenderTargetDrawer drawer, in CommonDataStruct settings)
	{
        var dest = drawer.Bounds;
        var pos = drawer.DestinationRect;
        var rect = new Point(settings.GameWidth, settings.GameHeight);
		drawer.DrawToTarget(
			(batch) => batch.Draw(
				_patch,
				_location,
				scale: new(_zoom),
				origin: _center
			)
		);
	}

	public GameUpdateResult Update(GameTime gt, ref CommonDataStruct settings)
	{
        if(Keyboard.GetState().IsKeyDown(Keys.Escape)) return GameUpdateResult.ExitRequest;
		_location += _velocity;
		if (_region.Contains(_location)) 
        { 
            return GameUpdateResult.NoAction; 
        }
        if(_location.X < _region.Left || _location.X > _region.Right)
        {
            _velocity.X = -_velocity.X;
			_location.X = _location.X < _region.Left ? _region.Left : _region.Right;
		}
        if(_location.Y < _region.Top || _location.Y > _region.Bottom)
        {
            _velocity.Y = -_velocity.Y;
			_location.Y = _location.Y < _region.Top ? _region.Top : _region.Bottom;
		}
        return GameUpdateResult.NoAction;
	}
}