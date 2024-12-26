using System.Text.Json.Serialization;
using Comdirect.NET.Models.Accounting;

namespace Comdirect.NET.Models;

public interface IFakeEnum<T> where T : struct, Enum {
    public T Short { get; set; }
    public string Description { get; set; }
}