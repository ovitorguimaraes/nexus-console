using System.Xml.Linq;
class NFe
{
    #region invoicePisCofinsWithholding
    public static List<Product> products = new List<Product>();
    #endregion
    #region checkReport
    public static string[]? invoice;
    public static int checkControl;
    public static int xmlNotFound;
    public static int ok;
    public static List<Error> errors = new List<Error>();
    #endregion
    #region xmlsControl
    public static string[] files = Directory.GetFiles("db-xml", "*.xml");
    public static XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
    public static List<XDocument> xmls = new List<XDocument>();
    #endregion
    public static void ProcessNFe(string csvInvoiceNumber, string csvSupplierCnpj, string csvAccount, string csvCostCenter)
    {
        ClearNFe();

        foreach(string file in files)
        {
            XDocument xml = XDocument.Load(file);
            xmls.Add(xml);
        }

        invoice = xmls.Where(xml => xml.Descendants(ns + "nNF").First().Value == csvInvoiceNumber && xml.Descendants(ns + "CNPJ").First().Value == csvSupplierCnpj)
        .Select(xml =>
            {
                double totalProductValue =
                double.Parse(xml.Descendants(ns + "ICMSTot").First().Element(ns + "vProd")!.Value);

                double ipiValue = 0;
                double icmsValue = 0;
                double pisValue = 0;
                double cofinsValue = 0;

                foreach(XElement prod in xml.Descendants(ns + "prod"))
                {
                    Product product = new Product()
                    {
                        Ncm = prod.Element(ns + "NCM")!.Value,
                        Value = double.Parse(prod.Element(ns + "vProd")!.Value)
                    };

                    products.Add(product);
                }

                foreach (Account account in TaxRules.accounts)
                {
                    if (account.Number != csvAccount)
                    {
                        continue;
                    }

                    if (account.Ipi)
                    {
                        ipiValue =
                            double.Parse(xml.Descendants(ns + "vIPI").First().Value);
                    }

                    if (account.Icms)
                    {
                        icmsValue =
                            double.Parse(xml.Descendants(ns + "vICMS").First().Value);
                    }

                    if (account.PisCofins)
                    {
                        pisValue = (totalProductValue - icmsValue) * 0.0165;
                        cofinsValue = (totalProductValue - icmsValue) * 0.0760;
                    }

                    break;
                }

                return new string[]
                {
                    xml.Descendants(ns + "serie").First().Value,
                    xml.Descendants(ns + "nNF").First().Value,
                    xml.Descendants(ns + "CNPJ").First().Value,
                    xml.Descendants(ns + "dhEmi").First().Value.Substring(8, 2).ToString() + "/" 
                        + xml.Descendants(ns + "dhEmi").First().Value.Substring(5, 2) + "/" 
                        + xml.Descendants(ns + "dhEmi").First().Value.Substring(0, 4),
                    "cfop entrada", // ! <- <- <-
                    totalProductValue.ToString(),
                    xml.Descendants(ns + "vNF").First().Value,
                    "amountToPay", // ! REQUIRES THE IsPisCofinsWithholdingRequired FUNCTION
                    ipiValue.ToString(),
                    icmsValue.ToString(),
                    pisValue.ToString(),
                    cofinsValue.ToString(),
                    "account", // ! <- <- <-
                    "cost center", // ! <- <- <-
                };
            })
            .FirstOrDefault();
            
        if(invoice != null)
        {
            CheckNFe();
        }

        else
        {
            checkControl++;
            xmlNotFound++;
        }
    }

    public static void ClearNFe()
    {
        invoice = null;

        products.Clear();

        xmls.Clear();
    }

    public static void CheckNFe()
    {
        string[] report = File.ReadAllLines("reportNFe.csv");

        int invoicePosition = Array.FindIndex(report, reportLine =>
        {
            string[] columns = reportLine.Split(';');

            return columns[1].Trim() == invoice![1] && columns[2].Trim() == invoice![2];
        });

        for(int i = 0; i < invoice!.Length; i++)
        {
            if(invoice[i] != "" && invoice[i] != report[invoicePosition].Split(';')[i])
            {
                checkControl++;
                errors.Add(new Error{Line = invoicePosition, Column = 0, Justification = $"Serie correta da NFe: {invoice[i]}"});
            }
            
            else
            {
                checkControl++;
                ok++;
            }
        }
    }
}

class Product
{
    public required string Ncm { get; set; }
    public double Value {get; set;}
}
class Error
{
    public int Line {get; set; }
    public int Column { get; set; }
    public required string Justification { get; set; }
}