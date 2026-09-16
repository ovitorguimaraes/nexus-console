using System.Xml.Linq;
class NFe
{
    #region invoiceValues
    public static string series;
    public static string number;
    public static string supplierCnpj;

    public static double totalProductValue;
    public static double totalValue;
    public static double amountToPay; // <- REQUIRES THE IsPisCofinsWithholdingRequired FUNCTION
    public static double ipiValue;
    public static double icmsValue;
    public static double pisValue;
    public static double cofinsValue;

    public static List<Product> products = new List<Product>();
    public static Date issueDate;
    #endregion
    #region checkReport
    public static string[] invoice = new string[14];
    public static int checkControl;
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

        foreach(XDocument xml in xmls) // <- USE LINQ AND LAMBDA TO SIMPLIFY
        {
            number = xml.Descendants(ns + "nNF").First().Value;
            supplierCnpj = xml.Descendants(ns + "CNPJ").First().Value;
            
            if(number == csvInvoiceNumber && supplierCnpj == csvSupplierCnpj)
            {
                series = xml.Descendants(ns + "serie").First().Value;
                totalProductValue = double.Parse(xml.Descendants(ns + "ICMSTot").First().Element(ns + "vProd").Value); 
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

        // THIS WILL BE SUBSTITUTED =>
        invoice[0] = series;
        invoice[1] = number;
        invoice[2] = supplierCnpj;
        invoice[3] = issueDate.ToString();
        invoice[4] = "cfop entrada"; // <-
        invoice[5] = totalProductValue.ToString();
        invoice[6] = totalValue.ToString();
        invoice[7] = amountToPay.ToString();
        invoice[8] = ipiValue.ToString();
        invoice[9] = icmsValue.ToString();
        invoice[10] = pisValue.ToString();
        invoice[11] = cofinsValue.ToString();
        invoice[12] = "account"; // <-
        invoice[13] = "cost center"; // <-
        //
    }

    public static void ClearNFe()
    {
        invoice = new string[11];

        series = "";
        number = "";
        supplierCnpj = "";
        totalProductValue = 0;
        totalValue = 0;
        amountToPay = 0; // <- REQUIRES THE IsPisCofinsWithholdingRequired FUNCTION
        ipiValue = 0;
        icmsValue = 0;
        pisValue = 0;
        cofinsValue = 0;
        issueDate = new Date();

        products.Clear();

        xmls.Clear();
    }

    public static void CheckNFe()
    {
        string[] report = File.ReadAllLines("reportNFe.csv");

        int invoicePosition = Array.FindIndex(report, reportLine =>
        {
            string[] columns = reportLine.Split(';');

            return columns[1].Trim() == number && columns[2].Trim() == supplierCnpj;
        });

        for(int i = 0; i < invoice.Length; i++)
        {
            if(invoice[i] != "" && invoice[i] != report[invoicePosition].Split(';')[i])
            {
                checkControl++;
                errors.Add(new Error{Line = invoicePosition, Column = 0, Justification = $"Serie correta da NFe: {series}"});
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
    public string Ncm;
    public double Value;
}

class Date
{
    public string Day;
    public string Month;
    public string Year;
}

class Error
{
    public int Line;
    public int Column;
    public string Justification;
}