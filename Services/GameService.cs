using math_game.Models;
using System.Text.Json;

namespace math_game.Services
{
    // Oyun mantığını yöneten servis sınıfı
    public class GameService
    {
        private readonly Random _random = new Random();
        private readonly string _saveFilePath = "game_save.json";
        private readonly string _highScoresFilePath = "high_scores.json";
        private readonly string _settingsFilePath = "settings.json";

        public GameState CurrentGameState { get; private set; } = new();
        public GameSettings Settings { get; private set; } = new();
        public List<HighScore> HighScores { get; private set; } = new();

        public GameService()
        {
            InitializeLevels();
            LoadGameData();
        }

        // Oyun seviyelerini başlatır
        private void InitializeLevels()
        {
            CurrentGameState.Levels = new List<GameLevel>
            {
                new GameLevel
                {
                    LevelNumber = 1,
                    TimeLimit = 300, // 5 dakika
                    MinNumber = 1,
                    MaxNumber = 10,
                    Operations = new List<string> { "+", "-" },
                    IsUnlocked = true
                },
                new GameLevel
                {
                    LevelNumber = 2,
                    TimeLimit = 360, // 6 dakika
                    MinNumber = 1,
                    MaxNumber = 20,
                    Operations = new List<string> { "+", "-", "*" },
                    IsUnlocked = false
                },
                new GameLevel
                {
                    LevelNumber = 3,
                    TimeLimit = 420, // 7 dakika
                    MinNumber = 1,
                    MaxNumber = 50,
                    Operations = new List<string> { "+", "-", "*", "/" },
                    IsUnlocked = false
                },
                new GameLevel
                {
                    LevelNumber = 4,
                    TimeLimit = 480, // 8 dakika
                    MinNumber = 10,
                    MaxNumber = 100,
                    Operations = new List<string> { "+", "-", "*", "/" },
                    IsUnlocked = false
                },
                new GameLevel
                {
                    LevelNumber = 5,
                    TimeLimit = 540, // 9 dakika
                    MinNumber = 10,
                    MaxNumber = 200,
                    Operations = new List<string> { "+", "-", "*", "/" },
                    IsUnlocked = false
                }
            };
        }

        // Belirtilen seviyeyi açar (hile kodu için)
        public void UnlockLevel(int levelNumber)
        {
            if (levelNumber >= 1 && levelNumber <= 5)
            {
                CurrentGameState.Levels[levelNumber - 1].IsUnlocked = true;
            }
            else if (levelNumber == 0) // "all" için
            {
                foreach (var level in CurrentGameState.Levels)
                {
                    level.IsUnlocked = true;
                }
            }
        }

        // Yeni bir seviye için sorular oluşturur
        public void GenerateQuestionsForLevel(int levelNumber)
        {
            var level = CurrentGameState.Levels[levelNumber - 1];
            CurrentGameState.Questions.Clear();
            CurrentGameState.CurrentLevel = levelNumber;
            CurrentGameState.CurrentQuestionIndex = 0;
            CurrentGameState.CurrentBlock = 1;
            CurrentGameState.TimeRemaining = level.TimeLimit;

            for (int i = 0; i < 20; i++) // 4 blok × 5 soru = 20 soru
            {
                var question = GenerateQuestion(level);
                CurrentGameState.Questions.Add(question);
            }
        }

