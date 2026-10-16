using ToDoProject.Application.Interfaces;

namespace ToDoProject.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine("----- Example Email Service -----");
            Console.WriteLine($"To: {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");
            Console.WriteLine("---------------------------------");
            return Task.CompletedTask;
        }
    }
}
