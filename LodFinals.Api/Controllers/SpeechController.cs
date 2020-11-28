using LodFinals.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LodFinals.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : ControllerBase
    {
        private readonly SpeechService _speechService;

        public SpeechController(SpeechService speechService)
        {
            _speechService = speechService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> TextToSpeech(string text,string language)
        {
            string result = await _speechService.TextToSpeech(text, language);
            return Ok(result);
        }
    }
}
