using Nerdstrap.Identity.Services.RiskAnalysis.Models;

namespace Nerdstrap.Identity.Services.RiskAnalysis.Contracts
{
    public class AnalyzeRequest : BaseRequest
    {
        public CustomFact CustomFact { get; set; }
    }
}