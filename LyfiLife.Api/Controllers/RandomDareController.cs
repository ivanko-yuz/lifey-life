﻿using LyfiLife.Core.Contracts;
using LyfiLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyfiLife.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomDareController : ControllerBase
    {
        private readonly ILogger<RandomDareController> _logger;
        private readonly IRandomDareDataService _randomDareDataService;
        private readonly IHistoryDataService _historyDataService;

        public RandomDareController(
            ILogger<RandomDareController> logger,
            IRandomDareDataService randomDareDataService, 
            IHistoryDataService historyDataService)
        {
            _logger = logger;
            _randomDareDataService = randomDareDataService;
            _historyDataService = historyDataService;
        }

        [HttpGet]
        public async Task<RandomDare> Get()
        {
            return await _randomDareDataService.GetRandomDare();
        }
        
        [HttpPost]
        [Route("Complete")]
        public async Task Complete([FromBody]RandomDare randomDare)
        {
            await _historyDataService.CompleteRandomDare(Guid.NewGuid(), randomDare.Uuid);
        }
        
        [HttpPost]
        [Route("Skip")]
        public async Task Skip([FromBody]RandomDare randomDare)
        {
            await _historyDataService.SkipRandomDare(Guid.NewGuid(), randomDare.Uuid);
        }
    }
}