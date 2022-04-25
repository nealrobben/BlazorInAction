using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail
{
    public class GetTrailHandler : IRequestHandler<GetTrailRequest, GetTrailRequest.Response?>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetTrailHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetTrailRequest.Response> Handle(GetTrailRequest request, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("SecureAPIClient");

            try
            {
                return await client.GetFromJsonAsync<GetTrailRequest.Response>(GetTrailRequest.RouteTemplate.Replace("{trailId}",request.TrailId.ToString()));
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }
}
