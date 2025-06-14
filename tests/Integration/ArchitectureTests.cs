using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Integration.Tests;

public class ArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            typeof(Core.Domain.Entities.Note).Assembly,
            typeof(Core.Application.INoteRepository).Assembly,
            typeof(Infrastructure.AppDbContext).Assembly,
            typeof(Presentation.Desktop.App).Assembly,
            typeof(Presentation.Functions.BackupTimer).Assembly)
        .Build();

    private readonly IObjectProvider<IType> Domain = Types().That().ResideInNamespace("Core.Domain", true).As("Domain");
    private readonly IObjectProvider<IType> Application = Types().That().ResideInNamespace("Core.Application", true).As("Application");
    private readonly IObjectProvider<IType> Infrastructure = Types().That().ResideInNamespace("Infrastructure", true).As("Infrastructure");
    private readonly IObjectProvider<IType> Presentation = Types().That().ResideInNamespace("Presentation", true).As("Presentation");

    [Fact]
    public void Domain_Should_Not_Depend_On_Other_Layers()
    {
        IArchRule rule = Types().That().Are(Domain)
            .Should().NotDependOnAny(Application)
            .AndShould().NotDependOnAny(Infrastructure)
            .AndShould().NotDependOnAny(Presentation);
        rule.Check(Architecture);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Presentation_Or_Infrastructure()
    {
        IArchRule rule = Types().That().Are(Application)
            .Should().NotDependOnAny(Presentation)
            .AndShould().NotDependOnAny(Infrastructure);
        rule.Check(Architecture);
    }

    [Fact]
    public void Presentation_Should_Not_Depend_On_Infrastructure()
    {
        IArchRule rule = Types().That().Are(Presentation)
            .Should().NotDependOnAny(Infrastructure);
        rule.Check(Architecture);
    }
}
