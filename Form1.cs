using math_game.Models;
using math_game.Services;

namespace math_game
{
    public partial class Form1 : Form
    {
        private readonly GameService _gameService;
        private System.Windows.Forms.Timer _timer = null!;
        private bool _isGameActive = false;

        // UI Kontrolleri
        private Label lblTitle = null!;
        private Label lblQuestion = null!;
        private TextBox txtAnswer = null!;
        private Button btnAnswer = null!;
        private Button btnPass = null!;
        private Button btnContinue = null!;
        private Button btnNext = null!;
        private Label lblTimer = null!;
        private Label lblScore = null!;
        private Label lblLevel = null!;
        private Label lblBlock = null!;
        private Label lblQuestionNumber = null!;
        private Panel pnlMainMenu = null!;
        private Panel pnlGame = null!;
        private Panel pnlLevelSelect = null!;
        private Panel pnlSettings = null!;
        private Panel pnlHighScores = null!;
        private Panel pnlResults = null!;

        public Form1()
        {
            InitializeComponent();
            _gameService = new GameService();
            InitializeGameUI();
            SetupTimer();
            ShowMainMenu();
        }

        // Oyun UI'ını başlatır
        private void InitializeGameUI()
        {
            this.Size = new Size(800, 600);
            this.Text = "Matematik Oyunu";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightBlue;

            // Ana menü paneli
            pnlMainMenu = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightBlue
            };

            lblTitle = new Label
            {
                Text = "🎮 Matematik Oyunu 🎮",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(600, 50),
                Location = new Point(100, 50)
            };

            var btnNewGame = new Button
            {
                Text = "Yeni Oyun",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(300, 150),
                BackColor = Color.Blue,
                ForeColor = Color.White
            };
            btnNewGame.Click += (s, e) => ShowNewGameDialog();

            var btnSettings = new Button
            {
                Text = "Ayarlar",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(300, 220),
                BackColor = Color.Orange,
                ForeColor = Color.White
            };
            btnSettings.Click += (s, e) => ShowSettings();

            var btnHighScores = new Button
            {
                Text = "Yüksek Skorlar",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(300, 290),
                BackColor = Color.Purple,
                ForeColor = Color.White
            };
            btnHighScores.Click += (s, e) => ShowHighScores();

            var btnExit = new Button
            {
                Text = "Çıkış",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(300, 360),
                BackColor = Color.Red,
                ForeColor = Color.White
            };
            btnExit.Click += (s, e) => Application.Exit();

            pnlMainMenu.Controls.AddRange(new Control[] { lblTitle, btnNewGame, btnSettings, btnHighScores, btnExit });

            // Oyun paneli
            pnlGame = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGreen,
                Visible = false
            };

            lblLevel = new Label
            {
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(20, 20),
                Size = new Size(200, 30)
            };

            lblBlock = new Label
            {
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(20, 60),
                Size = new Size(200, 30)
            };

            lblQuestionNumber = new Label
            {
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(20, 100),
                Size = new Size(200, 30)
            };

            lblTimer = new Label
            {
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(600, 20),
                Size = new Size(150, 30),
                TextAlign = ContentAlignment.MiddleRight
            };

            lblScore = new Label
            {
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(600, 60),
                Size = new Size(150, 30),
                TextAlign = ContentAlignment.MiddleRight
            };

