using Microsoft.Xna.Framework;

namespace GTT
{
	public interface IInputManager
	{
		Point PointerLocation { get; }
		bool Clicked { get; }
		void Update();
	}
}