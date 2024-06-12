namespace FIAP.TechChalenge.EpicCollections.Application.Exceptions;
public class CustomAuthenticationException : ApplicationException
{
    public CustomAuthenticationException(string message) : base(message) { }
}
