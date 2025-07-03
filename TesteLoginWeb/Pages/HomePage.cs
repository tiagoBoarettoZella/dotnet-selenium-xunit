using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Pages
{
    public class HomePage
    {
        private readonly IPage _page;

        public HomePage(IPage page) => _page = page;

        public async Task ValidarCamposVisiveisAsync()
        {
            await _page.WaitForSelectorAsync("h5:has-text(\"Bem vindo\")");
            Assert.True(await _page.IsVisibleAsync("h5:has-text(\"Bem vindo\")"));

            await _page.WaitForSelectorAsync("img[alt='Home']");
            Assert.True(await _page.IsVisibleAsync("img[alt='Home']"));

            await _page.WaitForSelectorAsync("button[ptooltip='Abrir chamado']");
            Assert.True(await _page.IsVisibleAsync("button[ptooltip='Abrir chamado']"));

            await _page.WaitForSelectorAsync("button[ptooltip='Sair']");
            Assert.True(await _page.IsVisibleAsync("button[ptooltip='Sair']"));
        }
    }
}
