using System;
using System.Text;

namespace ValueObject.ObjetosDeValor
{
    public class CNPJ : IEquatable<CNPJ>
    {
        private const int NumeroPadraoDaDivisao = 11;
        private const int TamanhoDoCnpjComMascara = 18;
        private const int TamanhoDoCnpjSemMascara = 14;
        private const char ponto = '.';
        private const char traco = '-';
        private const char barra = '/';

        private string cnpj;

        /// <summary>
        /// Remoção da máscara
        /// </summary>
        /// <returns>CNPJ sem a máscara Ex: "73129217000170"</returns>
        public string CnpjSemMascara => cnpj;

        public string Cnpj
        {
            get => cnpj;
            private set
            {
                if (ValidarCNPJ(value))
                    cnpj = RetirarMascaraCNPJ(value);
                else
                    throw new Exception("Cnpj inválido");
            }
        }
        public CNPJ()
        {
        }
        public CNPJ(string cnpj) => Cnpj = cnpj;

        /// <summary>
        /// Permite a atribuição de uma string diretamente com o operador de atribuição. Ex: public CNPJ cnpj { get; set; } / ACME.CNPJ = "73.129.217/0001-70"
        /// </summary>
        /// <param name="value">CNPJ a ser instanciado</param>
        /// <exception>Cnpj inválido</exception>
        public static implicit operator CNPJ(string value) => new CNPJ(value);

        #region Validação de CNPJ
        /// <summary>
        /// Validçao de um CNPJ
        /// Obs: A validação não se trata de esse CNPJ estar atrelado a um CNPJ existente, e sim se os digtos informados conferem com os digitos verificadores
        /// </summary>
        /// <param name="cnpj">CNPJ a ser validado</param>
        /// <returns>falso se um cnpj não for valido</returns>
        public static bool ValidarCNPJ(string cnpj)
        {
            Span<int> NumerosDoCnpj = stackalloc int[14];

            if (cnpj.Length == TamanhoDoCnpjSemMascara || (cnpj.Length == TamanhoDoCnpjComMascara && cnpj[2] == ponto && cnpj[6] == ponto && cnpj[10] == barra && cnpj[15] == traco))
            {
                int j = 0;
                for (int i = 0; i < cnpj.Length; i++)
                {
                    if (cnpj[i] != ponto && cnpj[i] != traco && cnpj[i] != barra)
                    {
                        NumerosDoCnpj[j] = cnpj[i] - '0';
                        j++;
                    }
                }
            }
            else
                return false;

            ReadOnlySpan<int> MultiplicadoresParaValidacaoPrimeiroDigito = stackalloc int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            ReadOnlySpan<int> MultiplicadoresParaValidacaoSegundoDigito = stackalloc int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var primeiroDigitoVerificador = CalcularDigitosVerificadores(NumerosDoCnpj, MultiplicadoresParaValidacaoPrimeiroDigito);
            var segundoDigitoVerificador = CalcularDigitosVerificadores(NumerosDoCnpj, MultiplicadoresParaValidacaoSegundoDigito, 13);

            if (primeiroDigitoVerificador == NumerosDoCnpj[12] && segundoDigitoVerificador == NumerosDoCnpj[13])
                return true;

            return false;
        }

        /// <summary>
        /// Calcula os digitos verificadores do Cnpj de acordo com a regra "https://souforce.cloud/regra-de-validacao-para-cpf-e-cnpj/"
        /// </summary>
        /// <param name="numerosDoCpf"></param>
        /// <param name="multiplicador"></param>
        /// <param name="primeirosNumerosDoCpf"></param>
        /// <returns></returns>
        private static int CalcularDigitosVerificadores(Span<int> numerosDoCnpj, ReadOnlySpan<int> multiplicador, int primeirosNumerosDoCnpj = 12)
        {
            int resultado = 0;

            for (int i = 0; i < primeirosNumerosDoCnpj; i++)
                resultado += multiplicador[i] * numerosDoCnpj[i];

            var restoDaDivisao = resultado % NumeroPadraoDaDivisao;

            if (restoDaDivisao < 2)
                return 0;
            else
                return NumeroPadraoDaDivisao - restoDaDivisao;
        }
        #endregion

        private string RetirarMascaraCNPJ(string cnpj)
        {
            if (cnpj?.Length == TamanhoDoCnpjComMascara)
            {
                StringBuilder stringBuilder = new();
                foreach (var item in cnpj)
                    if (item != ponto && item != traco && item != barra) stringBuilder.Append(item);

                return stringBuilder.ToString();
            }
            return cnpj;
        }

        private string AtribuirMascaraCNPJ()
        {
            StringBuilder stringBuilder = new(cnpj);
            return stringBuilder.Insert(2, ponto).Insert(6, ponto).Insert(10, barra).Insert(15, traco).ToString();
        }


        /// <summary>
        /// Comparação entre CNPJ
        /// </summary>
        /// <returns>verdadeiro se o CNPJ for igual ao outro</returns>
        public bool Equals(CNPJ other) => this.cnpj == other.cnpj;

        /// <summary>
        /// Inserção de máscara
        /// </summary>
        /// <returns>CNPJ com a máscara EX:"73.129.217/0001-70"</returns>
        public override string ToString() => AtribuirMascaraCNPJ();

    }
}
