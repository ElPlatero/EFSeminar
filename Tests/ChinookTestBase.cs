using EntityFrameworkCoreSeminar.Database.Models.Chinook;
using Xunit.Abstractions;

namespace Tests;

public abstract partial class ChinookTestBase
{
    protected partial ChinookContext getContext(ITestOutputHelper helper);
}