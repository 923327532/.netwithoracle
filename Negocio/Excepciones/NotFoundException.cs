using System;

namespace Taller.Negocio.Excepciones;

public class NotFoundException : Exception
{
    public NotFoundException(string mensaje) : base(mensaje)
    {
    }

    public NotFoundException(string mensaje, Exception innerException) : base(mensaje, innerException)
    {
    }
}