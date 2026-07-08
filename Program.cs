
  
public struct Ovi
{
    static void Main(string[] arguments) 
   
    {
   
    byte[] Memory = new byte[256];
   // byte[] Special_memory = new byte[256];
    //byte[] Stack = new byte[25];
    int  cur_exec_line = 0;
    //byte stack_pointer = 0;
    bool zero_flag = false;
    bool cond_flag = false;
    bool silent = false;
    bool visual = false;
    byte speed = 50;
       
    int cursorY = 0;
    int viewportY = 0;

    int screenHeight = Console.WindowHeight;
    
    
    bool running = false;
    bool entered_main = false;
        if (arguments.Length == 1 ) 
        {
            if (arguments[0].Contains("-help") )
            {
                Console.WriteLine("______________________________________________________________\nWelcome to OVI\n./excecutable (path to file with .ovi extension)\nOptional falgs\n -s = silent flag only print output\n -viz = visual code representation");
            }
        } 
        if (arguments.Length >= 1 ) 
        {    
            if (arguments.Length >= 2)
            {
                if (arguments[1]  == "-s" )
                    {
                        silent = true;
                        
                    }
                else if (arguments[1]  == "-viz" )
                {
                    visual = true;
                    silent = true;
                    
                }
            }
            if (arguments.Length  ==  0)
            {
                Console.WriteLine("Welcome to OVI/n/./excecutable (path to file with .ovi extension)\nOptional falgs\n -s = silent flag only print output\n -viz = visual code representation");
            }
            
             Console.Write("\x1b[2J\x1b[H");
            for (int i = 0; i < 1; i++)
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
                                   if (visual)
                                   {
                                        Console.Write($"{instructions[ins_Line][collum]}");
                                   }
                                    //Console.WriteLine($"{word}");
                                    collum += 1 ;
                                }
                                ins_Line += 1;
                                if (visual)
                                {
                                    Console.Write($"\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                             Console.WriteLine($"\x1b[31mFailed to parse the file\x1b[0m {ex}");
                        }
                        //Console.WriteLine($"[{string.Join(", ", instructions)}]");
                        if (visual)
                        {
                            Console.WriteLine("______________________________________________________________\n");
                        }
                        var registers = new Dictionary<string,Byte> 
                        {
                            ["A"] = 0,
                            ["B"] = 0,
                            ["C"] = 0,
                            ["D"] = 0,
                        };
                       
                        (byte val, byte opt) Value_Parser(string input , params string[] flags)
                        {
                            bool binary_byte = false;
                            bool register = false ;
                            bool number = false ;
                            bool memory_adress = false ;
                            bool special_mem = false ;
                            
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
                                    case "smm" :
                                        special_mem = true;
                                        break;
                                     
                                }
                                
                            };
                            try
                            {
                                if ( registers.ContainsKey(input) )
                                {
                                    if (register == true)
                                    {
                                        return (registers[input],0) ; 
                                    } 
                                    else
                                    {
                                       throw  new Exception("something went wrong ");
                                    }
                                }
                                else if (input.StartsWith("$") )
                                {
                                   
                                    if (memory_adress == true)
                                    {
                                       // input = input.Replace("$","");
                                        byte byte_value = Value_Parser(input.Substring(1),"num","reg","bin").val;
                                       // byte byte_input = unchecked((byte) int.Parse(input.Substring(1))) ;
                                        return (Memory[byte_value],byte_value) ;
                                    }
                                    else
                                    {
                                        throw  new Exception("something went wrong ");
                                    }
                                }
                                else if (input.StartsWith("0b") )
                                {
                                    
                                    if ( binary_byte)
                                    {
                                        
                                        byte byte_value = Convert.ToByte(input.Substring(2),2);
                                        return (byte_value,0);
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
                                        return (unchecked((byte) int.Parse(input)),0);
                                    }
                                    else
                                    {
                                        throw  new Exception("something went wrong ");
                                    }
                                }
                            
                            }
                            catch 
                            {
                                Console.WriteLine($"\x1b[32m{input}\x1b[0m");
                                Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                running = false;
                                return (67,67);
                               
                            }
                             return (67,67);
                        };
                       
                       
                        Dictionary<string,Action> operations = new Dictionary<string,Action>()
                        {
                        
                            ["add"] = () =>
                            {   
                                try
                                {    
                                    //Console.WriteLine($" {Value_Parser(instructions[cur_exec_line][1],"reg")},  { Value_Parser(instructions[cur_exec_line][2],"num")}, {Value_Parser(instructions[cur_exec_line][3],"bin")},{ Value_Parser(instructions[cur_exec_line][4],"mem")}" );
                                    
                                    //Value_Parser(instructions[cur_exec_line][1],"reg");
                                    //Value_Parser(instructions[cur_exec_line][2],"num");
                                    //Value_Parser(instructions[cur_exec_line][3],"bin");
                                    //Value_Parser(instructions[cur_exec_line][4],"mem");
                                    string register = instructions[cur_exec_line][1].Replace(",","");
                                    byte left_value =  Value_Parser(register,"reg").val;
                                    byte right_value = Value_Parser(instructions[cur_exec_line][2],"num","bin","reg").val;
                                    //byte a = unchecked((byte)left_value);
                                   // byte b = unchecked((byte)right_value);
                                    int result = left_value + right_value;
                                    byte Byte_result = unchecked( (byte)result) ;
                                    registers[register] = Byte_result;
                                    
                                    if (result == 0) 
                                    {
                                        zero_flag = true;
                                    } 
                                    else
                                    {
                                         zero_flag = false;
                                    }
                                    if (result > 255 )
                                    {
                                        cond_flag = true;
                                    }
                                    else
                                    {
                                        cond_flag = false;
                                    }

                                      if (!silent )
                                    {
                                        Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, add : res - {Byte_result} ");
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                            },
                             ["sub"] = () =>
                             {
                                try
                                {    
                                    string register = instructions[cur_exec_line][1].Replace(",","");
                                    byte left_value =  Value_Parser(register,"reg").val;
                                    byte right_value = Value_Parser(instructions[cur_exec_line][2],"num","bin","reg").val;
                                    int result = left_value - right_value;
                                    byte byte_result = unchecked( (byte) result ) ;
                                    registers[register] = byte_result;

                                     if (!silent )
                                    {
                                        Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, sub : res - {byte_result} ");
                                    }
                                    if (result == 0) 
                                    {
                                        zero_flag = true;
                                    } 
                                    else
                                    {
                                         zero_flag = false;
                                    }
                                    if (result < 0 )
                                    {
                                        cond_flag = true;
                                    }
                                    else
                                    {
                                        cond_flag = false;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                             ["print"] = () =>
                             {
                                try
                                {
                                    byte result = Value_Parser(instructions[cur_exec_line][1],"num","bin","mem","reg").val;
                                    Console.WriteLine($"{result}");
                                     if (!silent )
                                    {
                                        Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, print : res - {result} ");
                                    }
                                   
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                              ["store"] = () =>
                             {
                                try
                                {    
                                    string memory = instructions[cur_exec_line][1].Replace(",","");
                                    byte left_value =  Value_Parser(memory,"mem").val;
                                    byte right_value = Value_Parser(instructions[cur_exec_line][2],"num","bin","reg").val;
                                    //byte memory_adress = unchecked((byte) int.Parse(memory.Substring(1))) ;
                                    byte memory_adress =  Value_Parser(memory,"mem").opt;
                                    Memory[memory_adress] = right_value;

                                     if (!silent )
                                    {
                                        Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, store : stored to adr - ${memory_adress}, val - {right_value}");
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                              ["load"] = () =>
                             {
                                try
                                {    
                                    string reg = instructions[cur_exec_line][1].Replace(",","");
                                    byte left_value =  Value_Parser(reg,"reg").val;
                                    byte right_value = Value_Parser(instructions[cur_exec_line][2],"num","bin","reg","mem").val;
                                    registers[reg] = right_value;

                                     if (!silent )
                                    {
                                        Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, load : load to reg - {reg}, val - {right_value}");
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                             ["jmp"] = () =>
                             {
                                try
                                {    
                                    int old_exec_line = cur_exec_line;
                                    string jmp_operator = instructions[cur_exec_line][1];
                                    byte right_value = Value_Parser(instructions[cur_exec_line][1].Substring(1),"num","bin","reg").val;

                                    if (jmp_operator.StartsWith("+")) 
                                    {
                                        cur_exec_line += right_value - 1   ; 
                                    }
                                    else if (jmp_operator.StartsWith("-")) 
                                    {
                                         cur_exec_line -= right_value + 1  ; 
                                    }
                                    else
                                    {
                                        throw new Exception("Wrong stuff ");
                                    }
                                   
                                     if (!silent )
                                    {
                                        Console.WriteLine($"Line {old_exec_line + 1 },value {right_value}, ins exec, jmp : jump to line - {cur_exec_line + 2}");
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                             ["jz"] = () =>
                             {
                                try
                                {
                                    int old_exec_line = cur_exec_line;
                                    if (zero_flag) 
                                    {
                                            
                                        string jmp_operator = instructions[cur_exec_line][1];
                                        byte right_value = Value_Parser(instructions[cur_exec_line][1].Substring(1),"num","bin","reg").val;
                                        if (jmp_operator.StartsWith("+")) 
                                        {
                                            cur_exec_line += right_value - 1     ; 
                                        }
                                        else if (jmp_operator.StartsWith("-")) 
                                        {
                                            cur_exec_line -= right_value + 1  ; 
                                        }
                                        else
                                        {
                                            throw new Exception("Wrong stuff ");
                                        }
                                        
                                    }
                                    if (!silent )
                                        {
                                            Console.WriteLine($"Line {old_exec_line + 1 }, ins exec, jz : jump if zero = {zero_flag} - {cur_exec_line + 2}");
                                        }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                             ["jcn"] = () =>
                             {
                                try
                                {
                                    int old_exec_line = cur_exec_line;
                                    if (cond_flag) 
                                    {
                                            
                                        string jmp_operator = instructions[cur_exec_line][1];
                                        byte right_value = Value_Parser(instructions[cur_exec_line][1].Substring(1),"num","bin","reg").val;
                                        if (jmp_operator.StartsWith("+")) 
                                        {
                                            cur_exec_line += right_value - 1     ; 
                                        }
                                        else if (jmp_operator.StartsWith("-")) 
                                        {
                                            cur_exec_line -= right_value + 1  ; 
                                        }
                                        else
                                        {
                                            throw new Exception("Wrong stuff ");
                                        }
                                        
                                    }
                                    if (!silent )
                                        {
                                            Console.WriteLine($"Line {old_exec_line + 1 }, ins exec, jcn : jump conditional = {cond_flag} - {cur_exec_line + 2}");
                                        }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                             
                             ["jnz"] = () =>
                             {
                                try
                                {
                                    int old_exec_line = cur_exec_line;
                                  
                                    if (!zero_flag) 
                                    {
                                        
                                        string jmp_operator = instructions[cur_exec_line][1];
                                        byte right_value = Value_Parser(instructions[cur_exec_line][1].Substring(1),"num","bin","reg").val;
                                        if (jmp_operator.StartsWith("+")) 
                                        {
                                            cur_exec_line += right_value - 1     ; 
                                        }
                                        else if (jmp_operator.StartsWith("-")) 
                                        {
                                            cur_exec_line -= right_value + 1  ; 
                                        }
                                        else
                                        {
                                            throw new Exception("Wrong stuff ");
                                        }
                                    
                                    }
                                    if (!silent )
                                        {
                                            Console.WriteLine($"Line {old_exec_line + 1 }, ins exec, jnz : zero_flag = {zero_flag}, jump if not zero - {cur_exec_line + 2 }");
                                        }
                                }
                                catch
                                {
                                    Console.WriteLine($"\x1b[31mRUNTIME ERROR ! code failed on line {cur_exec_line + 1 }\x1b[0m");
                                    running = false;
                                }
                             },
                             ["halt"] = () => 
                             {
                                if (!silent )
                                {
                                    Console.WriteLine($"Line {cur_exec_line + 1}, ins exec, halt : program end");
                                }
                                entered_main = false;
                                running = false;
                             },
                             ["sleep"] = () => 
                             {
                                byte right_value = Value_Parser(instructions[cur_exec_line][1],"num","bin","reg").val;
                                speed = right_value;
                                if (!silent )
                                {
                                    Console.WriteLine($"Line {cur_exec_line + 1}, ins exec, sleep :{speed}ms");
                                }
                             },
                             ["["] = () =>
                             {
                                if (!silent )
                                {
                                    Console.WriteLine($"Line {cur_exec_line + 1 }, ins exec, [ : entered_main - {true} ");
                                }
                                entered_main = true;

                             },
                             
                             ["]"] = () =>
                             {
                                if (!silent )
                                {
                                    Console.WriteLine($"Line {cur_exec_line + 1}, ins exec, ] : program end");
                                }
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
                                   
                                    if (operations.ContainsKey( instructions[cur_exec_line][0] ) )
                                    {
                                        operations[ instructions[cur_exec_line][0] ]();
                                    }
                                    else
                                    {
                                          if (!silent )
                                        {
                                            Console.WriteLine($"exec, line  {cur_exec_line + 1}");
                                        }
                                    }
                                    if (visual)
                                    {
                       
                                       Console.Write($"\x1b[{cur_exec_line+3};1H") ;
                                        //Console.SetCursorPosition(0, cur_exec_line);
                                        //Console.Write($"\x1b[1A");
                                        
                                    }
                                   
                                    
                                    if (cur_exec_line < instructions.Length  )
                                    {

                                        cur_exec_line += 1;
                                      
                                    }
                                    else
                                    {
                                        running = false;
                                    
                                    }
                                     Thread.Sleep(speed);
                                    
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
                                        Console.WriteLine("______________________________________________________________\nWelcome to OVI\n./excecutable (path to file with .ovi extension)\nOptional falgs\n -s = silent flag only print output\n -viz = visual code representation");

                    }
                    
                   
                }
                else
                {
                    Console.WriteLine("\x1b[31mWrong File type \x1b[0m");
                                    Console.WriteLine("______________________________________________________________\nWelcome to OVI\n./excecutable (path to file with .ovi extension)\nOptional falgs\n -s = silent flag only print output\n -viz = visual code representation");

                }
            }
        }
        else
        {
            Console.WriteLine($"\x1b[31mFalse arguments\x1b[0m,\nArgumet number : {arguments.Length}");
                            Console.WriteLine("______________________________________________________________\nWelcome to OVI\n./excecutable (path to file with .ovi extension)\nOptional falgs\n -s = silent flag only print output\n -viz = visual code representation");

        }
        
    
    }
}
