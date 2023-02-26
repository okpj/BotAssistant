﻿namespace BotAssistant.Application.Contract.YandexCloud.Model.Speech;

public class Audio
{
    public Audio()
    { }

    public Audio(string uri)
    {
        Uri = uri;
    }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}