        // Tek bir soru oluşturur
        private MathQuestion GenerateQuestion(GameLevel level)
        {
            string operation;
            if (Settings.RandomOperations)
            {
                operation = level.Operations[_random.Next(level.Operations.Count)];
            }
            else
            {
                operation = Settings.SelectedOperation == "all" 
                    ? level.Operations[_random.Next(level.Operations.Count)]
                    : Settings.SelectedOperation;
            }

            int number1, number2, correctAnswer;

            do
            {
                // Seviye 2 için özel sayı aralıkları
                if (level.LevelNumber == 2)
                {
                    if (operation == "*")
                    {
                        // Çarpma için tek basamaklı sayılar (1-9)
                        number1 = _random.Next(1, 10);
                        number2 = _random.Next(1, 10);
                    }
                    else
                    {
                        // Toplama ve çıkarma için iki basamaklı sayılar (10-99)
                        number1 = _random.Next(10, 100);
                        number2 = _random.Next(10, 100);
                    }
                }
                else
                {
                    // Diğer seviyeler için normal aralık
                    number1 = _random.Next(level.MinNumber, level.MaxNumber + 1);
                    number2 = _random.Next(level.MinNumber, level.MaxNumber + 1);
                }

                correctAnswer = operation switch
                {
                    "+" => number1 + number2,
                    "-" => number1 - number2,
                    "*" => number1 * number2,
                    "/" => number1 / number2,
                    _ => number1 + number2
                };

                // Bölme işlemi için özel kontrol
                if (operation == "/")
                {
                    // Tam bölünebilir sayılar seç
                    if (number2 == 0 || number1 % number2 != 0)
                    {
                        continue;
                    }
                }

            } while (operation == "-" && correctAnswer < 0); // Çıkarma işleminde negatif sonuç olmasın

            return new MathQuestion
            {
                Number1 = number1,
                Number2 = number2,
                Operation = operation,
                CorrectAnswer = correctAnswer
            };
        }

        // Mevcut soruyu alır
        public MathQuestion? GetCurrentQuestion()
        {
            if (CurrentGameState.CurrentQuestionIndex < CurrentGameState.Questions.Count)
            {
                return CurrentGameState.Questions[CurrentGameState.CurrentQuestionIndex];
            }
            return null;
        }

        // Cevabı kontrol eder
        public bool CheckAnswer(string answer)
        {
            var question = GetCurrentQuestion();
            if (question == null) return false;

            if (int.TryParse(answer, out int userAnswer))
            {
                question.IsAnswered = true;
                question.IsCorrect = userAnswer == question.CorrectAnswer;
                question.UserAnswer = answer;

                if (question.IsCorrect)
                {
                    CurrentGameState.TotalScore += CalculateScore();
                }

                return question.IsCorrect;
            }
            return false;
        }

        // Soruyu pas geçer
        public void PassQuestion()
        {
            var question = GetCurrentQuestion();
            if (question != null)
            {
                question.PassCount++;
                
                // İkinci pas yanlış kabul edilir
                if (question.PassCount >= 2)
                {
                    question.IsAnswered = true;
                    question.IsCorrect = false;
                }
            }
        }

        // Sonraki soruya geçer
        public bool NextQuestion()
        {
            CurrentGameState.CurrentQuestionIndex++;
            
            // Blok kontrolü (5 soru = 1 blok)
            if (CurrentGameState.CurrentQuestionIndex % 5 == 0)
            {
                CurrentGameState.CurrentBlock++;
                
                // Pas geçilen soruları tekrar sor
                var passedQuestions = CurrentGameState.Questions
                    .Where(q => q.PassCount > 0 && q.PassCount < 2)
                    .ToList();
                
                foreach (var question in passedQuestions)
                {
                    question.PassCount = 0;
                    question.IsAnswered = false;
                    question.IsCorrect = false;
                    question.UserAnswer = null;
                }
            }

            return CurrentGameState.CurrentQuestionIndex < CurrentGameState.Questions.Count;
        }

        // Seviye tamamlandı mı kontrol eder
        public bool IsLevelCompleted()
        {
            var answeredQuestions = CurrentGameState.Questions.Count(q => q.IsAnswered);
            return answeredQuestions >= 20;
        }

        // Seviye sonucunu hesaplar
        public (int correctCount, int stars, int score) CalculateLevelResult()
        {
            var correctCount = CurrentGameState.Questions.Count(q => q.IsCorrect);
            var stars = correctCount switch
            {
                >= 19 => 3, // 19-20 doğru = 3 yıldız
                >= 16 => 2, // 16-18 doğru = 2 yıldız
                >= 11 => 1, // 11-15 doğru = 1 yıldız
                _ => 0
            };

            var score = correctCount * CalculateScore();
            
            // Seviyeyi güncelle
            var level = CurrentGameState.Levels[CurrentGameState.CurrentLevel - 1];
            level.CorrectAnswers = correctCount;
            level.Stars = Math.Max(level.Stars, stars);
            level.LastPlayed = DateTime.Now;

            // Sonraki seviyeyi aç (en az 11 doğru cevap gerekli)
            if (correctCount >= 11 && CurrentGameState.CurrentLevel < 5)
            {
                CurrentGameState.Levels[CurrentGameState.CurrentLevel].IsUnlocked = true;
            }
            
            // Oyun verilerini kaydet
            SaveGameData();

            return (correctCount, stars, score);
        }

