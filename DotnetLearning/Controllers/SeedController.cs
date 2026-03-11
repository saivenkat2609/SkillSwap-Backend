using DotnetLearning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetLearning.Controllers
{
    [ApiController]
    [Route("/api/seed")]
    public class SeedController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _db;

        public SeedController(UserManager<ApplicationUser> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            // --- TEACHERS ---
            var teacher1 = await GetOrCreateUser("alice@skillswap.com", "Alice", "Johnson", "Test@1234", "Teacher");
            var teacher2 = await GetOrCreateUser("bob@skillswap.com", "Bob", "Smith", "Test@1234", "Teacher");

            // --- STUDENTS ---
            var student1 = await GetOrCreateUser("charlie@skillswap.com", "Charlie", "Brown", "Test@1234", "Student");
            var student2 = await GetOrCreateUser("diana@skillswap.com", "Diana", "Prince", "Test@1234", "Student");

            // --- CATEGORIES (already seeded via migrations, but ensure they exist) ---
            var categories = await _db.SkillCategories.ToListAsync();

            // --- SKILLS ---
            if (!await _db.Skills.AnyAsync())
            {
                var skills = new List<Skill>
                {
                    // Programming
                    new() { Title = "React for Beginners", Description = "Learn React from scratch. We'll cover components, hooks, state management, and build real projects together.", PricePerHour = 800, TeacherId = teacher1.Id, TeacherName = "Alice Johnson", Rating = 4.8, CategoryId = 1, NumberOfReviews = 24, CreatedAt = DateTime.UtcNow.AddDays(-60), IsActive = true },
                    new() { Title = "ASP.NET Core API Development", Description = "Build production-ready REST APIs with ASP.NET Core 8. Covers Entity Framework, Identity, and deployment.", PricePerHour = 1000, TeacherId = teacher1.Id, TeacherName = "Alice Johnson", Rating = 4.5, CategoryId = 1, NumberOfReviews = 18, CreatedAt = DateTime.UtcNow.AddDays(-45), IsActive = true },
                    new() { Title = "Python Data Science", Description = "Master Python for data analysis using Pandas, NumPy, and Matplotlib. Great for beginners.", PricePerHour = 750, TeacherId = teacher2.Id, TeacherName = "Bob Smith", Rating = 4.9, CategoryId = 1, NumberOfReviews = 41, CreatedAt = DateTime.UtcNow.AddDays(-90), IsActive = true },
                    new() { Title = "JavaScript & TypeScript", Description = "Deep dive into modern JavaScript and TypeScript. Async/await, generics, and best practices.", PricePerHour = 700, TeacherId = teacher2.Id, TeacherName = "Bob Smith", Rating = 4.3, CategoryId = 1, NumberOfReviews = 15, CreatedAt = DateTime.UtcNow.AddDays(-30), IsActive = true },

                    // Design
                    new() { Title = "Figma UI/UX Design", Description = "Learn to design beautiful interfaces in Figma. From wireframes to high-fidelity prototypes.", PricePerHour = 600, TeacherId = teacher1.Id, TeacherName = "Alice Johnson", Rating = 4.7, CategoryId = 2, NumberOfReviews = 32, CreatedAt = DateTime.UtcNow.AddDays(-50), IsActive = true },
                    new() { Title = "Brand Identity Design", Description = "Create professional brand identities. Logo design, colour theory, typography, and brand guidelines.", PricePerHour = 900, TeacherId = teacher2.Id, TeacherName = "Bob Smith", Rating = 4.6, CategoryId = 2, NumberOfReviews = 11, CreatedAt = DateTime.UtcNow.AddDays(-20), IsActive = true },

                    // Marketing
                    new() { Title = "SEO Fundamentals", Description = "Rank higher on Google. Learn on-page SEO, keyword research, backlinks, and content strategy.", PricePerHour = 500, TeacherId = teacher1.Id, TeacherName = "Alice Johnson", Rating = 4.2, CategoryId = 3, NumberOfReviews = 9, CreatedAt = DateTime.UtcNow.AddDays(-15), IsActive = true },
                    new() { Title = "Social Media Marketing", Description = "Grow your brand on Instagram, LinkedIn, and TikTok with proven strategies and content frameworks.", PricePerHour = 550, TeacherId = teacher2.Id, TeacherName = "Bob Smith", Rating = 4.4, CategoryId = 3, NumberOfReviews = 20, CreatedAt = DateTime.UtcNow.AddDays(-35), IsActive = true },

                    // Business
                    new() { Title = "Excel for Business", Description = "Master Excel formulas, pivot tables, and dashboards to boost your productivity at work.", PricePerHour = 450, TeacherId = teacher1.Id, TeacherName = "Alice Johnson", Rating = 4.1, CategoryId = 4, NumberOfReviews = 7, CreatedAt = DateTime.UtcNow.AddDays(-10), IsActive = true },
                    new() { Title = "Financial Modelling", Description = "Build professional financial models in Excel. Valuation, DCF, and sensitivity analysis.", PricePerHour = 1200, TeacherId = teacher2.Id, TeacherName = "Bob Smith", Rating = 4.9, CategoryId = 4, NumberOfReviews = 28, CreatedAt = DateTime.UtcNow.AddDays(-70), IsActive = true },

                    // Personal Development
                    new() { Title = "Public Speaking Coaching", Description = "Overcome fear of public speaking and deliver confident, engaging presentations.", PricePerHour = 650, TeacherId = teacher1.Id, TeacherName = "Alice Johnson", Rating = 5.0, CategoryId = 5, NumberOfReviews = 14, CreatedAt = DateTime.UtcNow.AddDays(-25), IsActive = true },
                    new() { Title = "Productivity Systems", Description = "Build a personalised productivity system using GTD, Notion, and time-blocking techniques.", PricePerHour = 400, TeacherId = teacher2.Id, TeacherName = "Bob Smith", Rating = 4.6, CategoryId = 5, NumberOfReviews = 19, CreatedAt = DateTime.UtcNow.AddDays(-40), IsActive = true },
                };

                _db.Skills.AddRange(skills);
                await _db.SaveChangesAsync();
            }

            var allSkills = await _db.Skills.ToListAsync();
            var reactSkill = allSkills.First(s => s.Title == "React for Beginners");
            var pythonSkill = allSkills.First(s => s.Title == "Python Data Science");
            var figmaSkill = allSkills.First(s => s.Title == "Figma UI/UX Design");

            // --- BOOKINGS ---
            if (!await _db.Bookings.AnyAsync())
            {
                var bookings = new List<Booking>
                {
                    // Upcoming confirmed
                    new() { SkillId = reactSkill.SkillId, StudentId = student1.Id, TeacherId = teacher1.Id, ScheduledAt = DateTime.UtcNow.AddDays(3).Date.AddHours(10), DurationMinutes = 60, TotalPrice = 800, Status = BookingStatus.Confirmed, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                    new() { SkillId = pythonSkill.SkillId, StudentId = student1.Id, TeacherId = teacher2.Id, ScheduledAt = DateTime.UtcNow.AddDays(7).Date.AddHours(14), DurationMinutes = 90, TotalPrice = 1125, Status = BookingStatus.Pending, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                    new() { SkillId = figmaSkill.SkillId, StudentId = student2.Id, TeacherId = teacher1.Id, ScheduledAt = DateTime.UtcNow.AddDays(5).Date.AddHours(11), DurationMinutes = 60, TotalPrice = 600, Status = BookingStatus.Confirmed, CreatedAt = DateTime.UtcNow.AddDays(-3) },

                    // Completed
                    new() { SkillId = reactSkill.SkillId, StudentId = student1.Id, TeacherId = teacher1.Id, ScheduledAt = DateTime.UtcNow.AddDays(-10).Date.AddHours(10), DurationMinutes = 60, TotalPrice = 800, Status = BookingStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-15) },
                    new() { SkillId = pythonSkill.SkillId, StudentId = student2.Id, TeacherId = teacher2.Id, ScheduledAt = DateTime.UtcNow.AddDays(-5).Date.AddHours(9), DurationMinutes = 120, TotalPrice = 1500, Status = BookingStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-8) },

                    // Cancelled
                    new() { SkillId = figmaSkill.SkillId, StudentId = student1.Id, TeacherId = teacher1.Id, ScheduledAt = DateTime.UtcNow.AddDays(-2).Date.AddHours(15), DurationMinutes = 60, TotalPrice = 600, Status = BookingStatus.Cancelled, CreatedAt = DateTime.UtcNow.AddDays(-7) },
                };

                _db.Bookings.AddRange(bookings);
                await _db.SaveChangesAsync();
            }

            // --- REVIEWS ---
            if (!await _db.Reviews.AnyAsync())
            {
                var completedBookings = await _db.Bookings
                    .Where(b => b.Status == BookingStatus.Completed)
                    .ToListAsync();

                if (completedBookings.Count >= 2)
                {
                    var reviews = new List<Review>
                    {
                        new() { SkillId = reactSkill.SkillId, ReviewerId = student1.Id, TeacherId = teacher1.Id, BookingId = completedBookings[0].BookingId, Rating = 5, Content = "Alice is an amazing teacher! She explained React hooks so clearly. I finally understand useEffect. Highly recommend.", CreatedAt = DateTime.UtcNow.AddDays(-9) },
                        new() { SkillId = pythonSkill.SkillId, ReviewerId = student2.Id, TeacherId = teacher2.Id, BookingId = completedBookings[1].BookingId, Rating = 4, Content = "Bob's Python session was really helpful. Great examples and very patient with questions. Would book again.", CreatedAt = DateTime.UtcNow.AddDays(-4) },
                    };

                    _db.Reviews.AddRange(reviews);
                    await _db.SaveChangesAsync();
                }
            }

            return Ok(new
            {
                message = "Database seeded successfully!",
                teachers = new[] {
                    new { email = "alice@skillswap.com", password = "Test@1234", role = "Teacher" },
                    new { email = "bob@skillswap.com", password = "Test@1234", role = "Teacher" }
                },
                students = new[] {
                    new { email = "charlie@skillswap.com", password = "Test@1234", role = "Student" },
                    new { email = "diana@skillswap.com", password = "Test@1234", role = "Student" }
                }
            });
        }

        private async Task<ApplicationUser> GetOrCreateUser(string email, string firstName, string lastName, string password, string role)
        {
            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null) return existing;

            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true,
                IsOnboardingComplete = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception($"Failed to create {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            await _userManager.AddToRoleAsync(user, role);
            return user;
        }
    }
}
