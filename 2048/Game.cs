using System.Windows.Forms;

namespace _2048
{
	public partial class Game : Form
	{
		private readonly VisualGrid Grid;

		public Game()
		{
			InitializeComponent();

			int size = 300;
			int cells = 4;
			Grid = new VisualGrid(this, new Grid(cells), 50, 30, size / cells);

			KeyDown += Game_KeyDown;
			Grid.Grid.MoveMade += (s, e) => UpdateStats();
			Grid.Grid.OnNewGame += (s, e) => UpdateStats();

			NewGame();
		}

		private void Game_KeyDown(object sender, KeyEventArgs e)
		{
			Keys key = e.KeyCode;
			if (key == Keys.Up || key == Keys.W)
			{
				Grid.MakeMove(Dir.Up);
			}
			else if (key == Keys.Right || key == Keys.D)
			{
				Grid.MakeMove(Dir.Right);
			}
			else if (key == Keys.Down || key == Keys.S)
			{
				Grid.MakeMove(Dir.Down);
			}
			else if (key == Keys.Left || key == Keys.A)
			{
				Grid.MakeMove(Dir.Left);
			}
			else if (key == Keys.Space)
			{
				NewGame();
			}

			if (Grid.Grid.GameOver)
			{
				GameOver();
			}
		}

		private void GameOver()
		{
			GameOverLbl.Visible = true;
		}

		private void NewGame()
		{
			Grid.NewGame();
			GameOverLbl.Visible = false;
		}

		private void UpdateStats()
		{
			UpdateScore(Grid.Grid.Score);
			UpdateMoves(Grid.Grid.Moves);
		}

		private void UpdateScore(int score)
		{
			Score.Text = $"תוצאה: {score}";
		}

		private void UpdateMoves(int moves)
		{
			Moves.Text = $"מהלכים: {moves}";
		}
	}
}
