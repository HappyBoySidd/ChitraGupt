using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChitraGupt.API.Interfaces;

namespace Translate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslate _translationService;

        public TranslationController(ITranslate translationService)
        {
            _translationService = translationService;
        }

        [HttpGet]
        public IActionResult TranslateText(string strText)
        {
            if(string.IsNullOrWhiteSpace(strText))
                return StatusCode(StatusCodes.Status406NotAcceptable, "Invalid Input");
            var strTranslatedText = _translationService.TranslateText(strText).Result;
            if (string.IsNullOrWhiteSpace(strTranslatedText))
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to translate");
            return Ok(strTranslatedText);
        }

    }
}
