namespace GoodHamburger.Aplicacao.DTOs
{
    public class RespostaGeral<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T? Dados { get; set; }
        public List<string>? Erros { get; set; }

        public static RespostaGeral<T> Ok(T dados, string mensagem = "Sucesso")
        {
            return new RespostaGeral<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados,
                Erros = null
            };
        }

        public static RespostaGeral<T> Falha(string mensagem, List<string>? erros = null)
        {
            return new RespostaGeral<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Dados = default,
                Erros = erros
            };
        }
    }
}