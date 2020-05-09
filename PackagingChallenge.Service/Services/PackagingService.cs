using PackagingChallenge.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using PackageChallenge.Model.Models;
using System.Linq;

namespace PackagingChallenge.Service.Services
{
    public class PackagingService : IPackagingService
    {
        public ResponseModel<List<List<string>>> ProcessPackages(List<Package> packages)
        {
            var response = new ResponseModel<List<List<string>>>() { Success = true };
            try
            {
                var indexesList = new List<List<string>>();
                foreach (var package in packages)
                {
                    string errorMessage;
                    bool validationResult = ValidatePackage(package, out errorMessage);
                    if (!validationResult)
                    {
                        response.Success = false;
                        response.Message = errorMessage;
                        return response;
                    }
                    var allowedPackageWeight = package.Weight;
                    var items = package.Items;
                    
                    //get all package items that can potentially go in main package
                    var packageWeightItems = GetAllItemsThatFitPackage(package, new List<PackageItem>(), new List<List<PackageItem>>());

                    //get the ones that have highest price and lowest weight (if price is same)
                    var maxPriceItems = packageWeightItems.OrderByDescending(item => item.Sum(p => p.Price))
                                        .ThenBy(item => item.Sum(w => w.Weight)).FirstOrDefault();
                  
                    indexesList.Add(new List<string>() { string.Join(",", maxPriceItems.Select(mpi => mpi.Index.ToString())) });

                    
                }
                response.Data = indexesList;
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        private bool ValidatePackage(Package package, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (package.Weight > 100) {
                errorMessage = "Weight of package must be bellow 100.";
                return false;
            }
            if (package.Items.Count > 15) {
                errorMessage = "Maximum of 15 items is allowed.";
                return false;
            }
            if(package.Items.Any(item => item.Weight > 100))
            {
                errorMessage = "All items must be bellow 100.";
                return false;
            }
            return true;
        }

        private List<List<PackageItem>> GetAllItemsThatFitPackage(Package packageModel, List<PackageItem> temporaryList, List<List<PackageItem>> resultList)
        {
            var allowedWeight = packageModel.Weight;
            var itemsWeight = packageModel.Items.ToList();
            decimal totalWeight = 0;
            foreach (var item in temporaryList) 
            {
                totalWeight += item.Weight;
            }

            if (totalWeight <= allowedWeight)
                resultList.Add(temporaryList);

            if (totalWeight >= allowedWeight)
                return resultList;

            for (int i = 0; i < itemsWeight.Count; i++)
            {
                var newPackage = new Package() { Weight = allowedWeight };

                var currentItem = itemsWeight[i];
               

                for (int j = i + 1; j < itemsWeight.Count; j++)
                {
                    newPackage.Items.Add(new PackageItem() 
                    {   Weight = itemsWeight[j].Weight,
                        Index = itemsWeight[j].Index,
                        Price = itemsWeight[j].Price
                    });
                }
                List<PackageItem> temporaryItems = new List<PackageItem>(temporaryList);
                temporaryItems.Add(currentItem);
                GetAllItemsThatFitPackage(newPackage, temporaryItems, resultList);
            }

            return resultList;
        }

    }
}
