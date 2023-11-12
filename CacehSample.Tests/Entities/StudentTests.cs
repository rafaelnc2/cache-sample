using CacheSample.Core.Entities;

namespace CacehSample.Tests.Entities;

public class StudentTests
{
    [Fact]
    public void CriarUmEstudanteComSucesso()
    {
        var student = new Student("Rafael", "34031566808", "14/02/1985", "rafael.nc2@gmail.com");

        Assert.True(student.IsValid);
        Assert.Equal(0, student.Errors.Count);
    }

    [Fact]
    public void CriarUmEstudanteComNomeInvalido()
    {
        var student = new Student("", "34031566808", "14/02/1985", "rafael.nc2@gmail.com");

        Assert.False(student.IsValid);
        Assert.Equal(1, student.Errors.Count);
        Assert.Equal("Nome inválido", student.Errors.First());
    }

    [Fact]
    public void CriarUmEstudanteComEmailInvalido()
    {
        var student = new Student("Rafael", "34031566808", "14/02/1985", "rafael.nc2");

        Assert.False(student.IsValid);
        Assert.Equal(1, student.Errors.Count);
        Assert.Equal("Email inválido", student.Errors.First());
    }
}
