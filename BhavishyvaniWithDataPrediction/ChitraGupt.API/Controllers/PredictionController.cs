using Asp.Versioning;
using ChitraGupt.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChitraGupt.API.Controllers
{
    [ApiVersion(1)]
    [Route("[controller]/v{v:apiVersion}")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpGet]
        [Route("PredictReportedProblemCodeL0")]
        public IActionResult PredictL0(string description, string shortDescription)
        {
            var (strPredictedValue, percentAccuracy) = _predictionService.PredictL0(description.Trim(), shortDescription.Trim());
            if (strPredictedValue.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, strPredictedValue);

            return Ok(new { strPredictedValue, percentAccuracy });
        }

        [HttpGet]
        [Route("PredictReportedProblemCodeL1")]
        public IActionResult PredictL1(string description, string shortDescription, string strL0)
        {
            var(strPredictedValue, percentAccuracy) = _predictionService.PredictL1(description.Trim(), shortDescription.Trim(), strL0.Trim());
            if(strPredictedValue.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, strPredictedValue);
            return Ok(new { strPredictedValue, percentAccuracy });
        }

        [HttpGet]
        [Route("PredictReportedProblemCodeL2")]
        public IActionResult PredictL2(string description, string shortDescription, string strL0, string strL1)
        {
            var(strPredictedValue, percentAccuracy) = _predictionService.PredictL2(description.Trim(), shortDescription.Trim(), strL0.Trim(), strL1.Trim());
            if (strPredictedValue.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, strPredictedValue);
            return Ok(new { strPredictedValue, percentAccuracy });
        }

        [HttpGet]
        [Route("PredictReportedProblemCodeL3")]
        public IActionResult PredictL3(string description, string shortDescription, string strL0, string strL1, string strL2)
        {
            var(strPredictedValue, percentAccuracy) = _predictionService.PredictL3(description.Trim(), shortDescription.Trim(), strL0.Trim(), strL1.Trim(), strL2.Trim());
            if (strPredictedValue.Trim().StartsWith("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, strPredictedValue);
            return Ok(new { strPredictedValue, percentAccuracy });
        }

        [HttpGet]
        [Route("PredictReportedProblemCodes")]
        public IActionResult PredictReportedCodes(string description, string shortDescription)
        {
            var (strL1, strL2, strL3, isSuccess) = _predictionService.PredictReportedCodeValue(description.Trim(), shortDescription.Trim());
            if (!isSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to predict");
            return Ok(new
            {
                L1 = strL1,
                L2 = strL2,
                L3 = strL3
            });
        }
    }
}
