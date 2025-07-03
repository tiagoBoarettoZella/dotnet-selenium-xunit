/*
using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

public class LoginTestsPlaywright
{
    [Theory]
    [InlineData("tiago.torre_geral@h", "senhaErrada", false)] // valida erro de senha
    [InlineData("usuario.invalido@teste.com", "123456", false)] // valida erro de usuario
    [InlineData("tiago.torre_geral@h", "Rf@c6h12o6h3po4", true)] // <- Login válido
    public async Task TestarLogin(string usuario, string senha, bool deveLogar)
    {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        try
        {
            // seta a pagina de teste do smartz 
            await page.GotoAsync("https://smartz.zellasistemas.com.br/#/autenticacao/login");

            // verifica se os campos sa pagina de login existem
            await page.WaitForSelectorAsync("#usuario");
            await page.WaitForSelectorAsync("#senha");
            await page.WaitForSelectorAsync("button:has-text(\"ENTRAR\")");

            await page.FillAsync("#usuario", usuario);
            await page.FillAsync("#senha", senha);
            await page.ClickAsync("button:has-text(\"ENTRAR\")");

            // loop para testar os 3 casos configurados no Theory
            if (deveLogar)
            {
                // Deve ir para /home
                await page.WaitForURLAsync(url => url.Contains("/home"), new() { Timeout = 15000 });
                Assert.Contains("/home", page.Url);
            }
            else
            {
                // Deve continuar na tela de login
                await page.WaitForTimeoutAsync(3000); // pequeno tempo pra não ser instantâneo
                Assert.Contains("/autenticacao", page.Url);
            }
        }
        catch (Exception ex)
        {
            Directory.CreateDirectory("screenshots");
            string fileName = $"screenshots/erro-{DateTime.Now:yyyyMMdd-HHmmss}.png";
            await page.ScreenshotAsync(new() { Path = fileName, FullPage = true });
            throw new Exception($"Erro durante o teste. Screenshot salvo em: {fileName}", ex);
        }
    }
}
*/