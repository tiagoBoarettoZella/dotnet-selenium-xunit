using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Tests.Pages;

namespace TestHomeeWeb.Tests
{
    public class HomeTests
    {
        [Fact]
        public async Task DeveValidarComponentesDaHome()
        {
            Console.WriteLine("🏠 Iniciando validação da Home...");

            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var loginPage = new LoginPage(page);
            var homePage = new HomePage(page);

            try
            {
                Console.WriteLine("🔐 Realizando login...");
                await loginPage.NavegarParaLoginAsync();
                await loginPage.PreencherFormularioAsync("tiago.torre_geral@h", "Rf@c6h12o6h3po4");
                await loginPage.ClicarEntrarAsync();
                await loginPage.AguardarHomeAsync();

                Console.WriteLine("🧩 Validando campos principais...");
                await homePage.ValidarCamposVisiveisAsync();

                Console.WriteLine("📂 Validando menu lateral...");
                await homePage.ValidarMenuLateralAsync();

                Console.WriteLine("📸 Capturando screenshot da Home...");
                Directory.CreateDirectory("screenshots");
                await page.ScreenshotAsync(new() { Path = "screenshots/home-validada.png", FullPage = true });

                Console.WriteLine("✅ Validação concluída com sucesso.");
            }
            catch (Exception ex)
            {
                string fileName = $"screenshots/erro-home-{DateTime.Now:yyyyMMdd-HHmmss}.png";
                await page.ScreenshotAsync(new() { Path = fileName, FullPage = true });
                Console.WriteLine($"💥 Erro ao validar a Home. Screenshot: {fileName}");
                throw new Exception($"Erro no teste da Home. Screenshot: {fileName}", ex);
            }
        }
    }
}
