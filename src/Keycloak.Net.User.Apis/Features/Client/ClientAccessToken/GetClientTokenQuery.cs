﻿using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;

internal class GetClientTokenQuery : IGetClientTokenQuery
{

    public async Task<Result<TokenRepresentation?>> GetClientTokenAsync(string tokenEndpointUrl
        , GetClientTokenRequest client, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var requestBody = GetClientTokenQueryBodyBuilder.ClientTokenRequestBody(client);

            var response = await httpClient.PostAsync(tokenEndpointUrl, requestBody, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<TokenRepresentation?>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<TokenRepresentation>(cancellationToken);
                var rpt = content?.AccessToken;
                if (response.StatusCode == HttpStatusCode.OK && content is not null && !String.IsNullOrEmpty(rpt)) 
                {
                    return Result<TokenRepresentation?>.Success(content, response.StatusCode);
                }
                else
                {
                    return Result<TokenRepresentation?>.Fail($"Access token not found from the Client");
                }
            }
        }
        catch (Exception ex)
        {
            return Result<TokenRepresentation?>.Fail(HttpStatusCode.InternalServerError,$"Something went wrong //br{ex.Message}");
            throw;
        }
    }
}