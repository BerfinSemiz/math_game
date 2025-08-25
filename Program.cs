namespace math_game
{
    internal static class Program
    {
        // The main entry point for the application.
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            var form = new Form1();
            
            // Hile kodu kontrolü
            if (args.Length >= 2 && args[0].ToLower() == "open")
            {
                var levelArg = args[1].ToLower();
                var gameService = form.GetType().GetField("_gameService", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form);
                
                if (gameService != null)
                {
                    var unlockMethod = gameService.GetType().GetMethod("UnlockLevel");
                    
                    if (levelArg == "all")
                    {
                        unlockMethod?.Invoke(gameService, new object[] { 0 });
                    }
                    else if (int.TryParse(levelArg, out int levelNumber) && levelNumber >= 2 && levelNumber <= 5)
                    {
                        unlockMethod?.Invoke(gameService, new object[] { levelNumber });
                    }
                }
            }
            
            Application.Run(form);
        }
    }
}