            lblQuestion = new Label
            {
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(200, 200),
                Size = new Size(400, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            txtAnswer = new TextBox
            {
                Font = new Font("Arial", 18),
                Location = new Point(300, 280),
                Size = new Size(200, 40),
                TextAlign = HorizontalAlignment.Center
            };
            txtAnswer.KeyPress += TxtAnswer_KeyPress;

            btnAnswer = new Button
            {
                Text = "Cevapla",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(120, 50),
                Location = new Point(200, 350),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnAnswer.Click += BtnAnswer_Click;

            btnPass = new Button
            {
                Text = "PAS",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(120, 50),
                Location = new Point(340, 350),
                BackColor = Color.Orange,
                ForeColor = Color.White
            };
            btnPass.Click += BtnPass_Click;

            btnContinue = new Button
            {
                Text = "Devam",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(120, 50),
                Location = new Point(480, 350),
                BackColor = Color.Blue,
                ForeColor = Color.White,
                Visible = false
            };
            btnContinue.Click += BtnContinue_Click;

            btnNext = new Button
            {
                Text = "Sonraki",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(120, 50),
                Location = new Point(480, 350),
                BackColor = Color.Purple,
                ForeColor = Color.White,
                Visible = false
            };
            btnNext.Click += BtnNext_Click;

            var btnBackToMenu = new Button
            {
                Text = "Ana Menü",
                Font = new Font("Arial", 12),
                Size = new Size(100, 40),
                Location = new Point(20, 500),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnBackToMenu.Click += (s, e) => ShowMainMenu();

            pnlGame.Controls.AddRange(new Control[] 
            { 
                lblLevel, lblBlock, lblQuestionNumber, lblTimer, lblScore,
                lblQuestion, txtAnswer, btnAnswer, btnPass, btnContinue, btnNext, btnBackToMenu
            });

            // Seviye seçim paneli
            pnlLevelSelect = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightYellow,
                Visible = false
            };

            var lblLevelSelectTitle = new Label
            {
                Text = "Seviye Seçin",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkOrange,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 40),
                Location = new Point(200, 50)
            };

            pnlLevelSelect.Controls.Add(lblLevelSelectTitle);

            // Seviye butonları
            for (int i = 1; i <= 5; i++)
            {
                var levelBtn = new Button
                {
                    Text = $"Seviye {i}",
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    Size = new Size(150, 80),
                    Location = new Point(100 + (i - 1) * 120, 150),
                    Tag = i
                };

                if (i == 1 || _gameService.CurrentGameState.Levels[i - 1].IsUnlocked)
                {
                    levelBtn.BackColor = Color.Green;
                    levelBtn.ForeColor = Color.White;
                    levelBtn.Click += LevelButton_Click;
                }
                else
                {
                    levelBtn.BackColor = Color.Gray;
                    levelBtn.ForeColor = Color.DarkGray;
                    levelBtn.Enabled = false;
                }

                pnlLevelSelect.Controls.Add(levelBtn);
            }

            var btnBackFromLevels = new Button
            {
                Text = "Geri",
                Font = new Font("Arial", 12),
                Size = new Size(100, 40),
                Location = new Point(350, 500),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnBackFromLevels.Click += (s, e) => ShowMainMenu();
            pnlLevelSelect.Controls.Add(btnBackFromLevels);

            // Ayarlar paneli
            pnlSettings = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightCyan,
                Visible = false
            };

            var lblSettingsTitle = new Label
            {
                Text = "Ayarlar",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkCyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 40),
                Location = new Point(200, 50)
            };

            var lblPlayerName = new Label
            {
                Text = "Oyuncu Adı:",
                Font = new Font("Arial", 14),
                Location = new Point(200, 150),
                Size = new Size(150, 30)
            };

            var txtPlayerName = new TextBox
            {
                Text = _gameService.Settings.PlayerName,
                Font = new Font("Arial", 12),
                Location = new Point(350, 150),
                Size = new Size(200, 30)
            };

            var btnSaveSettings = new Button
            {
                Text = "Kaydet",
                Font = new Font("Arial", 12),
                Size = new Size(100, 40),
                Location = new Point(350, 200),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnSaveSettings.Click += (s, e) =>
            {
                _gameService.Settings.PlayerName = txtPlayerName.Text;
                _gameService.SaveSettings();
                ShowMainMenu();
            };

            var btnBackFromSettings = new Button
            {
                Text = "Geri",
                Font = new Font("Arial", 12),
                Size = new Size(100, 40),
                Location = new Point(200, 200),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnBackFromSettings.Click += (s, e) => ShowMainMenu();

            pnlSettings.Controls.AddRange(new Control[] 
            { 
                lblSettingsTitle, lblPlayerName, txtPlayerName, btnSaveSettings, btnBackFromSettings 
            });

            // Yüksek skorlar paneli
            pnlHighScores = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightPink,
                Visible = false
            };

            var lblHighScoresTitle = new Label
            {
                Text = "Yüksek Skorlar",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkMagenta,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 40),
                Location = new Point(200, 50)
            };

            var btnBackFromHighScores = new Button
            {
                Text = "Geri",
                Font = new Font("Arial", 12),
                Size = new Size(100, 40),
                Location = new Point(350, 500),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnBackFromHighScores.Click += (s, e) => ShowMainMenu();

            pnlHighScores.Controls.AddRange(new Control[] { lblHighScoresTitle, btnBackFromHighScores });

            // Sonuçlar paneli
            pnlResults = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGoldenrodYellow,
                Visible = false
            };

            this.Controls.AddRange(new Control[] 
            { 
                pnlMainMenu, pnlGame, pnlLevelSelect, pnlSettings, pnlHighScores, pnlResults 
            });
        }

