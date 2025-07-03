using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Tests.Pages;

namespace Tests
{
    public class LoginTestsPlaywright
    {
        [Theory]
        [InlineData("tiago.torre_geral@h", "senhaErrada", false)]
        [InlineData("usuario.invalido@teste.com", "123456", false)]
        [InlineData("tiago.torre_geral@h", "Rf@c6h12o6h3po4", true)]
        public async Task TestarLogin(string usuario, string senha, bool deveLogar)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var loginPage = new LoginPage(page);

            try
            {
                await loginPage.NavegarParaLoginAsync();
                await loginPage.PreencherFormularioAsync(usuario, senha);
                await loginPage.ClicarEntrarAsync();

                if (deveLogar)
                {
                    await loginPage.AguardarHomeAsync();
                    Assert.Contains("/home", page.Url);
                }
                else
                {
                    await page.WaitForTimeoutAsync(2000);
                    Assert.Contains("/autenticacao", page.Url);
                }
            }
            catch (Exception ex)
            {
                Directory.CreateDirectory("screenshots");
                string fileName = $"screenshots/erro-login-{DateTime.Now:yyyyMMdd-HHmmss}.png";
                await page.ScreenshotAsync(new() { Path = fileName, FullPage = true });
                throw new Exception($"Erro no teste de login. Screenshot: {fileName}", ex);
            }
        }
    }

    public class HomePageTests
    {
        [Fact]
        public async Task DeveValidarCamposDaHome()
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var loginPage = new LoginPage(page);
            var homePage = new HomePage(page);

            await loginPage.NavegarParaLoginAsync();
            await loginPage.PreencherFormularioAsync("tiago.torre_geral@h", "Rf@c6h12o6h3po4");
            await loginPage.ClicarEntrarAsync();
            await loginPage.AguardarHomeAsync();

            await homePage.ValidarCamposVisiveisAsync();
            await page.ScreenshotAsync(new() { Path = "screenshots/home-validada.png", FullPage = true });
        }
    }
}
