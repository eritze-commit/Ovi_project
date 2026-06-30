
  
public struct Ovi
{
    static void Main(string[] arguments) 
   
    {
    byte[] Memory = new byte[1000];
    int cur_exec_line = 0;
    var registers = new Dictionary<string,Byte> 
    {
         ["A"] = 0,
         ["B"] = 0,
         ["C"] = 0,
         ["D"] = 0,
    };
    
    
    bool running = false;
        
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
                        //System.IO.FileStream fs = System.IO.File.Open(filepath, System.IO.FileMode.Open);
                        
                        string[][] instructions = new string[data.Length][];
                        Console.WriteLine($"File Opened {filepath}");
                        try
                        {
                            int ins_Line = 0;
                            foreach (string line in data) 
                            {
                                string[] line_split = line.Split(' ');
                        
                                instructions[ins_Line] = new string[line_split.Length];
                                int collum = 0;
                              
                                foreach (string word in line_split) 
                                {
                                   
                                    instructions[ins_Line][collum] = word;
                                    //Console.WriteLine($"{instructions[ins_Line][collum]}");
                                    //Console.WriteLine($"{word}");
                                    collum += 1 ;
                                }
                                ins_Line += 1;
                            }
                        }
                        catch (Exception ex)
                        {
                             Console.WriteLine($"\x1b[31mFailed to parse the file\x1b[0m {ex}");
                        }
                       
                       
                        Dictionary<string,Action> operations = new Dictionary<string,Action>()
                        {
                        
                            ["add"] = () =>
                            {   
                                try
                                {
                                    int register = int.Parse(instructions[cur_exec_line][1]);
                                    int right_value = int.Parse(instructions[cur_exec_line][2]);
                                    int result = register + right_value;
                                    Console.WriteLine($"ins exec, add : res - {result} ");
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line}\x1b[0m");
                                    running = false;
                                }
                            },
                        };
        

                        running = true;
                        void Runtime() 
                        {
                            
                            while (running)
                            {
                             
                                try
                                {
                               
                                    operations[ instructions[cur_exec_line][0] ]();
                                }
                                catch
                                {
                                 
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line}\x1b[0m");
                                   
                                    running = false;
                                    
                                }
                                
                                cur_exec_line += 1;
                            }
                        }
                       
                        Runtime();
                        Console.WriteLine("Program stopped running");
                       
                        
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
