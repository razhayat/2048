using System;
using System.Linq;
using System.Collections.Generic;

namespace _2048
{
	public class Grid
	{
		public int Size { get; }

		public int Score { get; private set; }
		public int Moves { get; private set; }
		public bool GameOver { get; private set; }

		public event EventHandler MoveMade;
		public event EventHandler OnNewGame;

		private Tile[,] grid;

		private static readonly Random RNG = new Random();

		public Grid()
		{
			Size = 4;
			GameOver = true;
		}
		public Grid(int size) : this()
		{
			Size = size;
		}

		public void MakeMove(Dir dir)
		{
			if (GameOver) return;

			NextCycle();

			Tile[,] gridBeforeMove = Copy(grid);

			int rotations = (int)dir;
			RotateClockWise(rotations);

			Push();
			Merge();
			Push();

			rotations = (4 - rotations) % 4;
			RotateClockWise(rotations);

			// if the move was valid
			if (!IsSame(gridBeforeMove, grid))
			{
				Moves++;
				AddRandom();
				GameOver = IsGameOver();
			}

			MoveMade?.Invoke(this, EventArgs.Empty);
		}

		public void NewGame()
		{
			grid = new Tile[Size, Size];
			Score = 0;
			Moves = 0;
			GameOver = false;

			AddRandom();
			AddRandom();

			OnNewGame?.Invoke(this, EventArgs.Empty);
		}

		public Tile[,] GetTiles()
		{
			return Copy(grid);
		}

		private void NextCycle()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					grid[i, j]?.NextCycle();
				}
			}
		}

		private void RotateClockWise(int rotations)
		{
			for (int i = 0; i < rotations; i++)
			{
				RotateClockWise();
			}
		}

		private void RotateClockWise()
		{
			void Rotate(int top, int left, int size)
			{
				if (size <= 1) return;

				int row = top;
				int col = left;
				for (int i = 0; i < size - 1; i++)
				{
					Tile tile = grid[row, col];
					for (int j = 0; j < 4; j++)
					{
						(row, col) = (col, Size - 1 - row);
						(grid[row, col], tile) = (tile, grid[row, col]);

						grid[row, col]?.SetRow(row);
						grid[row, col]?.SetCol(col);
					}
					col++;
				}

				Rotate(top + 1, left + 1, size - 2);
			}

			Rotate(0, 0, Size);
		}

		private void Push()
		{
			for (int col = 0; col < Size; col++)
			{
				int emptyRow = 0;
				for (int row = 0; row < Size; row++)
				{
					Tile tile = grid[row, col];
					if (tile == null) continue;

					if (emptyRow != row)
					{
						grid[emptyRow, col] = tile;
						tile.SetRow(emptyRow);
						grid[row, col] = null;
					}
					emptyRow++;
				}
			}
		}

		private void Merge()
		{
			for (int col = 0; col < Size; col++)
			{
				for (int row = 0; row < Size - 1; row++)
				{
					Tile curr = grid[row, col];
					Tile next = grid[row + 1, col];

					if (curr != null && next != null && curr.Value == next.Value)
					{
						grid[row, col] = Tile.Merged(row, col, curr.Value + 1, curr, next);
						grid[row + 1, col] = null;

						Score += 1 << grid[row, col].Value;
					}
				}
			}
		}

		private void AddRandom()
		{
			List<(int, int)> emptyCells = EmptyCells();
			int index = RNG.Next(emptyCells.Count);

			(int row, int col) = emptyCells[index];

			int val = RNG.NextDouble() < 0.9 ? 1 : 2;
			grid[row, col] = Tile.Regular(row, col, val);
		}

		private List<(int, int)> EmptyCells()
		{
			List<(int, int)> emptyCells = new List<(int, int)>();

			for (int row = 0; row < Size; row++)
			{
				for (int col = 0; col < Size; col++)
				{
					if (grid[row, col] == null) emptyCells.Add((row, col));
				}
			}

			return emptyCells;
		}

		private bool IsGameOver()
		{
			if (EmptyCells().Count > 0) return false;

			for (int row = 0; row < Size; row++)
			{
				for (int col = 0; col < Size; col++)
				{
					IEnumerable<(int row, int col)> neighbors = new (int row, int col)[]
					{
						(row - 1, col),
						(row + 1, col),
						(row, col - 1),
						(row, col + 1)
					}.Where(n => n.row >= 0 && n.row < Size && n.col >= 0 && n.col < Size);

					foreach (var n in neighbors)
					{
						if (grid[row, col].Value == grid[n.row, n.col].Value) return false;
					}
				}
			}

			return true;
		}

		private static T[,] Copy<T>(T[,] mat)
		{
			T[,] output = new T[mat.GetLength(0), mat.GetLength(1)];

			for (int i = 0; i < mat.GetLength(0); i++)
			{
				for (int j = 0; j < mat.GetLength(1); j++)
				{
					output[i, j] = mat[i, j];
				}
			}

			return output;
		}

		private static bool IsSame(Tile[,] mat1, Tile[,] mat2)
		{
			if (mat1.GetLength(0) != mat2.GetLength(0) || mat1.GetLength(1) != mat2.GetLength(1)) return false;

			for (int row = 0; row < mat1.GetLength(0); row++)
			{
				for (int col = 0; col < mat1.GetLength(1); col++)
				{
					Tile t1 = mat1[row, col];
					Tile t2 = mat2[row, col];

					if (t1 != t2) return false;
				}
			}
			return true;
		}
	}
}