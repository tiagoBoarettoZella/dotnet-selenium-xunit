using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
//using Xunit.SkippableFact;
using Tests.Pages;

namespace TesteLoginWeb.Tests
{
    public class LoginTests
    {
        public static IEnumerable<object[]> CasosLogin => new List<object[]>
        {
            new object[] { "chromium", "tiago.torre_geral@h", "senhaErrada", false },
            new object[] { "chromium", "usuario.invalido@teste.com", "123456", false },
            new object[] { "chromium", "tiago.torre_geral@h", "Rf@c6h12o6h3po4", true },
            new object[] { "firefox",  "tiago.torre_geral@h", "Rf@c6h12o6h3po4", true },
            new object[] { "webkit",   "tiago.torre_geral@h", "Rf@c6h12o6h3po4", true }
        };

        [SkippableTheory]
        [MemberData(nameof(CasosLogin))]
        public async Task TestarLogin(string navegador, string usuario, string senha, bool deveLogar)
        {
            Console.WriteLine($"🔍 Iniciando teste de login no navegador: {navegador}");

            // Pula teste WebKit no Linux (ex: GitHub Actions ou Codespaces)
            Skip.If(navegador == "webkit" && OperatingSystem.IsLinux(), "⚠️ WebKit não suportado em Linux no CI.");

            using var playwright = await Playwright.CreateAsync();

            Console.WriteLine("🚀 Abrindo navegador...");
            IBrowser browser = navegador switch
            {
                "chromium" => await playwright.Chromium.LaunchAsync(new() { Headless = true }),
                "firefox"  => await playwright.Firefox.LaunchAsync(new() { Headless = true }),
                "webkit"   => await playwright.Webkit.LaunchAsync(new() { Headless = true }),
                _ => throw new ArgumentException("Navegador inválido")
            };

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            var loginPage = new LoginPage(page);

            try
            {
                Console.WriteLine("🌐 Navegando para tela de login...");
                await loginPage.NavegarParaLoginAsync();

                Console.WriteLine("🧾 Preenchendo formulário...");
                await loginPage.PreencherFormularioAsync(usuario, senha);

                Console.WriteLine("🔒 Clicando em ENTRAR...");
                await loginPage.ClicarEntrarAsync();

                if (deveLogar)
                {
                    Console.WriteLine("🕒 Aguardando redirecionamento para /home...");
                    await loginPage.AguardarHomeAsync();
                    Assert.Contains("/home", page.Url);
                    Console.WriteLine("✅ Login bem-sucedido!");
                }
                else
                {
                    await page.WaitForTimeoutAsync(2000);
                    Assert.Contains("/autenticacao", page.Url);
                    Console.WriteLine("❌ Login inválido validado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Erro no teste ({navegador}): {ex.Message}");
                Directory.CreateDirectory("screenshots");
                string fileName = $"screenshots/erro-{navegador}-{DateTime.Now:yyyyMMdd-HHmmss}.png";
                await page.ScreenshotAsync(new() { Path = fileName, FullPage = true });
                Console.WriteLine($"📸 Screenshot salvo em: {fileName}");
                throw new Exception($"Erro no teste de login ({navegador}). Screenshot: {fileName}", ex);
            }
        }
    }
}
