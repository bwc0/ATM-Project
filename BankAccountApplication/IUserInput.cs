
namespace PromptNameSpace
{
    public interface IUserInput
    {
        void GiveMessage(string message);
        int AskForInt(string message);
        decimal AskForDecimal(string message);
        string AskForString(string message);
    }
}
