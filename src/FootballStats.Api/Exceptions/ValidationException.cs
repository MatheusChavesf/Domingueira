namespace FootballStats.Api.Exceptions;

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Erros { get; }

    public ValidationException(string mensagem)
        : base(mensagem)
    {
        Erros = new Dictionary<string, string[]>();
    }

    public ValidationException(string mensagem, Dictionary<string, string[]> erros)
        : base(mensagem)
    {
        Erros = erros;
    }

    public ValidationException(string campo, string detalhe)
        : base(detalhe)
    {
        Erros = new Dictionary<string, string[]>
        {
            { campo, new[] { detalhe } }
        };
    }
}
