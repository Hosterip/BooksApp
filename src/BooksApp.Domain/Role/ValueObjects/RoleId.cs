﻿using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Role.ValueObjects;

public class RoleId : ValueObject
{
    private RoleId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static RoleId Create(Guid? value = null)
    {
        return new RoleId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}