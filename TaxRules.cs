class TaxRules
{
    public static MaterialCause[] causes =
    {
        new MaterialCause
        {
            Code = "01",
            Description = "Estoque"
        },

        new MaterialCause
        {
            Code = "02",
            Description = "Industrialização para Estoque"
        },

        new MaterialCause
        {
            Code = "22",
            Description = "Retorno de Industrialização para Estoque"
        },

        new MaterialCause
        {
            Code = "10",
            Description = "Diverso"
        },
    };
    public static List<Account> accounts = new List<Account>
    {
        new Account
        {
            Number = "310101",
            Description = "Estoque - Linha produtiva 01",
            Cause = causes[0],
            Ipi = true,
            Icms = true,
            PisCofins = true,
        },

        new Account
        {
            Number = "310201",
            Description = "Estoque - Linha produtiva 02",
            Cause = causes[0],
            Ipi = true,
            Icms = true,
            PisCofins = true,
        },

        new Account
        {
            Number = "310301",
            Description = "Estoque - Linha produtiva 03",
            Cause = causes[0],
            Ipi = true,
            Icms = true,
            PisCofins = true,
        }
    };
}

class Account
{
    public required string Number;
    public required string Description;
    public required MaterialCause Cause;
    public bool Ipi;
    public bool Icms;
    public bool PisCofins;
}

class MaterialCause
{
    public required string Code;
    public required string Description;
}
