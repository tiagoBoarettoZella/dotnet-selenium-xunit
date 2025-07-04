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

        public async Task ValidarMenuLateralAsync()
        {
            // Valida a barra lateral visível
            var sidebar = await _page.QuerySelectorAsync(".layout-sidebar");
            Assert.NotNull(sidebar);
            Assert.True(await sidebar.IsVisibleAsync());

            // Valida avatar e nome do usuário
            var nomeUsuario = await _page.InnerTextAsync("app-menu-profile strong");
            var cargo = await _page.InnerTextAsync("app-menu-profile small");

            Assert.Equal("Tiago torre_geral", nomeUsuario.Trim());
            Assert.Equal("Administrador(a)", cargo.Trim());

            // Valida alguns itens de menu principais
            var itensMenu = new[]
            {
                "Dashboard",
                "Organização",
                "Logística",
                "Transporte",
                "Configuração"
            };

            foreach (var texto in itensMenu)
            {
                var item = await _page.QuerySelectorAsync($"span.layout-menuitem-text:text-is(\"{texto}\")");
                Assert.NotNull(item);
                Assert.True(await item.IsVisibleAsync());
            }
        }
    }
}
