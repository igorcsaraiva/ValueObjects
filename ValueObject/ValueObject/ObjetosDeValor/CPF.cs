using System;
using System.Text;

namespace ValueObject.ObjetosDeValor
{
    public class CPF : IEquatable<CPF>
    {
        private const int NumeroPadraoDaDivisao = 11;
        private const int TamanhoDoCpfComMascara = 14;
        private const int TamanhoDoCpfSemMascara = 11;
        private const char ponto = '.';
        private const char traco = '-';

        private string cpf;

        /// <summary>
        /// Remoção da máscara
        /// </summary>
        /// <returns>CPF sem a máscara Ex: "66950845057"</returns>
        public string CpfSemMascara => cpf;
        public string Cpf
        {
            get => cpf;
            private set
            {
                if (ValidarCPF(value))
                    cpf = RetirarMascaraCPF(value);
                else
                    throw new Exception("Cpf inválido");
            }
        }

        public CPF()
        {
        }
        private CPF(string cpf) => Cpf = cpf;
        /// <summary>
        /// Permite a atribuição de uma string diretamente com o operador de atribuição. Ex: public CPF cpf { get; set; } / Jose.CPF = "669.508.450-57"
        /// </summary>
        /// <param name="value">CPF a ser instanciado</param>
        /// <exception>CPF inválido</exception>
        public static implicit operator CPF(string value) => new(value);

        #region Validação de CPF
        /// <summary>
        /// Validçao de um CPF
        /// Obs: A validação não se trata de esse CPF estar atrelado a um CPF existente, e sim se os digtos informados conferem com os digitos verificadores
        ///</summary>
        /// <param name="cpf"> CPF a ser validado</param>
        /// <returns>falso se um CPF não for valido</returns>
        public static bool ValidarCPF(string cpf)
        {
            Span<int> NumerosDoCpf = stackalloc int[11];

            if ((cpf?.Length == TamanhoDoCpfSemMascara || (cpf?.Length == TamanhoDoCpfComMascara && cpf[3] == ponto && cpf[7] == ponto && cpf[11] == traco)) && VerificaSeTodosOsDigitosSaoDiferentes(cpf))
            {
                int j = 0;
                for (int i = 0; i < cpf.Length; i++)
                {
                    if (cpf[i] != ponto && cpf[i] != traco)
                    {
                        NumerosDoCpf[j] = cpf[i] - '0';
                        j++;
                    }
                }
            }
            else
                return false;

            ReadOnlySpan<int> MultiplicadoresParaValidacaoPrimeiroDigito = stackalloc int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            ReadOnlySpan<int> MultiplicadoresParaValidacaoSegundoDigito = stackalloc int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var primeiroDigitoVerificador = CalcularDigitosVerificadores(NumerosDoCpf, MultiplicadoresParaValidacaoPrimeiroDigito);
            var segundoDigitoVerificador = CalcularDigitosVerificadores(NumerosDoCpf, MultiplicadoresParaValidacaoSegundoDigito, 10);

            if (primeiroDigitoVerificador == NumerosDoCpf[9] && segundoDigitoVerificador == NumerosDoCpf[10])
                return true;

            return false;
        }

        /// <summary>
        /// Calcula os digitos verificadores do cpf de acordo com a regra "https://pt.wikipedia.org/wiki/Cadastro_de_pessoas_f%C3%ADsicas"
        /// </summary>
        /// <param name="numerosDoCpf"></param>
        /// <param name="multiplicador"></param>
        /// <param name="primeirosNumerosDoCpf"></param>
        /// <returns></returns>
        private static int CalcularDigitosVerificadores(Span<int> numerosDoCpf, ReadOnlySpan<int> multiplicador, int primeirosNumerosDoCpf = 9)
        {
            int resultado = 0;

            for (int i = 0; i < primeirosNumerosDoCpf; i++)
            {
                resultado += multiplicador[i] * numerosDoCpf[i];
            }

            var digitoVerificador = NumeroPadraoDaDivisao - (resultado % NumeroPadraoDaDivisao);

            if (digitoVerificador == 10 || digitoVerificador == 11)
                return 0;
            else
                return digitoVerificador;
        }

        /// <summary>
        /// Cpfs inválidos para o governo, mas que são validos segundo a regra de validação de cpfs.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        private static bool VerificaSeTodosOsDigitosSaoDiferentes(string cpf)
        {
            if (cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" ||
                cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999" ||
                cpf == "000.000.000-00" || cpf == "111.111.111-11" || cpf == "222.222.222-22" || cpf == "333.333.333-33" || cpf == "444.444.444-44" ||
                cpf == "555.555.555-55" || cpf == "666.666.666-66" || cpf == "777.777.777-77" || cpf == "888.888.888-88" || cpf == "999.999.999-99")
                return false;

            return true;
        }
        #endregion

        private string RetirarMascaraCPF(string cpf)
        {
            if (cpf?.Length == TamanhoDoCpfComMascara)
            {
                StringBuilder stringBuilder = new();
                foreach (var item in cpf)
                    if (item != ponto && item != traco) stringBuilder.Append(item);

                return stringBuilder.ToString();
            }
            return cpf;
        }

        private string AtribuirMascaraCPF()
        {
            StringBuilder stringBuilder = new(cpf);
            return stringBuilder.Insert(3, ponto).Insert(7, ponto).Insert(11, traco).ToString();
        }

        /// <summary>
        /// Comparação entre CPF
        /// </summary>
        /// <returns>verdadeiro se o CPF for igual ao outro</returns>
        public bool Equals(CPF other) => this.cpf == other.cpf;

        /// <summary>
        /// Inserção de máscara
        /// </summary>
        /// <returns>CPF com a máscara Ex: "669.508.450-57"</returns>
        public override string ToString() => AtribuirMascaraCPF();

    }
}
