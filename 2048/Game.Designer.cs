
namespace _2048
{
	partial class Game
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.Score = new System.Windows.Forms.Label();
			this.Moves = new System.Windows.Forms.Label();
			this.GameOverLbl = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(725, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(241, 74);
			this.label1.TabIndex = 1;
			this.label1.Text = "לחצו על רווח כדי\r\nלהתחיל משחק חדש";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Score
			// 
			this.Score.AutoSize = true;
			this.Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
			this.Score.ForeColor = System.Drawing.Color.White;
			this.Score.Location = new System.Drawing.Point(809, 216);
			this.Score.Name = "Score";
			this.Score.Size = new System.Drawing.Size(0, 37);
			this.Score.TabIndex = 2;
			// 
			// Moves
			// 
			this.Moves.AutoSize = true;
			this.Moves.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
			this.Moves.ForeColor = System.Drawing.Color.White;
			this.Moves.Location = new System.Drawing.Point(809, 303);
			this.Moves.Name = "Moves";
			this.Moves.Size = new System.Drawing.Size(0, 37);
			this.Moves.TabIndex = 3;
			// 
			// GameOverLbl
			// 
			this.GameOverLbl.AutoSize = true;
			this.GameOverLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
			this.GameOverLbl.ForeColor = System.Drawing.Color.Red;
			this.GameOverLbl.Location = new System.Drawing.Point(764, 440);
			this.GameOverLbl.Name = "GameOverLbl";
			this.GameOverLbl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.GameOverLbl.Size = new System.Drawing.Size(167, 37);
			this.GameOverLbl.TabIndex = 4;
			this.GameOverLbl.Text = "המשחק נגמר!";
			this.GameOverLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.GameOverLbl.Visible = false;
			// 
			// Game
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.ClientSize = new System.Drawing.Size(1074, 580);
			this.Controls.Add(this.GameOverLbl);
			this.Controls.Add(this.Moves);
			this.Controls.Add(this.Score);
			this.Controls.Add(this.label1);
			this.Name = "Game";
			this.Text = "Game";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label Score;
		private System.Windows.Forms.Label Moves;
		private System.Windows.Forms.Label GameOverLbl;
	}
}