using System.Xml.Linq;

class NFe
{
    public static string Serie;
    public static string Number;
    public static List<Product> Products = new List<Product>();
    // FORNECEDOR
    // DATA EMISSA
    public static double TotalProdValue;
    public static double TotalValue;
    public static double ToPaymentTotalValue;
    public static double IpiValue;
    public static double IcmsValue;
    public static double PisValue;
    public static double CofinsValue;
    public static XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
    public static List<XDocument> xmls = new List<XDocument>();
    public static string[] files = Directory.GetFiles("/Users/ovitorguimaraes/Documents/GitHub/nexus-console/db-xmls");
    public static void ProcessingNFe(string csvNumber, string csvAccount, string csvCostCenter) // preciso de uma List de produtos da NFe
    {

        foreach(string file in files)
        {
            XDocument xml = XDocument.Load(file);
            xmls.Add(xml);
        }

        int i = 0;
        foreach(Account GlAccount in TaxRules.accounts)
        {
            if(GlAccount.Number == csvAccount)
            {
                break;
            }

            i++;
        }

        int j = 0;
        foreach(XDocument xml in xmls)
        {
            number = xml.Descendants(ns + "nNF").First().Value;
            if(number == TaxRules.accounts[i].Number) // -> Isso está completamente errado, preciso ajustar, o número deve ser comparado ao número do CSV, e não ao número da conta contábil.
            {
                serie = xml.Descendants(ns + "serie").First().Value;
                totalValue = double.Parse(xml.Descendants(ns + "vNF").First().Value);
                break;
            }

            j++;
        }

        totalProdValue = double.Parse(xmls[j].Descendants(ns + "vProd").First().Value); // preciso pegar vProd de total

        if (TaxRules.accounts[i].Ipi)
        {
            ipiValue = double.Parse(xmls[j].Descendants(ns + "vIPI").First().Value);
        }

        if (TaxRules.accounts[i].Icms)
        {
            icmsValue = double.Parse(xmls[j].Descendants(ns + "vICMS").First().Value);
        }

        if (TaxRules.accounts[i].PisCofins)
        {
            pisValue = totalProdValue - icmsValue * 0.0165;
            cofinsValue = totalProdValue - icmsValue * 0.0760;
        }

        foreach(XElement prod in xmls[j].Descendants(ns + "prod")) // pra cada ncm in xml && pra cada vProd in prod in XML
        {
            foreach(XElement ncm in prod)
            {
                new Product product = 
                {
                    Ncm = 
                    Value =
                }

                Products.Add(product);
            }
        }
    }
}

class Product
{
    public string Ncm;
    public double Value;

}