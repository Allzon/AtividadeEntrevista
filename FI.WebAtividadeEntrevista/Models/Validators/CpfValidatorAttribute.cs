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
            // Remove pontos e traços, se houver
            cpf = cpf.Replace(".", "").Replace("-", "");

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex.: 111.111.111-11 é inválido)
            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            // Cálculo do primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            // Verifica se o primeiro dígito verificador está correto
            if (cpf[9] - '0' != primeiroDigitoVerificador)
                return false;

            // Cálculo do segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            resto = soma % 11;
            int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            // Verifica se o segundo dígito verificador está correto
            return cpf[10] - '0' == segundoDigitoVerificador;
        }
    }
}