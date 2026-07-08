Console.Write("\x1b[2J");
Console.Write("\x1b[?25l");
while (true) 
{
    for (int i = 0 ;  i <= 8; i++)
    {    
        for (int b = 0; b <= 32 ; b++)
        {
            //System.Threading.Thread.Sleep(1);
            Console.Write($"\x1b[{i};{b}H0");
            //Console.Write("\x1B\a");
            
            
        }
    // Console.Write($"\n");
    }
    //sConsole.Write("\x1b[H");
}
Console.Write("\x1b[?25h");

