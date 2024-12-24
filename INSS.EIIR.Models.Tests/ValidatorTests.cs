using INSS.EIIR.Models.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace INSS.EIIR.Models.Tests
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("john was here")]
        [InlineData("\".,<>)(*&^'")]
        public void Only7bitCharacters_True(string value)
        {
            //Arrange
            var attrib = new ContainsOnly7bitCharactersAttribute();

            //Act
            var validationResult = attrib.IsValid(value);

            //Assert
            Assert.True(validationResult);
        }


        [Theory]
        [InlineData("’")]
        [InlineData("This a test ’É")]
        public void Only7bitCharacters_False(string value)
        {
            //Arrange
            var attrib = new ContainsOnly7bitCharactersAttribute();
            var context = new ValidationContext(value);

            //Act
            var validationResult = attrib.IsValid(value);

            //Assert
            Assert.False(validationResult);
        }

    }
}
