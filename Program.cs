
  
public struct Ovi
{
    static void Main(string[] arguments) 
   
    {
    byte[] Memory = new byte[255];
    int cur_exec_line = 0;
    bool zero_flag = false;
    
    
    bool running = false;
    bool entered_main = false;
        
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
                                  // if (word != "")
                                   //{
                                        instructions[ins_Line][collum] = word;
                                   // }
                                    Console.WriteLine($"{instructions[ins_Line][collum]}");
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
                        //Console.WriteLine($"[{string.Join(", ", instructions)}]");
                        var registers = new Dictionary<string,Byte> 
                        {
                            ["A"] = 67,
                            ["B"] = 0,
                            ["C"] = 0,
                            ["D"] = 0,
                        };

                        byte Value_Parser(string input , params string[] flags)
                        {
                            bool binary_byte = false;
                            bool register = false ;
                            bool number = false ;
                            bool memory_adress = false ;
                            
                            foreach (string flag in flags )
                            {
                                switch(flag)
                                {
                                    case "bin":
                                        binary_byte = true;
                                        break;
                                    case "reg":
                                        register = true;
                                        break;
                                    case "num":
                                        number = true;
                                        break;
                                    case "mem":
                                        memory_adress = true;
                                        break;
                                    
                                }
                                
                            };
                            try
                            {
                                if ( registers.ContainsKey(input) )
                                {
                                    if (register == true)
                                    {
                                        return registers[input] ; 
                                    } 
                                    else
                                    {
                                       throw  new Exception("something went wrong ");
                                    }
                                }
                                else if ( int.Parse(input) is int )
                                {
                                    if (number == true )
                                    {
                                        return unchecked((byte) int.Parse(input));
                                    }
                                    else
                                    {
                                        throw  new Exception("something went wrong ");
                                    }
                                }
                                else if (input.StartsWith('$') )
                                {
                                    if (memory_adress == true)
                                    {
                                       // input = input.Replace("$","");
                                        byte byte_input = unchecked((byte) int.Parse(input.Substring(1))) ;
                                        return Memory[byte_input] ;
                                    }
                                    else
                                    {
                                        throw  new Exception("something went wrong ");
                                    }
                                }
                                else if (input.StartsWith("0b"))
                                {
                                    if ( binary_byte == true  )
                                    {
                                    
                                        byte byte_value = Convert.ToByte(input.Substring(2),2);
                                        return byte_value;
                                    }
                                    else
                                    {
                                        throw  new Exception("something went wrong ");
                                        
                                    }
                                }
                            
                            }
                            catch
                            {
                                Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                running = false;
                                return 69;
                            }
                             return 69;
                        };
                       
                       
                        Dictionary<string,Action> operations = new Dictionary<string,Action>()
                        {
                        
                            ["add"] = () =>
                            {   
                                try
                                {    
                                    Console.WriteLine($" {Value_Parser(instructions[cur_exec_line][1],"reg")},  { Value_Parser(instructions[cur_exec_line][2],"num")}, {Value_Parser(instructions[cur_exec_line][3],"bin")},{ Value_Parser(instructions[cur_exec_line][4],"mem")}" );
                                    
                                    // Value_Parser(instructions[cur_exec_line][1],"reg");
                                    // Value_Parser(instructions[cur_exec_line][2],"num");
                                    // Value_Parser(instructions[cur_exec_line][3],"bin");
                                    // Value_Parser(instructions[cur_exec_line][4],"mem");
                                    
                                    // int left_value = int.Parse(instructions[cur_exec_line][1]);
                                    // int right_value = int.Parse(instructions[cur_exec_line][2]);
                                    // byte a = unchecked((byte)left_value);
                                    // byte b = unchecked((byte)right_value);
                                    // byte result = unchecked( (byte)( left_value + right_value) ) ;
                                    // Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, add : res - {result} ");
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                            },
                             ["["] = () =>
                             {
                                Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, [ : entered_main - {true} ");
                                entered_main = true;

                             },
                             ["]"] = () =>
                             {
                                Console.WriteLine($"Line {cur_exec_line + 1}, ins exec, ] : program end");
                                entered_main = false;
                                running = false;

                             },
                             
                            
                        };
        

                        running = true;
                        void Runtime() 
                        {
                            
                            while (running)
                            {
                             
                                try
                                {
                                    
                                    Console.WriteLine($"exec, line  {cur_exec_line + 1}");
                                    if (operations.ContainsKey( instructions[cur_exec_line][0] ) )
                                    {
                                        operations[ instructions[cur_exec_line][0] ]();
                                    }
                                    
                                    if (cur_exec_line < instructions.Length  )
                                    {

                                        cur_exec_line += 1;
                                      
                                    }
                                    else
                                    {
                                        running = false;
                                    
                                    }
                                    
                                }
                                catch
                                {
                                 
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    
                                    
                                }
                                
                            }
                        }
                       
                        Runtime();
                       
                        Console.WriteLine("Program stopped running");
                        Console.WriteLine($"program length {instructions.Length}");
                       
                        
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
