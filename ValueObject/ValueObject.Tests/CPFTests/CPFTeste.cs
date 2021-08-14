using ValueObject.ObjetosDeValor;
using Xunit;

namespace ValueObject.Tests.CPFTests
{

    public class CPFTeste
    {
        [Theory]
        [InlineData("16764889015")]
        [InlineData("329.209.540-91")]
        public void CPF_ValidarCPF_DeveRetornarVerdadeiroSeCPFForValido(string cpf)
        {
            //Arrange
            bool verdadeiro;

            //Act
            verdadeiro = CPF.ValidarCPF(cpf);

            //Assert
            Assert.True(verdadeiro);
        }

        [Theory]
        [InlineData("16764889016")]
        [InlineData("329.209.540-99")]
        [InlineData("329020905400.9")]
        [InlineData("32902090540099")]
        public void CPF_ValidarCPF_DeveRetornarFalsoSeCPFForInvalido(string cpf)
        {
            //Arrange
            bool falso;

            //Act
            falso = CPF.ValidarCPF(cpf);

            //Assert
            Assert.False(falso);
        }

        [Fact]
        public void CPF_ToString_DeveRetornarCPFComAMascaraAplicada()
        {
            //Arrange
            string cpfCorreto1 = "664.626.000-12";
            string cpfCorreto2 = "329.209.540-91";
            string cpfComMascara1;
            string cpfComMascara2;
            CPF cpf1 = new CPF();
            CPF cpf2 = new CPF();
            cpf1 = "66462600012";
            cpf2 = "329.209.540-91";

            //Act
            cpfComMascara1 = cpf1.ToString();
            cpfComMascara2 = cpf2.ToString();

            //Assert
            Assert.Equal(cpfCorreto1, cpfComMascara1);
            Assert.Equal(cpfCorreto2, cpfComMascara2);
        }

        [Fact]
        public void CPF_CPFSemMascara_DeveRetornarCPFSemAMascaraAplicada()
        {
            //Arrange
            string cpfCorreto1 = "66462600012";
            string cpfCorreto2 = "32920954091";
            string cpfSemMascara1;
            string cpfSemMascara2;
            CPF cpf1 = new CPF();
            CPF cpf2 = new CPF();
            cpf1 = "664.626.000-12";
            cpf2 = "32920954091";

            //Act
            cpfSemMascara1 = cpf1.CpfSemMascara;
            cpfSemMascara2 = cpf2.CpfSemMascara;

            //Assert
            Assert.Equal(cpfCorreto1, cpfSemMascara1);
            Assert.Equal(cpfCorreto2, cpfSemMascara2);
        }

        [Fact]
        public void CPF_Equals_DeveRetornarVerdadeiroSeUmCPFForIgualAoOutro()
        {
            //Arrange
            CPF cpf1 = new CPF();
            CPF cpf2 = new CPF();
            bool verdadeiro;
            cpf1 = "66462600012";
            cpf2 = "664.626.000-12";

            //Act
            verdadeiro = cpf1.Equals(cpf2);

            //Assert
            Assert.True(verdadeiro);
        }
    }
}
