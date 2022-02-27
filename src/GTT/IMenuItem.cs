using Microsoft.Xna.Framework;

namespace GTT
{
	public interface IMenuItem
	{
		void Set(Rectangle bds);
		void Update(GameTime gt);
		void Draw(GameTime gt);
	}
}