using Nerdstrap.Identity.Services.RiskAnalysis.Enums;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Models
{
    public class CustomFact
	{
        public DataTypeEnum DataType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}