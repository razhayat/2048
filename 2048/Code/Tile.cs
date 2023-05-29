namespace _2048
{
	public class Tile
	{
		public int PrevRow { get; private set; }
		public int PrevCol { get; private set; }
		public int Row { get; private set; }
		public int Col { get; private set; }
		public int Value { get; }

		public bool IsMerged { get; private set; }
		public Tile Father1 { get; private set; }
		public Tile Father2 { get; private set; }

		private Tile(int value)
		{
			Value = value;
		}

		public static Tile Regular(int row, int col, int value)
		{
			return new Tile(value)
			{
				PrevRow = -1,
				PrevCol = -1,
				Row = row,
				Col = col,
				IsMerged = false,
				Father1 = null,
				Father2 = null
			};
		}

		public static Tile Merged(int row, int col, int value, Tile father1, Tile father2)
		{
			Tile tile = new Tile(value)
			{
				PrevRow = -1,
				PrevCol = -1,
				IsMerged = true,
				Father1 = father1,
				Father2 = father2
			};
			tile.SetRow(row);
			tile.SetCol(col);

			return tile;
		}

		public void SetRow(int row)
		{
			Row = row;
			
			if (IsMerged)
			{
				Father1.SetRow(row);
				Father2.SetRow(row);
			}
		}

		public void SetCol(int col)
		{
			Col = col;

			if (IsMerged)
			{
				Father1.SetCol(col);
				Father2.SetCol(col);
			}
		}

		public void NextCycle()
		{
			PrevRow = Row;
			PrevCol = Col;

			IsMerged = false;
			Father1 = null;
			Father2 = null;
		}
	}
}