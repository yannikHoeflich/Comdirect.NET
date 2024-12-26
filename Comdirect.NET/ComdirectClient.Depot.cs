using Comdirect.NET.Models;
using Comdirect.NET.Models.Accounting;
using Comdirect.NET.Models.Depot;
using FluentResults;

namespace Comdirect.NET;

public partial class ComdirectClient {
    public async Task<Result<IReadOnlyCollection<Depot>>> GetDepots() {
        Result<PagingObject<Depot>> result = await Request<PagingObject<Depot>>(HttpMethod.Get, $"api/brokerage/clients/user/v3/depots");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value.Values);
    }
    
    public async Task<Result<IReadOnlyCollection<Position>>> GetPositions(Depot depot) {
        Result<PagingObject<Position>> result = await Request<PagingObject<Position>>(HttpMethod.Get, "brokerage", 3, "depots", depot.Id, "positions");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value.Values);
    }
    
    public async Task<Result<Position>> GetPosition(Depot depot, string id) {
        Result<Position> result = await Request<Position>(HttpMethod.Get, "brokerage", 3, "depots", depot.Id, $"positions/{id}");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value);
    }
    
    public async Task<Result<IReadOnlyCollection<DepotTransaction>>> GetTransactions(Depot depot) {
        Result<PagingObject<DepotTransaction>> result = await Request<PagingObject<DepotTransaction>>(HttpMethod.Get, "brokerage", 3, "depots", depot.Id, "transactions");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value.Values);
    }
}