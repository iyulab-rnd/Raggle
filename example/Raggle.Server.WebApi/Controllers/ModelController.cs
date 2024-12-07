﻿using Microsoft.AspNetCore.Mvc;
using Raggle.Abstractions;
using Raggle.Abstractions.AI;

namespace Raggle.Server.WebApi.Controllers;

[ApiController]
[Route("/v1/models")]
public class ModelController : ControllerBase
{
    private readonly IRaggle _raggle;

    public ModelController(IRaggle raggle)
    {
        _raggle = raggle;
    }

    [HttpGet("chat")]
    public async Task<ActionResult> GetChatCompletionModelsAsync()
    {
        var models = new Dictionary<string, string[]>();
        var services = _raggle.Services.GetKeyedServices<IChatCompletionService>(KeyedService.AnyKey);

        foreach (var service in services)
        {
            var chatModels = await service.GetChatCompletionModelsAsync();
            models["openai"] = chatModels.Select(m => m.Model).ToArray();
        }

        return Ok(models);
    }

    [HttpGet("embedding")]
    public async Task<ActionResult> GetEmbeddingModelsAsync()
    {
        var models = new Dictionary<string, string[]>();
        var services = _raggle.Services.GetKeyedServices<IEmbeddingService>("openai");

        foreach (var service in services)
        {
            var embeddingModels = await service.GetEmbeddingModelsAsync();
            models["openai"] = embeddingModels.Select(m => m.Model).ToArray();
        }

        return Ok(models);
    }
}
