using System.Net.Mail;

namespace ToDoPlatform.Helpers;

public static class Helper // Classe Static: não precisa ser instânciada (não precisa de um objeto na memória)
{
    public static bool IsValidEmail(string email)
    {
        try // tenta fazer o bloco de código
        {
            MailAddress mail = new(email);
            return true;
        }
        catch (FormatException) // Vem aqui se der erro
        {
            return false;
        }
    }
}