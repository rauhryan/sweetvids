using System;

namespace SweetVids.Web.Conventions
{
    public interface IRequestById
    {
        Guid Id { get; }
    }
}