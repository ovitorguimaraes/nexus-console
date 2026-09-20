class TaxRules
{
    private static AccountCause[] _causes = 
    {
        new AccountCause("01", "Estoque"),
        new AccountCause("02", "Industrialização para Estoque"),
        new AccountCause("92", "Retorno de Industrialização para Estoque"),
        new AccountCause("10", "Diverso"),
        new AccountCause("00", "Impostos"),
    };
    private static List<Account> _accounts = new List<Account> 
    {
        new Account("111103", "IPI a recuperar - Estoque", _causes[4], false, null, false, null, false, null, null),
        new Account("112103", "ICMS a recuperar - Estoque", _causes[4], false, null, false, null, false, null, null),
        new Account("113103", "PIS a recuperar - Estoque", _causes[4], false, null, false, null, false, null, null),
        new Account("114103", "COFINS a recuperar - Estoque", _causes[4], false, null, false, null, false, null, null),
        new Account("213103", "PIS retido a recolher", _causes[4], false, null, false, null, false, null, null),
        new Account("214103", "COFINS retida a recolher", _causes[4], false, null, false, null, false, null, null)
    };

    public static IReadOnlyList<Account> Accounts
    {
        get
        {
            return _accounts;
        }
    }

    static void CreateAccounts()
    {
        _accounts.Add(new Account("310101", "Estoque - Linha produtiva 01", _causes[0], 
        true, _accounts[0], 
        true, _accounts[1], 
        true, _accounts[2], _accounts[3]));

        _accounts.Add(new Account("310201", "Estoque - Linha produtiva 02", _causes[0], 
        true, _accounts[0], 
        true, _accounts[1], 
        true, _accounts[2], _accounts[3]));

        _accounts.Add(new Account("310301", "Estoque - Linha produtiva 03", _causes[0], 
        true, _accounts[0], 
        true, _accounts[1], 
        true, _accounts[2], _accounts[3]));
    }

    static TaxRules()
    {
        CreateAccounts();
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
                "PIS and COFINS accounts are required when PIS/COFINS credit is allowed."
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
