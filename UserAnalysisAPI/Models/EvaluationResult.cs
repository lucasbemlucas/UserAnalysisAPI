using System;

namespace UserAnalysisAPI.Models;

public class EvaluationResult
{
    public Dictionary<string, EndpointTestResult> TestedEndpoints { get; set; } = new Dictionary<string, EndpointTestResult>();

    public class EndpointTestResult
    {
        public int Status { get; set; }
        public long TimeMs { get; set; }
        public bool ValidResponse { get; set; }
    }
}
