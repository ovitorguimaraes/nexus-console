using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Nexus.UI;
class NFe
{
    #region invoicePisCofinsWithholding
    public static List<Product> products = new List<Product>();
    public static List<WithholdingNcm> withholdingNcms = new List<WithholdingNcm>();
    #endregion
    #region checkReport
    public static string[]? invoice;
    public static int checkControl;
    public static List<int> xmlNotFound = new List<int>();
    public static int ok;
    public static List<Error> errors = new List<Error>();
    #endregion
    #region xmlsControl
    public static string[] files = Directory.GetFiles("db-xml", "*.xml");
    public static XNamespace ns = "http://www.portalfiscal.inf.br/nfe";
    public static List<XDocument> xmls = new List<XDocument>();
    #endregion
    public static void ProcessNFe(string csvInvoiceNumber, string csvSupplierCnpj, string csvAccount, string csvCostCenter, int linePosition)
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
                decimal totalProductValue =
                decimal.Parse(xml.Descendants(ns + "ICMSTot").First().Element(ns + "vProd")!.Value, CultureInfo.InvariantCulture);

                string ipiValue = "0";
                string icmsValue = "0";
                string pisValue = "0";
                string cofinsValue = "0";
                decimal totalWithholdingValue = 0;

                foreach(XElement prod in xml.Descendants(ns + "prod"))
                {
                    Product product = new Product()
                    {
                        Ncm = prod.Element(ns + "NCM")!.Value,
                        Value = decimal.Parse(prod.Element(ns + "vProd")!.Value)
                    };

                    products.Add(product);

                    totalWithholdingValue += NFeRules.WithholdingVerification(product);
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
                            xml.Descendants(ns + "vIPI").First().Value;
                    }

                    if (account.Icms)
                    {
                        icmsValue =
                            xml.Descendants(ns + "vICMS").First().Value;
                    }

                    if (account.PisCofins)
                    {
                        pisValue = ((totalProductValue - decimal.Parse(icmsValue, CultureInfo.InvariantCulture)) * 0.0165m).ToString("F2", CultureInfo.InvariantCulture);
                        cofinsValue = ((totalProductValue - decimal.Parse(icmsValue, CultureInfo.InvariantCulture)) * 0.0760m).ToString("F2", CultureInfo.InvariantCulture);
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
                    xml.Descendants(ns + "CFOP").First().Value,
                    DataFormate(totalProductValue.ToString()),
                    xml.Descendants(ns + "vNF").First().Value,
                    (decimal.Parse(xml.Descendants(ns + "vNF").First().Value, CultureInfo.InvariantCulture) - totalWithholdingValue).ToString(), 
                    ipiValue,
                    icmsValue,
                    pisValue,
                    cofinsValue,
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
            xmlNotFound.Add(linePosition);
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
        int invoicePosition = Array.FindIndex(NFeRules.report, reportLine =>
        {
            string[] columns = reportLine.Split(';');

            return columns[1].Trim() == invoice![1] && columns[2].Trim() == invoice![2];
        });

        bool hasError = false;

        for(int i = 0; i < invoice!.Length; i++)
        {
            if(invoice[i] != "" && DataFormate(invoice[i]) != DataFormate(NFeRules.report[invoicePosition].Split(';')[i]))
            {
                checkControl++;
                hasError = true;
                errors.Add(new Error{Line = invoicePosition, Column = i, ColumnName = NFeRules.report[0].Split(';')[i], Justification = $"Informação correta da NFe: {invoice[i]}"});
            }
            
            else
            {
                checkControl++;
            }
        }

        if(!hasError)
            ok++;
    }

    public static string DataFormate(string data)
    {
        if(data.EndsWith(",00") || data.EndsWith(".00"))
        {
            return data.Substring(0, data.Length - 3);
        }

        return data;
    }
}


class Error
{
    public int Line {get; set; }
    public int Column { get; set; }
    public required string ColumnName { get; set; }
    public required string Justification { get; set; }
}

