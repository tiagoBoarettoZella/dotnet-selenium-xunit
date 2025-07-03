using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

public class LoginTestsPlaywright
{
    [Fact]
    public async Task DeveFazerLoginComSucesso()
    {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        try
        {
            // Acessa a página de login
            await page.GotoAsync("https://smartz.zellasistemas.com.br/#/autenticacao/login");

            // (Opcional) Captura de screenshot para debug // await page.ScreenshotAsync(new() { Path = "tela_inicial.png" });

            // Aguarda os campos aparecerem
            await page.WaitForSelectorAsync("#usuario");
            await page.WaitForSelectorAsync("#senha");
            await page.WaitForSelectorAsync("button:has-text(\"ENTRAR\")");

            // Valida existência e visibilidade dos campos
            var campoUsuario = await page.QuerySelectorAsync("#usuario");
            var campoSenha = await page.QuerySelectorAsync("#senha");
            var botaoEntrar = await page.QuerySelectorAsync("button:has-text(\"ENTRAR\")");


            Assert.NotNull(campoUsuario);
            Assert.True(await campoUsuario.IsVisibleAsync());

            Assert.NotNull(campoSenha);
            Assert.True(await campoSenha.IsVisibleAsync());

            Assert.NotNull(botaoEntrar);
            Assert.True(await botaoEntrar.IsVisibleAsync());


            // Preencher login e senha (substitua pelos dados corretos)
            await page.FillAsync("#usuario", "tiago.torre_geral@h");
            await page.FillAsync("#senha", "Rf@c6h12o6h3po4");

            // (Opcional) Captura de screenshot para debug
            //        await page.ScreenshotAsync(new() { Path = "Usuario_senha.png" });
            // Clicar no botão "ENTRAR"
            await page.ClickAsync("button:has-text(\"ENTRAR\")");

            // Espera a URL mudar para conter "/home"
            await page.WaitForURLAsync(url => url.Contains("/home"), new() { Timeout = 15000 });

            // (Opcional) Captura de screenshot para debug
            await page.ScreenshotAsync(new() { Path = "screenshot.png" });

            // Verifica se o login foi bem-sucedido
            Assert.Contains("/home", page.Url);
        }
        catch (Exception ex)
        {
            // Cria pasta se não existir
            Directory.CreateDirectory("screenshots");

            // Gera nome com data/hora
            string fileName = $"screenshots/erro-{DateTime.Now:yyyyMMdd-HHmmss}.png";
            await page.ScreenshotAsync(new() { Path = fileName, FullPage = true });

            // Lança o erro original para não "mascarar" o resultado do teste
            throw new Exception($"Erro durante o teste. Screenshot salvo em: {fileName}", ex);
        }        
    }
}
