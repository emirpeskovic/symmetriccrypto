using System.Security.Cryptography;

while (true)
{
    Console.Clear();
    
    Console.WriteLine("Select a crypto algorithm:");
    Console.WriteLine("1. AES (CSP 128 bit)");
    Console.WriteLine("2. AES (CSP 256 bit)");
    Console.WriteLine("3. AES (Managed 128 bit)");
    Console.WriteLine("4. AES (Managed 256 bit)");
    Console.WriteLine("5. Rijndael (Managed 128 bit)");
    Console.WriteLine("6. Rijndael (Managed 256 bit)");
    Console.WriteLine("7. DES (CSP 56 bit)");
    Console.WriteLine("8. Triple DES (CSP 168 bit)");

    // We get the user's choice
    var option = Console.ReadKey();
    
    // Delete the line
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    
    // Make sure the user selected a valid option
    if (option.Key is < ConsoleKey.D1 or > ConsoleKey.D8)
    {
        Console.WriteLine("Invalid option, press any key to try again.");
        Console.ReadKey();
        continue;
    }

    // Create the algorithm
    SymmetricAlgorithm algorithm = option.Key switch
    {
        ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.D3 or ConsoleKey.D4 => Aes.Create(),
        ConsoleKey.D5 or ConsoleKey.D6 => Rijndael.Create(),
        ConsoleKey.D7 => DES.Create(),
        ConsoleKey.D8 => TripleDES.Create()
    };
    
    // Set the key size
    algorithm.KeySize = option.Key switch
    {
        ConsoleKey.D1 or ConsoleKey.D3 or ConsoleKey.D5 => 128,
        ConsoleKey.D2 or ConsoleKey.D4 or ConsoleKey.D6 => 256,
        ConsoleKey.D7 => 64, // 56 bit key + 8 bit parity
        ConsoleKey.D8 => 192 // 168 bit key + 24 bit parity
    };
    
    // Generate the key and IV
    algorithm.GenerateKey();
    algorithm.GenerateIV();
    
    // Print the key and IV
    Console.WriteLine("Key: {0}", Convert.ToBase64String(algorithm.Key));
    Console.WriteLine("IV: {0}", Convert.ToBase64String(algorithm.IV));
    
    // Get the user's input
    Console.WriteLine("Enter the text to encrypt:");
    var input = Console.ReadLine();
    
    // Create the encryptor
    var encryptor = algorithm.CreateEncryptor();
    
    // Encrypt the input
    var encrypted = encryptor.TransformFinalBlock(
        input.ToCharArray().Select(c => (byte)c).ToArray(),
        0,
        input.Length);
    
    // Print the encrypted text
    Console.WriteLine("Encrypted: {0}", Convert.ToBase64String(encrypted));
    
    // Create the decryptor
    var decryptor = algorithm.CreateDecryptor();
    
    // Decrypt the input
    var decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
    
    // Print the decrypted text
    Console.WriteLine("Decrypted: {0}", new string(decrypted.Select(b => (char)b).ToArray()));
    
    // Wait for the user to press a key
    Console.WriteLine("Press any key to try again.");
    Console.ReadKey();
}