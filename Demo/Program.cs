using Comdirect.NET;
using Comdirect.NET.Models.Accounting;
using Comdirect.NET.Models.Depot;
using FluentResults;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Demo;

class Program {
    private const string CredentialsFile = "credentials.yaml";
    
    static async Task Main(string[] args) {
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, _) => cts.Cancel();
        
        var deserializer = new DeserializerBuilder()
                           .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                           .Build();

        if (!File.Exists(CredentialsFile)) {
            PrintCredentialsHelp();
            return;
        }

        string yml = await File.ReadAllTextAsync(CredentialsFile, cts.Token);
        var credentials = deserializer.Deserialize<Credentials>(yml);
        
        if (credentials.ClientId is null || credentials.ClientSecret is null || credentials.Username is null || credentials.Pin is null ) {
            PrintCredentialsHelp();
            return;
        }
        
        var comdirectClient
            = new ComdirectClient(credentials.ClientId, credentials.ClientSecret);
        
        Result result = await comdirectClient.FullLogin(credentials.Username, credentials.Pin, cts.Token);
        
        comdirectClient.KeepAlive(cts.Token);

        var balances = await comdirectClient.GetBalances();

        if (balances.IsFailed) {
            Console.WriteLine(balances);
            return;
        }

        foreach (AccountBalance balance in balances.Value) {
            Console.WriteLine($"{balance.Account.Type.Description} ({balance.Account.Iban}): {balance.BalanceEur.Value} EUR");
        }
        
        Console.WriteLine("---- Transactions ----");
        foreach (AccountBalance balance in balances.Value) {
            Console.WriteLine($"transactions for {balance.Account.Type.Description}");
            Result<IReadOnlyCollection<AccountTransaction>> transactionsResult = await comdirectClient.GetTransactions(balance.Account);
            if (result.IsFailed) {
                Console.WriteLine(transactionsResult);
                break;
            }

            IReadOnlyCollection<AccountTransaction>? transactions = transactionsResult.Value;

            foreach (AccountTransaction transaction in transactions) {
                Console.WriteLine($"{transaction.TransactionType.Description} on {transaction.Date} to/from {transaction.Remitter.HolderName} with {transaction.Amount.Value} {transaction.Amount.Unit}");
            }
            Console.WriteLine();
        }
        
        
        Console.WriteLine("---- Depots ----");
        var depots = await comdirectClient.GetDepots();
        if (depots.IsFailed) {
            Console.WriteLine(depots);
            return;
        }

        foreach (Depot depot in depots.Value) {
            Console.WriteLine($"{depot.Id}");
        }
    }

    private static void PrintCredentialsHelp() {
        Console.WriteLine($"Please create the config file '{CredentialsFile}'");
        Console.WriteLine($"Content:");
        Console.WriteLine($"client_id: [Client Id]");
        Console.WriteLine($"client_secret: [Client secret]");
        Console.WriteLine($"username: [Username]");
        Console.WriteLine($"pin: [Pin/Password]");
    }
}