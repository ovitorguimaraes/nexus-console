class NFe
{
    public static void ProcessingNFe(string account, string costCenter)
    {
        int i = 0;
        foreach(Account GlAccount in TaxRules.accounts)
        {
            if(GlAccount.Number == account)
            {
                break;
            }

            i++;
        }

        if (TaxRules.accounts[i].Ipi)
        {
            
        }

        if (TaxRules.accounts[i].Icms)
        {
            
        }

        if (TaxRules.accounts[i].PisCofins)
        {
            
        }
    }
}