namespace Keycloak.Net.FluentApi.Test.Integration;

public class RegisterUserTests
{
    private const string _url = "https://localhost:8843/realms/oidc/";
    private const string _authClientId = "public";
    private const string _authClientSecret = "Jj4hCpRdezqvSVDjxXmNRWZ5CTRsg14p";
    private const string _clientId = "";

    //[Fact]
    public void RegisterUser_RegisterUserWithoutRole_ReturnCreated()
    {
        //Arrange
        var username = "username";
        var password = "password";

        //Act
        var response = RegisterUser
            .Realm(_url)
            .AuthClientId(_authClientId)
            .AuthClientSecret(_authClientSecret)
            .UserName(username)
            .UserPassword(password)
            .Register();

        //Assert
        var res = response.IsSuccess;
        Assert.True(res);
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
    }

    //[Fact]
    public void RegisterUser_RegisterUserWithRealmRole_ReturnCreated()
    {
        //Arrange
        var username = "username";
        var password = "password";
        var role = "Admin";

        //Act
        var response = RegisterUser
            .Realm(_url)
            .AuthClientId(_authClientId)
            .AuthClientSecret(_authClientSecret)
            .UserName(username)
            .UserPassword(password)
            .RealmRole(role)
            .Register();

        //Assert
        var res = response.IsSuccess;
        Assert.True(res);
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
    }

    //[Fact]
    public void RegisterUser_RegisterUserWithClientRole_ReturnCreated()
    {
        //Arrange
        var username = "username";
        var password = "password";
        var role = "user";

        //Act
        var response = RegisterUser
            .Realm(_url)
            .AuthClientId(_authClientId)
            .AuthClientSecret(_authClientSecret)
            .UserName(username)
            .UserPassword(password)
            .ClientRole(role)
            .ForClient(_clientId)
            .Register();

        //Assert
        var res = response.IsSuccess;
        Assert.True(res);
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
    }
}
