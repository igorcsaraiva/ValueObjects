using ValueObject.ObjetosDeValor;
using Xunit;

namespace ValueObject.Tests.CNPJTests
{
    public class CNPJTeste
    {
        [Theory]
        [InlineData("48986424000101")]
        [InlineData("11.506.292/0001-55")]
        public void CNPJ_ValidarCNPJ_DeveRetornarVerdadeiroSeCNPJForValido(string cnpj)
        {
            //Arrange
            bool verdadeiro;

            //Act
            verdadeiro = CNPJ.ValidarCNPJ(cnpj);

            //Assert
            Assert.True(verdadeiro);
        }

        [Theory]
        [InlineData("48986424000102")]
        [InlineData("88.161.701/0001-54")]
        [InlineData("7411776015900018-6")]
        [InlineData("87.539/839/0001380")]
        public void CNPJ_ValidarCNPJ_DeveRetornarFalsoSeCNPJForValido(string cnpj)
        {
            //Arrange
            bool falso;

            //Act
            falso = CNPJ.ValidarCNPJ(cnpj);

            //Assert
            Assert.False(falso);
        }

        [Fact]
        public void CNPJ_ToString_DeveRetornarCNPJComAMascaraAplicada()
        {
            //Arrange
            string cnpjCorreto1 = "10.157.407/0001-80";
            string cnpjCorreto2 = "63.058.193/0001-51";
            string cnpjComMascara1;
            string cnpjComMascara2;
            CNPJ cnpj1 = new CNPJ();
            CNPJ cnpj2 = new CNPJ();
            cnpj1 = "10157407000180";
            cnpj2 = "63.058.193/0001-51";

            //Act
            cnpjComMascara1 = cnpj1.ToString();
            cnpjComMascara2 = cnpj2.ToString();

            //Assert
            Assert.Equal(cnpjCorreto1, cnpjComMascara1);
            Assert.Equal(cnpjCorreto2, cnpjComMascara2);
        }

        [Fact]
        public void CNPJ_CNPJSemMascara_DeveRetornarCNPJSemAMascaraAplicada()
        {
            //Arrange
            string cnpjCorreto1 = "99421075000103";
            string cnpjCorreto2 = "45385482000183";
            string cnpjComMascara1;
            string cnpjComMascara2;
            CNPJ cnpj1 = new CNPJ();
            CNPJ cnpj2 = new CNPJ();
            cnpj1 = "99421075000103";
            cnpj2 = "45.385.482/0001-83";

            //Act
            cnpjComMascara1 = cnpj1.CnpjSemMascara;
            cnpjComMascara2 = cnpj2.CnpjSemMascara;

            //Assert
            Assert.Equal(cnpjCorreto1, cnpjComMascara1);
            Assert.Equal(cnpjCorreto2, cnpjComMascara2);
        }

        [Fact]
        public void CNPJ_Equals_DeveRetornarVerdadeiroSeUmCNPJForIgualAoOutro()
        {
            //Arrange
            CNPJ cnpj1 = new CNPJ();
            CNPJ cnpj2 = new CNPJ();
            bool verdadeiro;
            cnpj1 = "75.933.634/0001-04";
            cnpj2 = "75933634000104";

            //Act
            verdadeiro = cnpj1.Equals(cnpj2);

            //Assert
            Assert.True(verdadeiro);
        }
    }
}
