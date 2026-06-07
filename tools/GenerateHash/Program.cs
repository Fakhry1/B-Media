// BMedia — BCrypt Hash Generator
// Run: dotnet run --project tools/GenerateHash
// Then copy the output hash into SeedAdmin.sql

using BCrypt.Net;

Console.Write("Enter password to hash (leave blank for 'Admin@123'): ");
var input = Console.ReadLine();
var password = string.IsNullOrWhiteSpace(input) ? "Admin@123" : input;

var hash = BCrypt.HashPassword(password, workFactor: 12);

Console.WriteLine();
Console.WriteLine($"Password : {password}");
Console.WriteLine($"Hash     : {hash}");
Console.WriteLine();
Console.WriteLine("-- Copy this line into SeedAdmin.sql:");
Console.WriteLine($"    '$2a$...',   -- replace with above hash");
Console.WriteLine();
Console.WriteLine("-- Or run this SQL (replace <HASH> with the hash above):");
Console.WriteLine($"UPDATE users SET password_hash = '{hash}' WHERE username = 'admin';");
