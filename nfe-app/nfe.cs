using System.Xml.Linq;
class NFe
{
    public static XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
    public static List<XDocument> xmls = new List<XDocument>();
    public static string[] files = Directory.GetFiles("/Users/ovitorguimaraes/Documents/GitHub/nexus-console/db-xmls");
    public static void ProcessNFe(string csvInvoiceNumber, string csvSupplierCnpj, string csvAccount, string csvCostCenter)
    {
        string serie = "";
        string number = "";
        string supplierCnpj = "";

        double totalProductValue = 0;
        double totalValue = 0;
        double amountToPay = 0; // -> FUNCTION CALCRETENCION IS REQUIRED
        double ipiValue = 0;
        double icmsValue = 0;
        double pisValue = 0;
        double cofinsValue = 0;

        List<Product> products = new List<Product>();
        Date issueDate;

        foreach(string file in files)
        {
            XDocument xml = XDocument.Load(file);
            xmls.Add(xml);
        }

        foreach(XDocument xml in xmls)
        {
            number = xml.Descendants(ns + "nNF").First().Value;
            supplierCnpj = xml.Descendants(ns + "CNPJ").First().Value;
            
            if(number == csvInvoiceNumber && supplierCnpj == csvSupplierCnpj)
            {
                serie = xml.Descendants(ns + "serie").First().Value;
                totalProductValue = double.Parse(xml.Descendants(ns + "total").First().Element(ns + "vProd").Value); 
                totalValue = double.Parse(xml.Descendants(ns + "vNF").First().Value);
                issueDate = new Date
                {
                    Day = xml.Descendants(ns + "dhEmi").First().Value.Substring(8, 2),
                    Month = xml.Descendants(ns + "dhEmi").First().Value.Substring(5, 2),
                    Year = xml.Descendants(ns + "dhEmi").First().Value.Substring(0, 4),
                };

                        
                foreach(XElement prod in xml.Descendants(ns + "prod"))
                {
                    Product product = new Product()
                    {
                        Ncm = prod.Element(ns + "NCM").Value,
                        Value = double.Parse(prod.Element(ns + "vProd").Value)
                    };

                    products.Add(product);
                }

                foreach(Account account in TaxRules.accounts)
                {
                    if(account.Number != csvAccount)
                    {
                        continue;
                    }

                    if (account.Ipi)
                    {
                        ipiValue = double.Parse(xml.Descendants(ns + "vIPI").First().Value);
                    }

                    if (account.Icms)
                    {
                        icmsValue = double.Parse(xml.Descendants(ns + "vICMS").First().Value);
                    }

                    if (account.PisCofins)
                    {
                        pisValue = (totalProductValue - icmsValue) * 0.0165;
                        cofinsValue = (totalProductValue - icmsValue) * 0.0760;
                    }

                    break;
                }

                break;
            }

            else
            {
                number = "";
                supplierCnpj = "";
            }
        }
    }
}

class Product
{
    public string Ncm;
    public double Value;

}

class Date
{
    public string Day;
    public string Month;
    public string Year;
}