        // Soru başına puan hesaplar
        private int CalculateScore()
        {
            return CurrentGameState.CurrentLevel * 10;
        }



        // Süreyi günceller
        public void UpdateTimer()
        {
            if (CurrentGameState.TimeRemaining > 0)
            {
                CurrentGameState.TimeRemaining--;
            }
        }

        // Süre doldu mu kontrol eder
        public bool IsTimeUp()
        {
            return CurrentGameState.TimeRemaining <= 0;
        }

        // Süre dolduğunda cevaplanmayan soruları yanlış kabul eder
        public void HandleTimeUp()
        {
            foreach (var question in CurrentGameState.Questions)
            {
                if (!question.IsAnswered)
                {
                    question.IsAnswered = true;
                    question.IsCorrect = false;
                }
            }
        }

        // Oyun verilerini kaydeder
        public void SaveGameData()
        {
            try
            {
                var gameData = new
                {
                    CurrentGameState,
                    Settings,
                    HighScores
                };

                var json = JsonSerializer.Serialize(gameData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_saveFilePath, json);
            }
            catch (Exception ex)
            {
                // Hata durumunda sessizce devam et
                Console.WriteLine($"Kaydetme hatası: {ex.Message}");
            }
        }

        // Oyun verilerini yükler
        public void LoadGameData()
        {
            try
            {
                if (File.Exists(_saveFilePath))
                {
                    var json = File.ReadAllText(_saveFilePath);
                    var gameData = JsonSerializer.Deserialize<dynamic>(json);
                    
                    // Basit JSON parsing (daha güvenli yaklaşım)
                    LoadSettings();
                    LoadHighScores();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yükleme hatası: {ex.Message}");
                // Varsayılan ayarları kullan
            }
        }

        // Ayarları kaydeder
        public void SaveSettings()
        {
            try
            {
                var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ayar kaydetme hatası: {ex.Message}");
            }
        }

        // Ayarları yükler
        public void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    var json = File.ReadAllText(_settingsFilePath);
                    Settings = JsonSerializer.Deserialize<GameSettings>(json) ?? new GameSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ayar yükleme hatası: {ex.Message}");
                Settings = new GameSettings();
            }
        }

        // Yüksek skorları kaydeder
        public void SaveHighScores()
        {
            try
            {
                var json = JsonSerializer.Serialize(HighScores, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_highScoresFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yüksek skor kaydetme hatası: {ex.Message}");
            }
        }

        // Yüksek skorları yükler
        public void LoadHighScores()
        {
            try
            {
                if (File.Exists(_highScoresFilePath))
                {
                    var json = File.ReadAllText(_highScoresFilePath);
                    HighScores = JsonSerializer.Deserialize<List<HighScore>>(json) ?? new List<HighScore>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yüksek skor yükleme hatası: {ex.Message}");
                HighScores = new List<HighScore>();
            }
        }

        // Yeni yüksek skor ekler
        public void AddHighScore(string playerName, int level, int score, int stars)
        {
            var highScore = new HighScore
            {
                PlayerName = playerName,
                Level = level,
                Score = score,
                Stars = stars,
                Date = DateTime.Now
            };

            HighScores.Add(highScore);
            HighScores = HighScores.OrderByDescending(hs => hs.Score).Take(10).ToList();
            SaveHighScores();
        }

        // Oyunu sıfırlar
        public void ResetGame()
        {
            CurrentGameState = new GameState();
            InitializeLevels();
            CurrentGameState.Levels[0].IsUnlocked = true;
        }
    }
}
