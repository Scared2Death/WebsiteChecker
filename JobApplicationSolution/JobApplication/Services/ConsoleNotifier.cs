using JobApplication.Interfaces;

namespace JobApplication.Services
{
    public class ConsoleNotifier : INotifier
    {
        public void Notify(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"[CONSOLE NOTIFICATION]: {message}");

            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}