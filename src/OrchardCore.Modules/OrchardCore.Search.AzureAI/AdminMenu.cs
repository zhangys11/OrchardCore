using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.Navigation;
using OrchardCore.Search.AzureAI.Drivers;
using OrchardCore.Search.AzureAI.Models;

namespace OrchardCore.Search.AzureAI;

public sealed class AdminMenu : AdminNavigationProvider
{
    private static readonly RouteValueDictionary _routeValues = new()
    {
        { "area", "OrchardCore.Settings" },
        { "groupId", AzureAISearchDefaultSettingsDisplayDriver.GroupId},
    };

    private readonly AzureAISearchDefaultOptions _azureAISearchSettings;

    internal readonly IStringLocalizer S;

    public AdminMenu(
        IOptions<AzureAISearchDefaultOptions> azureAISearchSettings,
        IStringLocalizer<AdminMenu> stringLocalizer)
    {
        _azureAISearchSettings = azureAISearchSettings.Value;
        S = stringLocalizer;
    }

    protected override ValueTask BuildAsync(NavigationBuilder builder)
    {
        builder
            .Add(S["Search"], NavigationConstants.AdminMenuSearchPosition, search => search
                .AddClass("search")
                .Id("search")
                .Add(S["Indexing"], S["Indexing"].PrefixPosition(), indexing => indexing
                    .Add(S["Azure AI Indices"], S["Azure AI Indices"].PrefixPosition(), indexes => indexes
                        .Action("Index", "Admin", "OrchardCore.Search.AzureAI")
                        .AddClass("azureaiindices")
                        .Id("azureaiindices")
                        .Permission(AzureAISearchPermissions.ManageAzureAISearchIndexes)
                        .LocalNav()
                    )
                )
            );

        if (_azureAISearchSettings.DisableUIConfiguration)
        {
            return ValueTask.CompletedTask;
        }

        if (NavigationHelper.UseLegacyFormat())
        {
            builder
               .Add(S["Configuration"], configuration => configuration
                   .Add(S["Settings"], settings => settings
                       .Add(S["Azure AI Search"], S["Azure AI Search"].PrefixPosition(), azureAISearch => azureAISearch
                       .AddClass("azure-ai-search")
                           .Id("azureaisearch")
                           .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = AzureAISearchDefaultSettingsDisplayDriver.GroupId })
                           .Permission(AzureAISearchPermissions.ManageAzureAISearchIndexes)
                           .LocalNav()
                       )
                   )
               );

            return ValueTask.CompletedTask;
        }

        builder
            .Add(S["Settings"], settings => settings
                .Add(S["Search"], S["Search"].PrefixPosition(), search => search
                    .Add(S["Azure AI Search"], S["Azure AI Search"].PrefixPosition(), azureAISearch => azureAISearch
                    .AddClass("azure-ai-search")
                        .Id("azureaisearch")
                        .Action("Index", "Admin", _routeValues)
                        .Permission(AzureAISearchPermissions.ManageAzureAISearchIndexes)
                        .LocalNav()
                    )
                )
            );

        return ValueTask.CompletedTask;
    }
}
