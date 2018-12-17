using System;


namespace PromptNameSpace
{
    public class Prompt : IUserInput
    {

        public void GiveMessage(string message)
        {
            Console.WriteLine(message);
        }

        public int AskForInt(string message)
        {
            GiveMessage(message);
            return Convert.ToInt32(Console.ReadLine());
        }

        public decimal AskForDecimal(string message)
        {
            GiveMessage(message);
            return Convert.ToDecimal(Console.ReadLine());
        }

        public string AskForString(string message)
        {
            GiveMessage(message);
            return Console.ReadLine();
        }
    }
}
