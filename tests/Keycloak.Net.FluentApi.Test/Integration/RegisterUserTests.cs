namespace Keycloak.Net.FluentApi.Test.Integration;

public class RegisterUserTests
{
    private const string _url = "https://localhost:8843/realms/oidc/";
    private const string _clientId = "public";
    private const string _clientSecret = "Jj4hCpRdezqvSVDjxXmNRWZ5CTRsg14p";

    //[Fact]
    public void RegisterUser_RegisterUserWithoutRole_ReturnCreated()
    {
        //Arrange
        var username = "username";
        var password = "password";
        var email = "email";
        var firstName = "";
        var lastName = "";

        //Act
        var response = RegisterUser
            .RealmUrl(_url)
            .ClientId(_clientId)
            .ClientSecret(_clientSecret)
            .UserName(username)
            .UserPassword(password)
            .UserEmail(email)
            .UserFirstName(firstName)
            .UserLastName(lastName)
            .Register();

        //Assert
        Assert.True(response.IsSuccess);
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
    }

}
