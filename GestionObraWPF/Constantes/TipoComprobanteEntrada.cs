using System.ComponentModel;

namespace GestionObraWPF.Constantes
{
    public enum TipoComprobanteEntrada
    {
        [Description("Factura A")]
        A = 1,
        [Description("Nota de debito")]
        NotaDebito = 2,
        Recibo = 3,
        Cheque = 4,
        Ninguno = 5,
        Otro = 6
    }
}