using Comdirect.NET.Models;
using Comdirect.NET.Models.Depot;
using Comdirect.NET.Models.Instruments;
using FluentResults;

namespace Comdirect.NET;

public partial class ComdirectClient {
    public async Task<Result<IReadOnlyCollection<Instrument>>> GetInstruments(string id) {
        Result<PagingObject<Instrument>> result = await Request<PagingObject<Instrument>>(HttpMethod.Get, "brokerage", 1, "instruments", id, "?with-attr=orderDimensions&with-attr=fundDistribution&with-attr=derivativeData");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value.Values);
    }
}