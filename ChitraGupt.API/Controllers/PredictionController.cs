using Chitragupt_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChitraGupt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpGet]
        [Route("ChitraGupt/L1")]
        public IActionResult PredictL1(string description, string shortDescription, string strL0)
        {
            var strPredictedValue = _predictionService.PredictL1(description.Trim(), shortDescription.Trim(), strL0.Trim());
            if(strPredictedValue.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, strPredictedValue);
            return Ok(strPredictedValue);
        }

        [HttpGet]
        [Route("ChitraGupt/L2")]
        public IActionResult PredictL2(string description, string shortDescription, string strL0, string strL1)
        {
            var strPredictedValue = _predictionService.PredictL2(description.Trim(), shortDescription.Trim(), strL0.Trim(), strL1.Trim());
            if (strPredictedValue.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, strPredictedValue);
            return Ok(strPredictedValue);
        }

        [HttpGet]
        [Route("ChitraGupt/L3")]
        public IActionResult PredictL3(string description, string shortDescription, string strL0, string strL1, string strL2)
        {
            var strPredictedValue = _predictionService.PredictL3(description.Trim(), shortDescription.Trim(), strL0.Trim(), strL1.Trim(), strL2.Trim());
            if (strPredictedValue.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, strPredictedValue);
            return Ok(strPredictedValue);
        }
        [HttpGet]
        [Route("ChitraGupt/ReportedCodes")]
        public IActionResult PredictReportedCodes(string description, string shortDescription, string strL0)
        {
            var tplPredictedValue = _predictionService.PredictReportedCodeValue(description.Trim(), shortDescription.Trim(), strL0.Trim());
            if (tplPredictedValue.Item1.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, tplPredictedValue.Item1);
            return Ok(new
            {
                L1 = tplPredictedValue.Item1,
                L2 = tplPredictedValue.Item2,
                L3 = tplPredictedValue.Item3
            });
        }
    }
}
