using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace _2048
{
	public class AnimationControl
	{
		private const int MoveInterval = 5;
		private const int MovePercentInc = 15;

		private const int AppearInterval = 10;
		private const int AppearInitialPercent = 70;
		private const int AppearPercentInc = 5;

		private readonly VisualGrid Grid;
		private readonly Dictionary<Tile, Label> TileToVisual;

		public AnimationControl(VisualGrid grid)
		{
			Grid = grid;
			TileToVisual = new Dictionary<Tile, Label>();
		}

		public void RemoveAllTiles()
		{
			foreach (Label visualTile in TileToVisual.Values)
			{
				Grid.Form.Controls.Remove(visualTile);
			}
			TileToVisual.Clear();
		}

		/// <summary>
		///  Moves the given tiles from (PrevRow, PrevCol) to (Row, Col)
		/// </summary>
		/// <param name="tiles"></param>
		public async Task Move(params Tile[] tiles)
		{
			Label[] visualTiles = new Label[tiles.Length];
			for (int i = 0; i < tiles.Length; i++)
			{
				visualTiles[i] = TileToVisual[tiles[i]];
			}

			Point[] starts = new Point[tiles.Length];
			Point[] targets = new Point[tiles.Length];
			for (int i = 0; i < tiles.Length; i++)
			{
				Tile tile = tiles[i];

				starts[i] = GetPos(tile.PrevRow, tile.PrevCol);
				targets[i] = GetPos(tile);
			}

			int percent = 0;
			while (percent < 100)
			{
				await Task.Delay(MoveInterval);

				percent += MovePercentInc;
				if (percent > 100) percent = 100;

				for (int i = 0; i < tiles.Length; i++)
				{
					MoveAnimation(visualTiles[i], starts[i], targets[i], percent);
				}
			}
		}
		private void MoveAnimation(Label visualTile, Point start, Point target, int percent)
		{
			visualTile.Location = new Point
			{
				X = start.X + (target.X - start.X) * percent / 100,
				Y = start.Y + (target.Y - start.Y) * percent / 100
			};
		}

		/// <summary>
		/// Makes the given tile appear at (Row, Col)
		/// </summary>
		/// <param name="tiles"></param>
		public async Task Appear(params Tile[] tiles)
		{
			Label[] visualTiles = new Label[tiles.Length];
			for (int i = 0; i < tiles.Length; i++)
			{
				visualTiles[i] = CreateVisualTile(tiles[i]);
				Grid.Form.Controls.Add(visualTiles[i]);
				visualTiles[i].BringToFront();

				TileToVisual.Add(tiles[i], visualTiles[i]);

			}

			Point[] positions = new Point[tiles.Length];
			for (int i = 0; i < tiles.Length; i++)
			{
				positions[i] = GetPos(tiles[i]);
			}

			int percent = AppearInitialPercent;
			while (percent < 100)
			{
				await Task.Delay(AppearInterval);

				percent += AppearPercentInc;
				if (percent > 100) percent = 100;

				for (int i = 0; i < tiles.Length; i++)
				{
					AppearAnimation(visualTiles[i], positions[i], percent);
				}
			}
		}
		private void AppearAnimation(Label visualTile, Point pos, int percent)
		{
			// change side
			int size = Grid.CellSize * percent / 100;
			visualTile.Size = new Size(size, size);

			// change location
			int offset = (Grid.CellSize - size) / 2;
			pos.X += offset;
			pos.Y += offset;
			visualTile.Location = pos;
		}

		/// <summary>
		/// Makes the given tiles disappear
		/// </summary>
		/// <param name="tiles"></param>
		public void Disappear(params Tile[] tiles)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				Label visualTile = TileToVisual[tiles[i]];
				Grid.Form.Controls.Remove(visualTile);

				TileToVisual.Remove(tiles[i]);
			}

		}

		private Point GetPos(int row, int col)
		{
			return new Point(Grid.X + col * (Grid.CellSize + VisualGrid.TileGap), Grid.Y + row * (Grid.CellSize + VisualGrid.TileGap));
		}

		private Point GetPos(Tile tile)
		{
			return GetPos(tile.Row, tile.Col);
		}

		private Label CreateVisualTile(Tile tile)
		{
			int index = tile.Value < VisualGrid.Colors.Length ? tile.Value : VisualGrid.Colors.Length - 1;

			return new Label
			{
				Size = new Size(0, 0),
				Location = GetPos(tile),
				BackColor = VisualGrid.Colors[index],
				Text = (1 << tile.Value).ToString(),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Arial", VisualGrid.FontSize)
			};
		}
	}
}