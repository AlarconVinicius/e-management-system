using EMS.Core.Utils;

namespace EMS.Core.DomainObjects;

public class Cpf
{
    public const int MaxCpfLength = 11;
    public string Number { get; private set; }

    protected Cpf() { }

    public Cpf(string number)
    {
        if (!Validate(number)) throw new DomainException("Invalid CPF");
        Number = number;
    }

    public static bool Validate(string cpf)
    {
        cpf = cpf.OnlyNumbers(cpf);

        if (cpf.Length > MaxCpfLength)
            return false;

        while (cpf.Length != MaxCpfLength)
            cpf = '0' + cpf;

        var areAllDigitsEqual = true;
        for (var i = 1; i < MaxCpfLength && areAllDigitsEqual; i++)
            if (cpf[i] != cpf[0])
                areAllDigitsEqual = false;

        if (areAllDigitsEqual || cpf == "12345678909")
            return false;

        var numbers = new int[MaxCpfLength];

        for (var i = 0; i < MaxCpfLength; i++)
            numbers[i] = int.Parse(cpf[i].ToString());

        var sum = 0;
        for (var i = 0; i < MaxCpfLength - 2; i++)
            sum += ((MaxCpfLength - 1) - i) * numbers[i];

        var result = sum % 11;

        if (result == 1 || result == 0)
        {
            if (numbers[9] != 0)
                return false;
        }
        else if (numbers[9] != 11 - result)
            return false;

        sum = 0;
        for (var i = 0; i < MaxCpfLength - 1; i++)
            sum += (MaxCpfLength - i) * numbers[i];

        result = sum % 11;

        if (result == 1 || result == 0)
        {
            if (numbers[10] != 0)
                return false;
        }
        else if (numbers[10] != 11 - result)
            return false;

        return true;
    }
}