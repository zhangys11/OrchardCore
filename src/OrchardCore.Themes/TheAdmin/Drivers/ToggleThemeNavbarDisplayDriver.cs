using OrchardCore.Admin.Models;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;

namespace OrchardCore.Themes.TheAdmin.Drivers;

public sealed class ToggleThemeNavbarDisplayDriver : DisplayDriver<Navbar>
{
    private readonly ISiteService _siteService;

    public ToggleThemeNavbarDisplayDriver(ISiteService siteService)
    {
        _siteService = siteService;
    }

    public override IDisplayResult Display(Navbar model, BuildDisplayContext context)
    {
        return View("ToggleTheme", model)
            .RenderWhen(async () => (await _siteService.GetSettingsAsync<AdminSettings>()).DisplayThemeToggler)
            .Location(OrchardCoreConstants.DisplayType.DetailAdmin, "Content:10");
    }
}
