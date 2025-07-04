/*
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Tests.Pages;

namespace TesteWeb.Tests
{
  
    public class LoginTestsPlaywright
    {

        // Lista de usuarios e navegadores usados no teste
        public static IEnumerable<object[]> CasosLogin => new List<object[]>
        {
            new object[] { "chromium", "tiago.torre_geral@h", "senhaErrada", false }, // chrome
            new object[] { "chromium", "usuario.invalido@teste.com", "123456", false }, // chrome
            new object[] { "chromium", "tiago.torre_geral@h", "Rf@c6h12o6h3po4", true }, // chrome
            new object[] { "firefox", "tiago.torre_geral@h", "Rf@c6h12o6h3po4", true }, // firefox
            new object[] { "webkit", "tiago.torre_geral@h", "Rf@c6h12o6h3po4", true } // safari MAC
        };


        [SkippableTheory]
        [MemberData(nameof(CasosLogin))]
        public async Task TestarLogin(string navegador, string usuario, string senha, bool deveLogar)
        {
            // caso não tenha as dependencias do webkit ele da um skip para o teste dele
            Skip.If(navegador == "webkit" && OperatingSystem.IsLinux(), "WebKit não suportado em Linux no CI.");

            // carrega o navegador
            using var playwright = await Playwright.CreateAsync();
            IBrowser browser = navegador switch
            {
                "chromium" => await playwright.Chromium.LaunchAsync(new() { Headless = true }),
                "firefox" => await playwright.Firefox.LaunchAsync(new() { Headless = true }),
                "webkit" => await playwright.Webkit.LaunchAsync(new() { Headless = true }),
                _ => throw new ArgumentException("Navegador inválido")
            };

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var loginPage = new LoginPage(page);

            // executa os testes da tela de login
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
                // em caso de falha tira um print do ponto que deu o erro
                Directory.CreateDirectory("screenshots");
                string fileName = $"screenshots/erro-{navegador}-{DateTime.Now:yyyyMMdd-HHmmss}.png";
                await page.ScreenshotAsync(new() { Path = fileName, FullPage = true });
                throw new Exception($"Erro no teste de login ({navegador}). Screenshot: {fileName}", ex);
            }
        }
    }
*/
    /*public class HomePageTests
    {
        [Fact]
        public async Task DeveValidarCamposDaHome()
        {
            Console.WriteLine("🏠 Página home carregada");
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
*/