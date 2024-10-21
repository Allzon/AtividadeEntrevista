using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models.Validators
{
    public class CpfValidatorAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            return ValidarCPF(value.ToString());
        }

        public static bool ValidarCPF(string cpf)
        {
            // Remove pontos e tra�os, se houver
            cpf = cpf.Replace(".", "").Replace("-", "");

            // Verifica se o CPF tem 11 d�gitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os d�gitos s�o iguais (ex.: 111.111.111-11 � inv�lido)
            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            // C�lculo do primeiro d�gito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            // Verifica se o primeiro d�gito verificador est� correto
            if (cpf[9] - '0' != primeiroDigitoVerificador)
                return false;

            // C�lculo do segundo d�gito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            resto = soma % 11;
            int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            // Verifica se o segundo d�gito verificador est� correto
            return cpf[10] - '0' == segundoDigitoVerificador;
        }
    }
}