class TaxRules
{
    public static AccountCause[] causes =
    {
        new AccountCause("01", "Estoque"),
        new AccountCause("02", "Industrialização para Estoque"),
        new AccountCause("92", "Retorno de Industrialização para Estoque"),
        new AccountCause("10", "Diverso"),
        new AccountCause("00", "Impostos"),
    };
    public static List<Account> accounts = new List<Account> 
    {
        new Account("110101", "IPI a recuperar", causes[4], false, null, false, null, false, null, null),
        new Account("110102", "ICMS a recuperar", causes[4], false, null, false, null, false, null, null),
        new Account("110103", "PIS a recuperar", causes[4], false, null, false, null, false, null, null),
        new Account("110104", "COFINS a recuperar", causes[4], false, null, false, null, false, null, null)
    };

    public static void CreateAccounts()
    {
        accounts.Add(new Account("310101", "Estoque - Linha produtiva 01", causes[0], 
        true, accounts![0], 
        true, accounts![1], 
        true, accounts![2], accounts![3]));

        accounts.Add(new Account("310102", "Estoque - Linha produtiva 02", causes[0], 
        true, accounts![0], 
        true, accounts![1], 
        true, accounts![2], accounts![3]));

        accounts.Add(new Account("310103", "Estoque - Linha produtiva 03", causes[0], 
        true, accounts![0], 
        true, accounts![1], 
        true, accounts![2], accounts![3]));
    }
}

class Account
{
    public string Number { get; }
    public string Description { get; }
    public AccountCause Cause { get; }

    public bool Ipi { get; }
    public Account? IpiAccount { get; }

    public bool Icms { get; }
    public Account? IcmsAccount { get; }

    public bool PisCofins { get; } 
    public Account? PisAccount { get; }
    public Account? CofinsAccount { get; }

    public Account(string number, 
        string description, 
        AccountCause cause, 
        bool ipi, 
        Account? 
        ipiAccount, 
        bool icms, 
        Account? 
        icmsAccount, 
        bool pisCofins, 
        Account? pisAccount, 
        Account? cofinsAccount)
    {
        if(ipi && ipiAccount == null)
        {
            throw new ArgumentException(
                "IPI account is required when IPI credit is allowed.",
                nameof(ipiAccount)
            );
        }

        if(icms && icmsAccount == null)
        {
            throw new ArgumentException(
                "ICMS account is required when ICMS credit is allowed.", 
                nameof(icmsAccount)
            );
        }

        if(pisCofins && (pisAccount == null || cofinsAccount == null))
        {
            throw new ArgumentException(
                "PIS and COFINS accounts are required when PIS/COFINS credit is allowed.", 
                nameof(ipiAccount)
            );
        }

        Number = number;
        Description = description;
        Cause = cause;

        Ipi = ipi;
        IpiAccount = ipiAccount;

        Icms = icms;
        IcmsAccount = icmsAccount;

        PisCofins = pisCofins;
        PisAccount = pisAccount;
        CofinsAccount = cofinsAccount;
    }
}

class AccountCause
{
    public string Code { get; }
    public string Description { get; }

    public AccountCause(string code, string description)
    {
        if(string.IsNullOrEmpty(code) || string.IsNullOrEmpty(description))
            throw new ArgumentException(
                "Code and Description are required!"
            );

        Code = code;
        Description = description;
    }
}
