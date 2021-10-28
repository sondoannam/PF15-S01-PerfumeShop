using System;
using System.Collections.Generic;
using Persistence;
using DAL;

namespace BL
{
    public class ItemBL
    {
        private ItemDal idal = new ItemDal();

        public Perfume GetItemByID(int perfumeID)
        {
            return idal.GetItemByID(perfumeID);
        }

        public List<Perfume> GetAll()
        {
            return idal.GetItems(ItemMatch.GET_ALL, null);
        }

        public List<Perfume> GetByName(string perfumeName)
        {
            return idal.GetItems(ItemMatch.MATCH_BY_NAME, new Perfume{PerfumeName = perfumeName});
        }

        public List<Perfume> GetByGender(string perfumeGender)
        {
            return idal.GetItems(ItemMatch.MATCH_BY_GENDER, new Perfume{Gender = perfumeGender});
        }

        public List<Perfume> GetByBrand(string brandName)
        {
            return idal.GetItems(ItemMatch.MATCH_BY_BRAND, new Perfume{BrandName = brandName});
        }

        
    }
}