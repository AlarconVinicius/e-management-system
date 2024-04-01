﻿using EMS.WebApp.MVC.Business.DomainObjects;

namespace EMS.WebApp.MVC.Business.Models;

public class Client : Entity
{
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public Cpf Cpf { get; private set; }
    public Address Address { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; set; }

    protected Client() { }

    public Client(Guid id, string name, string lastName, string email, string phoneNumber, string cpf)
    {
        ChangeId(id);
        Name = name;
        LastName = lastName;
        Email = new Email(email);
        PhoneNumber = phoneNumber;
        Cpf = new Cpf(cpf);
        IsActive = true;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
    public void ChangeLastName(string lastName)
    {
        LastName = lastName;
    }
    public void ChangeEmail(string email)
    {
        Email = new Email(email);
    }
    public void AssignAddress(Address address)
    {
        Address = address;
    }
}