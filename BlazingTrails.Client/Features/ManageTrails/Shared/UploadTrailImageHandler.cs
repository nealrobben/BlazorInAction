﻿using BlazingTrails.Shared.Features.ManageTrails.Shared;
using MediatR;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingTrails.Client.Features.ManageTrails
{
    public class UploadTrailImageHandler
        : IRequestHandler<UploadTrailImageRequest, UploadTrailImageRequest.Response>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UploadTrailImageHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UploadTrailImageRequest.Response> Handle(UploadTrailImageRequest request, 
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("SecureAPIClient");
            var fileContent = request.File.OpenReadStream(request.File.Size, cancellationToken);

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileContent), "image", request.File.Name);

            var response = await client.PostAsync(UploadTrailImageRequest.RouteTemplate.Replace("{trailId}", 
                request.TrailId.ToString()),content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var fileName = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                return new UploadTrailImageRequest.Response(fileName);
            }
            else
            {
                return new UploadTrailImageRequest.Response("");
            }
        }
    }
}
