using Comdirect.NET.Models;
using Comdirect.NET.Models.Accounting;
using FluentResults;

namespace Comdirect.NET;

public partial class ComdirectClient {
    public async Task<Result<IReadOnlyCollection<AccountBalance>>> GetBalances() {
        Result<PagingObject<AccountBalance>> result = await Request<PagingObject<AccountBalance>>(HttpMethod.Get, $"api/banking/clients/user/v2/accounts/balances");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value.Values);
    }
    
    public async Task<Result<AccountBalance>> GetBalance(Account account) {
        Result<AccountBalance> result = await Request<AccountBalance>(HttpMethod.Get, "banking", 2, "accounts", account.Id, "balances");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value);
    }
    public async Task<Result<IReadOnlyCollection<AccountTransaction>>> GetTransactions(Account account) {
        var result = await Request<PagingObject<AccountTransaction>>(HttpMethod.Get, "banking", 1, "accounts", account.Id, "transactions");
        return result.IsFailed ? result.ToResult() : Result.Ok(result.Value.Values);
    }
}