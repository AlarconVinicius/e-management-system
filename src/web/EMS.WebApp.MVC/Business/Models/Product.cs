﻿namespace EMS.WebApp.MVC.Business.Models;

public class Product : Entity
{
    public Guid CompanyId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal UnitaryValue { get; private set; }
    public string Image { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; set; }

    public Product() { }

    public Product(Guid companyId, string title, string description, decimal unitaryValue, string image, bool isActive)
    {
        CompanyId = companyId;
        Title = title;
        Description = description;
        UnitaryValue = unitaryValue;
        Image = image;
        IsActive = isActive;
    }
}