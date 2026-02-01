using Microsoft.AspNetCore.Mvc;
using SmartMenza.Business.Models.FoodAnalysis;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.API.Controllers;

[ApiController]
[Route("api/Food")]
public sealed class FoodAnalysisController : ControllerBase
{
    private readonly IAIFoodAnalyzerService _analyzer;

    public FoodAnalysisController(IAIFoodAnalyzerService analyzer)
    {
        _analyzer = analyzer;
    }

    [HttpPost("analysis")]
    public async Task<ActionResult<FoodAnalysisResult>> Analyze([FromBody] AnalyzeFoodRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Text))
            return BadRequest("Text is required.");

        var result = await _analyzer.AnalyzeAsync(req.Text, ct);
        return Ok(result);
    }
}
