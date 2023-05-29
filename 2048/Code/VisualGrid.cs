using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace _2048
{
	public class VisualGrid
	{
		public static Color[] Colors { get; } = new Color[]
		{
			Color.FromArgb(187, 173, 160), // empty

			Color.FromArgb(238, 228, 218), // 2
			Color.FromArgb(237, 224, 200), // 4
			Color.FromArgb(242, 177, 121), // 8
			Color.FromArgb(245, 149, 99), // 16
			Color.FromArgb(246, 124, 95), // 32
			Color.FromArgb(246, 94, 59), // 64
			Color.FromArgb(237, 207, 114), // 128
			Color.FromArgb(237, 204, 97), // 256
			Color.FromArgb(237, 200, 80), // 512
			Color.FromArgb(237, 197, 63), // 1024
			Color.FromArgb(237, 194, 46), // 2048
			Color.FromArgb(4, 176, 33), // other
		};

		public static int TileGap { get; } = 3;
		public static int FontSize { get; } = 12;

		public Form Form { get; }
		public Grid Grid { get; }
		public int X { get; }
		public int Y { get; }
		public int CellSize { get; }

		private readonly AnimationControl AnimationControl;

		private bool Animating;
		private Dir? LastDir;
		private bool NewGameRequest;

		public VisualGrid(Form form, Grid grid, int x, int y, int cellSize)
		{
			Form = form;
			Grid = grid;
			X = x;
			Y = y;
			CellSize = cellSize;

			AnimationControl = new AnimationControl(this);

			CreatePlaceHolders();
		}

		public async void MakeMove(Dir dir)
		{
			if (Grid.GameOver) return;

			if (Animating)
			{
				LastDir = dir;
				return;
			}

			Grid.MakeMove(dir);
			await AnimateTiles();

			if (NewGameRequest)
			{
				NewGame();
			}
			else if (LastDir.HasValue)
			{
				MakeMove(LastDir.Value);
				LastDir = null;
			}
		}

		public async void NewGame()
		{
			if (Animating)
			{
				NewGameRequest = true;
				return;
			}

			AnimationControl.RemoveAllTiles();

			Animating = false;
			LastDir = null;
			NewGameRequest = false;

			Grid.NewGame();

			await AnimateTiles();
		}

		private void CreatePlaceHolders()
		{
			int y = Y;
			for (int row = 0; row < Grid.Size; row++)
			{
				int x = X;
				for (int col = 0; col < Grid.Size; col++)
				{
					Form.Controls.Add(new Label
					{
						Size = new Size(CellSize, CellSize),
						Location = new Point(x, y),
						BackColor = Colors[0]
					});

					x += CellSize + TileGap;
				}
				y += CellSize + TileGap;
			}
		}

		private async Task AnimateTiles()
		{
			Animating = true;

			var tiles = Grid.GetTiles().Cast<Tile>().Where(t => t != null).Reverse();

			List<Tile> tilesToMove = new List<Tile>();
			List<Tile> tilesToAppear = new List<Tile>();
			List<Tile> tilesToDisappear = new List<Tile>();
			foreach (Tile t in tiles)
			{
				if (t.IsMerged)
				{
					tilesToMove.Add(t.Father1);
					tilesToMove.Add(t.Father2);

					tilesToDisappear.Add(t.Father1);
					tilesToDisappear.Add(t.Father2);

					tilesToAppear.Add(t);
				}
				else if (t.PrevRow == -1) // new tile
				{
					tilesToAppear.Add(t);
				}
				else
				{
					tilesToMove.Add(t);
				}
			}

			await AnimationControl.Move(tilesToMove.Where(t => t.PrevRow != t.Row || t.PrevCol != t.Col).ToArray());
			await AnimationControl.Appear(tilesToAppear.ToArray());
			AnimationControl.Disappear(tilesToDisappear.ToArray());

			Animating = false;
		}
	}
}