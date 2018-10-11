using Nerdstrap.Identity.Services.RiskAnalysis.Models;

namespace Nerdstrap.Identity.Logic.Enrollment.Contracts
{
    public class AnalyzeRequest : BaseRequest
    {
        public CustomFact CustomFact { get; set; }
    }
}