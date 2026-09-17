class NFeRules
{
    public static List<WithholdingNcm> WithholdingNcms = new List<WithholdingNcm>()
    {
        // ! <- NCMS HERE
    };

    public static decimal WithholdingVerification(Product product)
    {
        if(WithholdingNcms.Any(ncm => ncm.Ncm == product.Ncm))
        {
            return product.Value - (product.Value * 0.006m);
        }
        
        return 0;
    }
}

class Product
{
    public required string Ncm { get; set; }
    public decimal Value {get; set;}
}

class WithholdingNcm
{
    public required string Ncm { get; set; }
    public bool HasException { get; set; }
    public string? Exception { get; set;}

    public WithholdingNcm(string ncm, bool hasException, string? exception)
    {
        if(hasException && string.IsNullOrWhiteSpace(exception))
            throw new ArgumentException(
                "Exception description is required when HasException is true.",
                nameof(exception)
            );

        Ncm = ncm;
        HasException = hasException;
        Exception = exception;
    }
}