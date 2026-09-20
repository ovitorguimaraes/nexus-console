class NFeRules
{
    public static string[] report = File.ReadAllLines("reportNFe.csv");
    private static List<WithholdingNcm> _withholdingNcms = new List<WithholdingNcm>()
    {
        // ANEXO I - Lei 10.485/2002

        new WithholdingNcm("4016.10.10", false, null),
        new WithholdingNcm("4016.99.90", true, "Somente Ex 03 e Ex 05"),
        new WithholdingNcm("68.13", false, null),
        new WithholdingNcm("7007.11.00", false, null),
        new WithholdingNcm("7007.21.00", false, null),
        new WithholdingNcm("7009.10.00", false, null),
        new WithholdingNcm("7320.10.00", true, "Somente Ex 01"),
        new WithholdingNcm("8301.20.00", false, null),
        new WithholdingNcm("8302.30.00", false, null),
        new WithholdingNcm("8407.33.90", false, null),
        new WithholdingNcm("8407.34.90", false, null),
        new WithholdingNcm("8408.20", false, null),
        new WithholdingNcm("8409.91", false, null),
        new WithholdingNcm("8409.99", false, null),
        new WithholdingNcm("8413.30", false, null),
        new WithholdingNcm("8413.91.00", true, "Somente Ex 01"),
        new WithholdingNcm("8414.80.21", false, null),
        new WithholdingNcm("8414.80.22", false, null),
        new WithholdingNcm("8415.20", false, null),
        new WithholdingNcm("8421.23.00", false, null),
        new WithholdingNcm("8421.31.00", false, null),
        new WithholdingNcm("8431.41.00", false, null),
        new WithholdingNcm("8431.42.00", false, null),
        new WithholdingNcm("8433.90.90", false, null),
        new WithholdingNcm("8481.80.99", true, "Somente Ex 01 e Ex 02"),
        new WithholdingNcm("8483.10", false, null),

        new WithholdingNcm("8483.20.00", false, null),
        new WithholdingNcm("8483.30", false, null),
        new WithholdingNcm("8483.40", false, null),
        new WithholdingNcm("8483.50", false, null),
        new WithholdingNcm("8505.20", false, null),
        new WithholdingNcm("8507.10.00", false, null),
        new WithholdingNcm("85.11", false, null),
        new WithholdingNcm("8512.20", false, null),
        new WithholdingNcm("8512.30.00", false, null),
        new WithholdingNcm("8512.40", false, null),
        new WithholdingNcm("8512.90.00", false, null),
        new WithholdingNcm("8527.2", false, null),
        new WithholdingNcm("8539.10", false, null),
        new WithholdingNcm("8544.30.00", false, null),
        new WithholdingNcm("8706.00", false, null),
        new WithholdingNcm("87.07", false, null),
        new WithholdingNcm("87.08", false, null),
        new WithholdingNcm("9029.20.10", false, null),
        new WithholdingNcm("9029.90.10", false, null),
        new WithholdingNcm("9030.39.21", false, null),
        new WithholdingNcm("9031.80.40", false, null),
        new WithholdingNcm("9032.89.2", false, null),
        new WithholdingNcm("9104.00.00", false, null),
        new WithholdingNcm("9401.20.00", false, null),

        // ANEXO II - Lei 10.485/2002
        // Condições de destinação/aplicação.

        new WithholdingNcm(
            "40.09",
            true,
            "Tubos de borracha vulcanizada não endurecida, com acessórios, próprios para as máquinas e veículos previstos no Anexo II"
        ),

        new WithholdingNcm(
            "84.31",
            true,
            "Somente partes reconhecíveis como exclusiva ou principalmente destinadas às máquinas da posição 84.29"
        ),

        new WithholdingNcm(
            "8408.90.90",
            true,
            "Somente motores próprios para as máquinas indicadas no item 3 do Anexo II"
        ),

        new WithholdingNcm(
            "8412.21.10",
            true,
            "Somente cilindros hidráulicos próprios para as máquinas indicadas no item 4 do Anexo II"
        ),

        new WithholdingNcm(
            "8412.21.90",
            true,
            "Somente outros motores hidráulicos de movimento retilíneo próprios para as máquinas indicadas no item 5 do Anexo II"
        ),

        new WithholdingNcm(
            "8412.31.10",
            true,
            "Somente cilindros pneumáticos próprios para produtos 8701.20.00, 87.02 e 87.04"
        ),

        new WithholdingNcm(
            "8413.60.19",
            true,
            "Somente bombas volumétricas rotativas próprias para os produtos indicados no item 7 do Anexo II"
        ),

        new WithholdingNcm(
            "8414.80.19",
            true,
            "Somente compressores de ar próprios para produtos 8701.20.00, 87.02 e 87.04"
        ),

        new WithholdingNcm(
            "8414.90.39",
            true,
            "Somente caixas de ventilação para veículos autopropulsados"
        ),

        new WithholdingNcm(
            "8432.90.00",
            true,
            "Somente partes de máquinas das posições 8432.40.00 e 8432.80.00"
        ),

        new WithholdingNcm(
            "8481.10.00",
            true,
            "Somente válvulas redutoras de pressão próprias para as máquinas e veículos indicados no item 11 do Anexo II"
        ),

        new WithholdingNcm(
            "8481.20.90",
            true,
            "Somente válvulas para transmissões óleo-hidráulicas ou pneumáticas próprias para as máquinas indicadas no item 12 do Anexo II"
        ),

        new WithholdingNcm(
            "8481.80.92",
            true,
            "Somente válvulas solenóides próprias para as máquinas e veículos indicados no item 13 do Anexo II"
        ),

        new WithholdingNcm(
            "8483.60.1",
            true,
            "Somente embreagens de fricção próprias para as máquinas indicadas no item 14 do Anexo II"
        ),

        new WithholdingNcm(
            "8501.10.19",
            true,
            "Somente motores de corrente contínua próprios para acionamento elétrico de vidros de veículos autopropulsados"
        )
    };

    public static IReadOnlyList<WithholdingNcm> WithholdingNcms
    {
        get
        {
            return _withholdingNcms;
        }
    }

    public static decimal WithholdingVerification(Product product)
    {
        if(WithholdingNcms.Any(ncm => ncm.Ncm == product.Ncm))
        {
            return product.Value * 0.006m;
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
    public string Ncm { get; }
    public bool HasException { get; }
    public string? Exception { get; }

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