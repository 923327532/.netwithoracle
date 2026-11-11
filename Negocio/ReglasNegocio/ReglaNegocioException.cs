using System;

namespace Taller.Negocio.ReglasNegocio;

public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje)
    {
    }

    public ReglaNegocioException(string mensaje, Exception innerException) : base(mensaje, innerException)
    {
    }
}