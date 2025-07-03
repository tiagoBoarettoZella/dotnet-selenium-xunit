using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Tests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page) => _page = page;

        public Task NavegarParaLoginAsync() =>
            _page.GotoAsync("https://smartz.zellasistemas.com.br/#/autenticacao/login");

        public async Task PreencherFormularioAsync(string usuario, string senha)
        {
            await _page.FillAsync("#usuario", usuario);
            await _page.FillAsync("#senha", senha);
        }

        public Task ClicarEntrarAsync() =>
            _page.ClickAsync("button:has-text(\"ENTRAR\")");

        public Task AguardarHomeAsync() =>
            _page.WaitForURLAsync(url => url.Contains("/home"), new() { Timeout = 15000 });
    }
}
