using System.Text;

namespace BT.Model.Address;

public class Address : IAddress
{
    public int AddressID { get; set; } = -1;
    public string? Street { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
    public int Zip { get; set; }

    public string AddressBlock()
    {
        if (AddressID == -1) { return string.Empty; }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Street);
            sb.AppendLine($"{City}, {State} {Zip}");
            return sb.ToString();
        }
    }
}
