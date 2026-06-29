  
  
public struct Ovi
{
    static void Main(string[] arguments) 
    string[] inst_lines
    {
        if (arguments.Length == 1 ) 
        {
           
            foreach (string argument in arguments) 
            {   
                string filepath = arguments[0];
                if (System.IO.Path.GetExtension(filepath) == ".ovi")
                {
                    try
                    {
                        string[] data = System.IO.File.ReadAllLines(filepath);
                        // System.IO.FileStream fs = System.IO.File.Open(filepath, System.IO.FileMode.Open);
                        Console.WriteLine($"File Opened {filepath}");
                        foreach (string word in data) 
                        {
                            Console.WriteLine($"{word}");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("\x1b[31mFailed to open the file\x1b[0m");
                    }
                    
                   
                }
                else
                {
                    Console.WriteLine("\x1b[31mWrong File type \x1b[0m");
                }
            }
        }
        else
        {
            Console.WriteLine($"\x1b[31mFalse arguments\x1b[0m,\nArgumet number : {arguments.Length}");
        }
        
    
    }
}
