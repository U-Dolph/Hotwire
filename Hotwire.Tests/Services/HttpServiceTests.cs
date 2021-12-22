using Hotwire.Model;
using Hotwire.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hotwire.Tests.Services
{
    public class HttpServiceTests
    {
        HttpService httpService = new HttpService();

        [Fact]
        public async void RegisterUser_WhenDatabaseIsEmpty_ShouldSignUp()
        {
            string expected = "Registration successful, now you can log in";
            string username = "Teszt1";
            string nickanme = "Teszt1";
            string password = "asd";

            string actual = await httpService.RegisterUser(new User(username, nickanme, password));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RegisterUser_WhenUsernameAlreadyTaken_ShouldSignUp()
        {
            string expected = "Username already taken!";
            string username = "Teszt1222";
            string nickanme = "Teszt1sada";
            string password = "asd232";

            await httpService.RegisterUser(new User(username, nickanme, password));
            string actual = await httpService.RegisterUser(new User(username, nickanme, password));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void LoginUser_Login_ShouldSignIn()
        {
            string expected = "Login successful";
            string username = "loginteszt";
            string nickanme = "loginteszt";
            string password = "asd232";

            await httpService.RegisterUser(new User(username, nickanme, password));
            string actual = await httpService.LoginUser(new User(username, nickanme, password));

            Assert.Equal(expected, actual);
        }
    }
}
