using PackageChallenge.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackagingChallenge.Service.Interfaces
{
    public interface IPackagingService
    {
        ResponseModel<List<List<string>>> ProcessPackages(List<Package> packages);
    }
}
