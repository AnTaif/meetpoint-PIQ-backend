using Serilog.Core;
using Serilog.Events;

namespace Core.Logger;

public class RemovePropertiesEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent le, ILogEventPropertyFactory lepf)
    {
        le.RemovePropertyIfPresent("ActionId");
        le.RemovePropertyIfPresent("ActionName");
    }
}