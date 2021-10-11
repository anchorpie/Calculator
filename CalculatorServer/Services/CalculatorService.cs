using CalulatorServer;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace CalculatorServer.Services
{
    public static class MathCalculator
    {
        public static string Calc(Expression request)
        {
            try
            {
                double calculationResalt = Convert.ToDouble(new DataTable().Compute(request.ReceivedExpression, null));
                return calculationResalt.ToString();
            }
            catch (Exception)
            {
                return "An error occurred while calculating an expression at the server level, please check your input";
            }
        }
    }

    class CalculatorService : Calculator.CalculatorBase
    {
        private readonly ILogger<CalculatorService> _logger;

        public CalculatorService(ILogger<CalculatorService> logger)
        {
            _logger = logger;
        }

        public override Task<CalculationResult> GetExpressionResult(Expression request, ServerCallContext context)
        {
            CalculationResult result = new CalculationResult();
            result.Result = MathCalculator.Calc(request);
            return Task.FromResult(result);
        }
    }
}
