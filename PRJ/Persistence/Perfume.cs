using System;

namespace Persistence
{
    public static class ProductStatus{
        public const int NOT_ACTIVE = 0;
        public const int ACTIVE = 1;
    }
    
    public class Perfume
    {
        public int? Perfume_ID {set;get;}
        public string PerfumeName {set;get;}
        public string FragranceFamily {set;get;}
        public string Classification {set;get;}
        public string Volume {set;get;}
        public string TopNotes {set;get;}
        public string HeartNotes {set;get;}
        public string BaseNotes {set;get;}
        public string Gender {set;get;}
        public string Ingredients {set;get;}
        public string Form {set;get;}
        public int? YearLaunched {set;get;}
        public string Strength {set;get;}
        public string Origin {set;get;}
        public decimal PerfumePrice {set;get;}
        public int? TotalQuantity {set;get;}
        public int? Status{set;get;}
        public string Description {set;get;}

        public override bool Equals(object obj)
        {
            if(obj is Perfume)
            {
                return ((Perfume)obj).Perfume_ID.Equals(Perfume_ID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Perfume_ID.GetHashCode();
        }
    }
}