        // Timer'ı ayarlar
        private void SetupTimer()
        {
            _timer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // 1 saniye
            };
            _timer.Tick += Timer_Tick;
        }

        // Ana menüyü gösterir
        private void ShowMainMenu()
        {
            HideAllPanels();
            pnlMainMenu.Visible = true;
            _isGameActive = false;
            _timer.Stop();
        }

        // Seviye seçim ekranını gösterir
        private void ShowLevelSelect()
        {
            HideAllPanels();
            pnlLevelSelect.Visible = true;
            UpdateLevelButtons();
        }

        // Ayarlar ekranını gösterir
        private void ShowSettings()
        {
            HideAllPanels();
            pnlSettings.Visible = true;
        }

        // Yüksek skorlar ekranını gösterir
        private void ShowHighScores()
        {
            HideAllPanels();
            pnlHighScores.Visible = true;
            LoadHighScores();
        }

        // Yeni oyun dialog'unu gösterir
        private void ShowNewGameDialog()
        {
            var inputForm = new Form
            {
                Text = "Yeni Oyun",
                Size = new Size(300, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblPrompt = new Label
            {
                Text = "Oyuncu adınızı girin:",
                Location = new Point(20, 20),
                Size = new Size(250, 20)
            };

            var txtPlayerName = new TextBox
            {
                Text = _gameService.Settings.PlayerName,
                Location = new Point(20, 50),
                Size = new Size(250, 25)
            };

            var btnOK = new Button
            {
                Text = "Tamam",
                DialogResult = DialogResult.OK,
                Location = new Point(120, 80),
                Size = new Size(75, 25)
            };

            var btnCancel = new Button
            {
                Text = "İptal",
                DialogResult = DialogResult.Cancel,
                Location = new Point(200, 80),
                Size = new Size(75, 25)
            };

            inputForm.Controls.AddRange(new Control[] { lblPrompt, txtPlayerName, btnOK, btnCancel });
            inputForm.AcceptButton = btnOK;
            inputForm.CancelButton = btnCancel;

            if (inputForm.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                _gameService.Settings.PlayerName = txtPlayerName.Text;
                _gameService.SaveSettings();
                _gameService.ResetGame();
                ShowLevelSelect();
            }
        }

        // Seviye butonlarını günceller
        private void UpdateLevelButtons()
        {
            foreach (Control control in pnlLevelSelect.Controls)
            {
                if (control is Button btn && btn.Tag is int levelNumber)
                {
                    var level = _gameService.CurrentGameState.Levels[levelNumber - 1];
                    if (levelNumber == 1 || level.IsUnlocked)
                    {
                        btn.BackColor = Color.Green;
                        btn.ForeColor = Color.White;
                        btn.Enabled = true;
                        btn.Click -= LevelButton_Click; // Önceki event'i kaldır
                        btn.Click += LevelButton_Click; // Yeni event ekle
                    }
                    else
                    {
                        btn.BackColor = Color.Gray;
                        btn.ForeColor = Color.DarkGray;
                        btn.Enabled = false;
                        btn.Click -= LevelButton_Click; // Event'i kaldır
                    }
                }
            }
        }

        // Tüm panelleri gizler
        private void HideAllPanels()
        {
            pnlMainMenu.Visible = false;
            pnlGame.Visible = false;
            pnlLevelSelect.Visible = false;
            pnlSettings.Visible = false;
            pnlHighScores.Visible = false;
            pnlResults.Visible = false;
        }

        // Seviye butonuna tıklandığında
        private void LevelButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int levelNumber)
            {
                StartLevel(levelNumber);
            }
        }

        // Seviyeyi başlatır
        private void StartLevel(int levelNumber)
        {
            _gameService.GenerateQuestionsForLevel(levelNumber);
            HideAllPanels();
            pnlGame.Visible = true;
            _isGameActive = true;
            _timer.Start();
            UpdateGameUI();
        }

        // Oyun UI'ını günceller
        private void UpdateGameUI()
        {
            var question = _gameService.GetCurrentQuestion();
            if (question == null) return;

            lblLevel.Text = $"Seviye: {_gameService.CurrentGameState.CurrentLevel}";
            lblBlock.Text = $"Blok: {_gameService.CurrentGameState.CurrentBlock}/4";
            lblQuestionNumber.Text = $"Soru: {_gameService.CurrentGameState.CurrentQuestionIndex + 1}/20";
            lblQuestion.Text = question.QuestionText;
            lblScore.Text = $"Skor: {_gameService.CurrentGameState.TotalScore}";
            
            UpdateTimerDisplay();

            txtAnswer.Clear();
            txtAnswer.Focus();
        }

        // Timer görüntüsünü günceller
        private void UpdateTimerDisplay()
        {
            var timeSpan = TimeSpan.FromSeconds(_gameService.CurrentGameState.TimeRemaining);
            lblTimer.Text = $"Süre: {timeSpan:mm\\:ss}";
            
            if (_gameService.CurrentGameState.TimeRemaining <= 30)
            {
                lblTimer.ForeColor = Color.Red;
            }
            else if (_gameService.CurrentGameState.TimeRemaining <= 60)
            {
                lblTimer.ForeColor = Color.Orange;
            }
            else
            {
                lblTimer.ForeColor = Color.Black;
            }
        }

        // Timer tick olayı
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!_isGameActive) return;

            _gameService.UpdateTimer();
            UpdateTimerDisplay();

            if (_gameService.IsTimeUp())
            {
                _gameService.HandleTimeUp();
                EndLevel();
            }
        }

        // Cevap butonuna tıklandığında
        private void BtnAnswer_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAnswer.Text)) return;

            var isCorrect = _gameService.CheckAnswer(txtAnswer.Text);
            
            if (isCorrect)
            {
                MessageBox.Show("✅ Doğru!", "Tebrikler", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var question = _gameService.GetCurrentQuestion();
                MessageBox.Show($"❌ Yanlış! Doğru cevap: {question?.CorrectAnswer}", 
                    "Yanlış Cevap", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (_gameService.IsLevelCompleted())
            {
                EndLevel();
            }
            else
            {
                _gameService.NextQuestion();
                UpdateGameUI();
            }
        }

        // Pas butonuna tıklandığında
        private void BtnPass_Click(object? sender, EventArgs e)
        {
            _gameService.PassQuestion();
            
            // Blok sonunda pas geçilen soruları tekrar sor
            if (_gameService.CurrentGameState.CurrentQuestionIndex % 5 == 0)
            {
                btnContinue.Visible = true;
                btnNext.Visible = true;
            }
            else
            {
                _gameService.NextQuestion();
                UpdateGameUI();
            }
        }

        // Devam butonuna tıklandığında
        private void BtnContinue_Click(object? sender, EventArgs e)
        {
            btnContinue.Visible = false;
            btnNext.Visible = false;
            UpdateGameUI();
        }

        // Sonraki butonuna tıklandığında
        private void BtnNext_Click(object? sender, EventArgs e)
        {
            btnContinue.Visible = false;
            btnNext.Visible = false;
            _gameService.NextQuestion();
            UpdateGameUI();
        }

        // Seviye sonunu işler
        private void EndLevel()
        {
            _isGameActive = false;
            _timer.Stop();

            var (correctCount, stars, score) = _gameService.CalculateLevelResult();
            
            // Yüksek skor ekle
            _gameService.AddHighScore(_gameService.Settings.PlayerName, 
                _gameService.CurrentGameState.CurrentLevel, score, stars);

            // Oyun verilerini kaydet
            _gameService.SaveGameData();

            // Sonuç mesajı göster
            var starsText = new string('⭐', stars);
            var message = $"Seviye {_gameService.CurrentGameState.CurrentLevel} Tamamlandı!\n\n" +
                         $"Doğru Cevap: {correctCount}/20\n" +
                         $"Yıldız: {starsText}\n" +
                         $"Skor: {score}";

            if (correctCount >= 11)
            {
                message += "\n\n🎉 Tebrikler! Sonraki seviye açıldı!";
            }
            else
            {
                message += "\n\n😔 En az 11 doğru cevap gerekli.";
            }

            MessageBox.Show(message, "Seviye Sonu", MessageBoxButtons.OK, 
                correctCount >= 11 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            // Her zaman seviye seçim ekranına dön
            ShowLevelSelect();
        }

        // Cevap kutusuna Enter tuşu basıldığında
        private void TxtAnswer_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnAnswer_Click(sender, e);
            }
        }

        // Yüksek skorları yükler
        private void LoadHighScores()
        {
            // Mevcut kontrolleri temizle
            var controlsToRemove = pnlHighScores.Controls.Cast<Control>()
                .Where(c => c is Label && c != pnlHighScores.Controls[0])
                .ToList();

            foreach (var control in controlsToRemove)
            {
                pnlHighScores.Controls.Remove(control);
            }

            // Yüksek skorları göster
            var highScores = _gameService.HighScores.Take(10).ToList();
            
            for (int i = 0; i < highScores.Count; i++)
            {
                var score = highScores[i];
                var lblScore = new Label
                {
                    Text = $"{i + 1}. {score.PlayerName} - Seviye {score.Level} - {score.Score} puan - {new string('⭐', score.Stars)}",
                    Font = new Font("Arial", 12),
                    Location = new Point(100, 120 + i * 30),
                    Size = new Size(600, 25)
                };
                pnlHighScores.Controls.Add(lblScore);
            }
        }

        // Form kapatılırken
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _gameService.SaveGameData();
            base.OnFormClosing(e);
        }
    }
}
