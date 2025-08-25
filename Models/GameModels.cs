using System.Text.Json;

namespace math_game.Models
{
    // Matematik sorusu modeli
    public class MathQuestion
    {
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public string Operation { get; set; } = "";
        public int CorrectAnswer { get; set; }
        public bool IsAnswered { get; set; } = false;
        public bool IsCorrect { get; set; } = false;
        public int PassCount { get; set; } = 0;
        public string? UserAnswer { get; set; }

        public string QuestionText => $"{Number1} {Operation} {Number2} = ?";
    }

    // Oyun seviyesi modeli
    public class GameLevel
    {
        public int LevelNumber { get; set; }
        public int TimeLimit { get; set; } // saniye cinsinden
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
        public List<string> Operations { get; set; } = new();
        public bool IsUnlocked { get; set; } = false;
        public int Stars { get; set; } = 0;
        public int CorrectAnswers { get; set; } = 0;
        public DateTime? LastPlayed { get; set; }
    }

    // Oyun durumu modeli
    public class GameState
    {
        public int CurrentLevel { get; set; } = 1;
        public int CurrentQuestionIndex { get; set; } = 0;
        public int CurrentBlock { get; set; } = 1;
        public List<MathQuestion> Questions { get; set; } = new();
        public int TimeRemaining { get; set; }
        public int TotalScore { get; set; } = 0;
        public List<GameLevel> Levels { get; set; } = new();
        public DateTime GameStartTime { get; set; } = DateTime.Now;
    }

    // Yüksek skor modeli
    public class HighScore
    {
        public string PlayerName { get; set; } = "";
        public int Level { get; set; }
        public int Score { get; set; }
        public int Stars { get; set; }
        public DateTime Date { get; set; }
    }

    // Oyun ayarları modeli
    public class GameSettings
    {
        public bool RandomOperations { get; set; } = true;
        public string SelectedOperation { get; set; } = "all";
        public bool SoundEnabled { get; set; } = true;
        public string PlayerName { get; set; } = "Öğrenci";
    }
}
