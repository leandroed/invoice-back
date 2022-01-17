using InvoiceApi.Models;
using Newtonsoft.Json;

namespace InvoiceApi.Tests;

/// <summary>
/// Invoice mock class.
/// </summary>
public class InvoiceMock
{
    /// <summary>
    /// Sample nfe json to use in tests.
    /// </summary>
    /// <value>Nfe json string.</value>
    private readonly string SampleNfeJson = "{\"nfeProc\":{\"@versao\":\"4.00\",\"@xmlns\":\"http://www.portalfiscal.inf.br/nfe\",\"NFe\":{\"infNFe\":{\"@Id\":\"61.9898\",\"@versao\":\"4.00\",\"ide\":{\"cUF\":\"31\",\"cNF\":\"00000001\",\"natOp\":\"VENDA DE MERCADORIA ADQUIRIDA OU RECEBIDA DE TERCEIROS, EFET\",\"mod\":\"55\",\"serie\":\"95\",\"nNF\":\"1\",\"dhEmi\":\"2021-11-28T14:02:01.548Z\",\"tpNF\":\"1\",\"idDest\":\"1\",\"cMunFG\":\"3166600\",\"tpImp\":\"1\",\"tpEmis\":\"1\",\"cDV\":\"7\",\"tpAmb\":\"1\",\"finNFe\":\"1\",\"indFinal\":\"1\",\"indPres\":\"1\",\"procEmi\":\"0\",\"verProc\":\"4.00\",\"dhSaiEnt\":\"2021-11-28T22:21:06.877Z\"},\"emit\":{\"CNPJ\":\"4b653228-585a-4ddf-b7ab-b1e6d0557624\",\"xNome\":\"Tasha Bashirian\",\"xFant\":\"Pacocha\",\"enderEmit\":{\"xLgr\":\"Chadd Rue\",\"nro\":27348,\"xBairro\":\"Reingertown\",\"cMun\":\"3550308\",\"xMun\":\"Serra da Saudade\",\"UF\":\"MG\",\"CEP\":\"39187-8404\",\"cPais\":\"1058\",\"xPais\":\"BRASIL\"},\"IE\":\"47198424-3d75-42cf-b454-0800abab5029\",\"CRT\":\"3\"},\"dest\":{\"CPF\":\"dda88694-63e8-48ac-8695-eda26940522a\",\"xNome\":\"Megane\",\"enderDest\":{\"xLgr\":\"Isadore Bypass\",\"nro\":2887,\"xCpl\":\"CASA\",\"xBairro\":\"Harveyside\",\"cMun\":\"3166600\",\"xMun\":\"Serra da Saudade\",\"UF\":\"MG\",\"CEP\":\"14410\",\"cPais\":\"1058\",\"xPais\":\"BRASIL\",\"fone\":\"437.269.3501 x40907\"},\"indIEDest\":\"9\"},\"det\":[{\"@nItem\":9616,\"prod\":{\"cProd\":\"879f6c27-8862-4097-9f08-c777d0f2c147\",\"cEAN\":\"3e43d659-4736-498a-bbae-a2b73683f6e0\",\"xProd\":\"Ferrari Charger\",\"NCM\":14884,\"CFOP\":20683,\"uCom\":\"UN\",\"qCom\":20715,\"vUnCom\":8433,\"vProd\":5668,\"cEANTrib\":31346,\"uTrib\":\"UN\",\"qTrib\":17561,\"vUnTrib\":30602,\"indTot\":\"1\",\"preco\":\"949.00\",\"marca\":\"Volvo\"}},{\"@nItem\":22054,\"prod\":{\"cProd\":\"faad81f3-36e2-40f2-ad5a-e415b99fa5e2\",\"cEAN\":\"f0fa82ef-8995-47d4-b466-0e0bb655a5aa\",\"xProd\":\"Dodge Land Cruiser\",\"NCM\":21519,\"CFOP\":27811,\"uCom\":\"UN\",\"qCom\":18135,\"vUnCom\":22330,\"vProd\":32015,\"cEANTrib\":28120,\"uTrib\":\"UN\",\"qTrib\":11642,\"vUnTrib\":11146,\"indTot\":\"1\",\"preco\":\"845.00\",\"marca\":\"Kia\"}}]}}},\"id\":\"2\"}";

    /// <summary>
    /// Gets the invoice content mock.
    /// </summary>
    /// <value>Invoice content.</value>
    public InvoiceContent InvoiceContentMock
    {
        get
        {
            return JsonConvert.DeserializeObject<InvoiceContent>(this.SampleNfeJson);
        }
    }
}
